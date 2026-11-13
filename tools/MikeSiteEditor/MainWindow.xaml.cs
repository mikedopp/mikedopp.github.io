using Markdig;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace MikeSiteEditor;

public partial class MainWindow : Window
{
    private static readonly Encoding Utf8NoBom = new UTF8Encoding(false);
    private readonly MarkdownPipeline _markdownPipeline;
    private readonly DispatcherTimer _blogPreviewTimer;
    private readonly DispatcherTimer _pagePreviewTimer;
    private readonly ObservableCollection<ProjectLink> _projects = [];
    private PageDocument? _currentPage;
    private bool _loadingPage;
    private string _repoRoot = "";

    public MainWindow()
    {
        InitializeComponent();

        _markdownPipeline = new MarkdownPipelineBuilder()
            .UseAdvancedExtensions()
            .Build();

        _blogPreviewTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(350) };
        _blogPreviewTimer.Tick += (_, _) =>
        {
            _blogPreviewTimer.Stop();
            RefreshBlogPreview();
        };

        _pagePreviewTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(350) };
        _pagePreviewTimer.Tick += (_, _) =>
        {
            _pagePreviewTimer.Stop();
            RefreshPagePreview();
        };

        Loaded += (_, _) => InitializeEditor();
    }

    private void InitializeEditor()
    {
        try
        {
            _repoRoot = FindRepositoryRoot();
            RepoPathText.Text = _repoRoot;
            VersionText.Text = GetVersionText();
            LastActionText.Text = "Ready";
            ProjectsGrid.ItemsSource = _projects;
            BlogDatePicker.SelectedDate = DateTime.Today;
            BlogAuthorBox.Text = "Mike Dopp";
            GitChangesList.Items.Add("Click Refresh Status to inspect the working tree.");

            LoadPages();
            LoadProjects();
            RefreshBlogPreview();
            AppendLog("Editor ready.");
            _ = RefreshGitStatusAsync(showPublishTab: false);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Site editor startup failed", MessageBoxButton.OK, MessageBoxImage.Error);
            Close();
        }
    }

    private static string FindRepositoryRoot()
    {
        foreach (string start in new[] { Environment.CurrentDirectory, AppContext.BaseDirectory }.Distinct())
        {
            DirectoryInfo? dir = new DirectoryInfo(start);
            while (dir is not null)
            {
                bool hasGit = Directory.Exists(Path.Combine(dir.FullName, ".git"));
                bool hasConfig = File.Exists(Path.Combine(dir.FullName, "_config.yml"));
                if (hasGit && hasConfig)
                {
                    return dir.FullName;
                }

                dir = dir.Parent;
            }
        }

        throw new InvalidOperationException("Could not find the Jekyll repository root. Launch the editor from Start-SiteEditor.ps1 in the repo root.");
    }

    private static string GetVersionText()
    {
        Assembly assembly = typeof(MainWindow).Assembly;
        string version = assembly
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
            .InformationalVersion
            ?? assembly.GetName().Version?.ToString(3)
            ?? "local";

        string revision = "";
        int metadataStart = version.IndexOf('+', StringComparison.Ordinal);
        if (metadataStart >= 0)
        {
            string metadata = version[(metadataStart + 1)..];
            version = version[..metadataStart];
            if (!string.IsNullOrWhiteSpace(metadata))
            {
                revision = $" ({metadata[..Math.Min(7, metadata.Length)]})";
            }
        }

        string build = File.GetLastWriteTime(assembly.Location)
            .ToString("yyyy.MM.dd HH:mm", CultureInfo.InvariantCulture);

        return $"v{version}{revision} | build {build}";
    }

    private void NewBlog_Click(object sender, RoutedEventArgs e)
    {
        BlogTitleBox.Text = "";
        BlogSubtitleBox.Text = "";
        BlogTagsBox.Text = "";
        BlogDatePicker.SelectedDate = DateTime.Today;
        BlogAuthorBox.Text = "Mike Dopp";
        BlogVideoUrlBox.Text = "";
        BlogBodyBox.Text = "";
        RefreshBlogPreview();
        AppendLog("Started a new blog draft.");
    }

    private void SavePost_Click(object sender, RoutedEventArgs e)
    {
        string title = BlogTitleBox.Text.Trim();
        if (string.IsNullOrWhiteSpace(title))
        {
            MessageBox.Show("Give the post a title first.", "Missing title", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        DateTime date = BlogDatePicker.SelectedDate ?? DateTime.Today;
        string slug = EditorContent.Slugify(title);
        string postsDir = Path.Combine(_repoRoot, "_posts");
        Directory.CreateDirectory(postsDir);

        string path = Path.Combine(postsDir, $"{date:yyyy-MM-dd}-{slug}.md");
        if (File.Exists(path))
        {
            MessageBoxResult result = MessageBox.Show(
                $"A post named {Path.GetFileName(path)} already exists. Overwrite it?",
                "Overwrite post?",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
            {
                return;
            }
        }

        string markdown = EditorContent.BuildPostMarkdown(
            title,
            BlogSubtitleBox.Text.Trim(),
            date,
            EditorContent.SplitTags(BlogTagsBox.Text),
            BlogAuthorBox.Text.Trim(),
            BlogBodyBox.Text.TrimEnd());

        File.WriteAllText(path, markdown, Utf8NoBom);
        CommitMessageBox.Text = $"Add {title}";
        AppendLog($"Saved post: {RelativePath(path)}");
    }

    private void LoadPages()
    {
        PagePicker.Items.Clear();

        string[] excluded =
        [
            "README.md",
            "CHANGELOG.md",
            "LICENSE",
            "Gemfile",
            "feed.xml"
        ];

        IEnumerable<string> files = Directory
            .EnumerateFiles(_repoRoot, "*.*", SearchOption.TopDirectoryOnly)
            .Where(file =>
            {
                string name = Path.GetFileName(file);
                string ext = Path.GetExtension(file).ToLowerInvariant();
                return (ext is ".md" or ".html") && !excluded.Contains(name, StringComparer.OrdinalIgnoreCase);
            })
            .Where(file => File.ReadLines(file).FirstOrDefault()?.Trim() == "---")
            .OrderBy(file => Path.GetFileName(file), StringComparer.OrdinalIgnoreCase);

        foreach (string file in files)
        {
            PagePicker.Items.Add(new ComboBoxItem { Content = Path.GetFileName(file), Tag = file });
        }

        if (PagePicker.Items.Count > 0)
        {
            PagePicker.SelectedIndex = 0;
        }
    }

    private void PagePicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (PagePicker.SelectedItem is not ComboBoxItem item || item.Tag is not string path)
        {
            return;
        }

        _loadingPage = true;
        try
        {
            _currentPage = PageDocument.Load(path);
            PageTitleBox.Text = _currentPage.FrontMatter.GetValueOrDefault("title", "");
            PageSubtitleBox.Text = _currentPage.FrontMatter.GetValueOrDefault("subtitle", "");
            PageBodyBox.Text = _currentPage.Body.TrimStart('\r', '\n');
            PageVideoUrlBox.Text = "";
        }
        finally
        {
            _loadingPage = false;
        }

        RefreshPagePreview();
        AppendLog($"Loaded page: {RelativePath(path)}");
    }

    private void SavePage_Click(object sender, RoutedEventArgs e)
    {
        if (_currentPage is null)
        {
            MessageBox.Show("Choose a page first.", "No page selected", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        _currentPage.FrontMatter["title"] = PageTitleBox.Text.Trim();
        _currentPage.FrontMatter["subtitle"] = PageSubtitleBox.Text.Trim();
        _currentPage.Body = PageBodyBox.Text.TrimEnd();
        _currentPage.Save();

        CommitMessageBox.Text = $"Update {Path.GetFileName(_currentPage.FilePath)}";
        AppendLog($"Saved page: {RelativePath(_currentPage.FilePath)}");
    }

    private void ReloadPages_Click(object sender, RoutedEventArgs e)
    {
        LoadPages();
        AppendLog("Reloaded page list.");
    }

    private void InsertBlogImage_Click(object sender, RoutedEventArgs e) => InsertImage(BlogBodyBox);

    private void InsertPageImage_Click(object sender, RoutedEventArgs e) => InsertImage(PageBodyBox);

    private void InsertBlogVideoFile_Click(object sender, RoutedEventArgs e) => InsertVideoFile(BlogBodyBox);

    private void InsertPageVideoFile_Click(object sender, RoutedEventArgs e) => InsertVideoFile(PageBodyBox);

    private void InsertBlogEmbed_Click(object sender, RoutedEventArgs e) => InsertEmbed(BlogBodyBox, BlogVideoUrlBox);

    private void InsertPageEmbed_Click(object sender, RoutedEventArgs e) => InsertEmbed(PageBodyBox, PageVideoUrlBox);

    private void InsertImage(TextBox editor)
    {
        string? relativePath = CopyMediaFile("Images|*.png;*.jpg;*.jpeg;*.gif;*.webp;*.svg|All files|*.*");
        if (relativePath is null)
        {
            return;
        }

        string alt = Path.GetFileNameWithoutExtension(relativePath).Replace('-', ' ');
        InsertAtCaret(editor, $"{Environment.NewLine}![{alt}]({relativePath}){Environment.NewLine}");
        AppendLog($"Copied image: {relativePath}");
    }

    private void InsertVideoFile(TextBox editor)
    {
        string? relativePath = CopyMediaFile("Videos|*.mp4;*.webm;*.mov;*.m4v|All files|*.*");
        if (relativePath is null)
        {
            return;
        }

        string snippet = $"""

<video class="site-video" controls preload="metadata" src="{relativePath}"></video>

""";
        InsertAtCaret(editor, snippet);
        AppendLog($"Copied video: {relativePath}");
    }

    private string? CopyMediaFile(string filter)
    {
        var dialog = new OpenFileDialog
        {
            CheckFileExists = true,
            Filter = filter,
            Title = "Choose media to copy into the site"
        };

        if (dialog.ShowDialog(this) != true)
        {
            return null;
        }

        string uploadFolder = DateTime.Today.ToString("yyyy", CultureInfo.InvariantCulture);
        string targetDir = Path.Combine(_repoRoot, "assets", "uploads", uploadFolder);
        Directory.CreateDirectory(targetDir);

        string fileName = EditorContent.SafeFileName(Path.GetFileName(dialog.FileName));
        string destination = UniquePath(Path.Combine(targetDir, fileName));
        File.Copy(dialog.FileName, destination);

        return $"/assets/uploads/{uploadFolder}/{Path.GetFileName(destination)}";
    }

    private void InsertEmbed(TextBox editor, TextBox sourceBox)
    {
        string url = sourceBox.Text.Trim();
        if (string.IsNullOrWhiteSpace(url))
        {
            MessageBox.Show("Paste a YouTube or Vimeo URL first.", "Missing video URL", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        string snippet = EditorContent.BuildEmbedSnippet(url);
        InsertAtCaret(editor, $"{Environment.NewLine}{snippet}{Environment.NewLine}");
        sourceBox.Text = "";
        AppendLog("Inserted video embed.");
    }

    private static void InsertAtCaret(TextBox editor, string text)
    {
        int start = editor.SelectionStart;
        editor.SelectedText = text;
        editor.SelectionStart = start + text.Length;
        editor.SelectionLength = 0;
        editor.Focus();
    }

    private void LoadProjects()
    {
        _projects.Clear();

        string path = ProjectsPath;
        if (!File.Exists(path))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            return;
        }

        ProjectLink? current = null;
        foreach (string rawLine in File.ReadLines(path))
        {
            string line = rawLine.Trim();
            if (line.StartsWith("- ", StringComparison.Ordinal))
            {
                current = new ProjectLink();
                _projects.Add(current);
                SetProjectField(current, line[2..]);
            }
            else if (current is not null && line.Contains(':'))
            {
                SetProjectField(current, line);
            }
        }

        AppendLog("Loaded latest projects.");
    }

    private static void SetProjectField(ProjectLink project, string line)
    {
        int colon = line.IndexOf(':');
        if (colon < 0)
        {
            return;
        }

        string key = line[..colon].Trim();
        string value = EditorContent.UnquoteYamlValue(line[(colon + 1)..].Trim());
        switch (key)
        {
            case "title":
                project.Title = value;
                break;
            case "url":
                project.Url = value;
                break;
            case "description":
                project.Description = value;
                break;
        }
    }

    private void SaveProjects_Click(object sender, RoutedEventArgs e)
    {
        ProjectsGrid.CommitEdit(DataGridEditingUnit.Cell, true);
        ProjectsGrid.CommitEdit(DataGridEditingUnit.Row, true);
        SaveProjects();
    }

    private void SaveProjects()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(ProjectsPath)!);

        var builder = new StringBuilder();
        foreach (ProjectLink project in _projects.Where(p => !p.IsBlank))
        {
            builder.AppendLine($"- title: {EditorContent.YamlValue(project.Title.Trim())}");
            builder.AppendLine($"  url: {EditorContent.YamlValue(project.Url.Trim())}");
            builder.AppendLine($"  description: {EditorContent.YamlValue(project.Description.Trim())}");
        }

        File.WriteAllText(ProjectsPath, builder.ToString(), Utf8NoBom);
        CommitMessageBox.Text = "Update project links";
        AppendLog($"Saved projects: {RelativePath(ProjectsPath)}");
    }

    private void AddProject_Click(object sender, RoutedEventArgs e)
    {
        _projects.Add(new ProjectLink { Title = "New project", Url = "https://", Description = "" });
    }

    private void RemoveProject_Click(object sender, RoutedEventArgs e)
    {
        if (ProjectsGrid.SelectedItem is ProjectLink project)
        {
            _projects.Remove(project);
        }
    }

    private void ReloadProjects_Click(object sender, RoutedEventArgs e) => LoadProjects();

    private void BlogPreviewInputChanged(object sender, TextChangedEventArgs e)
    {
        if (!IsLoaded)
        {
            return;
        }

        _blogPreviewTimer.Stop();
        _blogPreviewTimer.Start();
    }

    private void PagePreviewInputChanged(object sender, TextChangedEventArgs e)
    {
        if (!IsLoaded || _loadingPage)
        {
            return;
        }

        _pagePreviewTimer.Stop();
        _pagePreviewTimer.Start();
    }

    private void RefreshBlogPreview_Click(object sender, RoutedEventArgs e) => RefreshBlogPreview();

    private void RefreshPagePreview_Click(object sender, RoutedEventArgs e) => RefreshPagePreview();

    private void RefreshBlogPreview()
    {
        string html = BuildPreviewHtml(BlogTitleBox.Text, BlogSubtitleBox.Text, BlogBodyBox.Text);
        NavigatePreview(BlogPreviewBrowser, html);
    }

    private void RefreshPagePreview()
    {
        string html = BuildPreviewHtml(PageTitleBox.Text, PageSubtitleBox.Text, PageBodyBox.Text);
        NavigatePreview(PagePreviewBrowser, html);
    }

    private string BuildPreviewHtml(string title, string subtitle, string markdown)
    {
        string body = Markdig.Markdown.ToHtml(markdown ?? "", _markdownPipeline);
        body = RewriteAssetUrls(body);

        return $$"""
<!doctype html>
<html>
<head>
  <meta charset="utf-8">
  <style>
    body { background: #111827; color: #e5e7eb; font-family: "Segoe UI", sans-serif; line-height: 1.55; margin: 0; padding: 28px; }
    main { max-width: 760px; margin: 0 auto; }
    h1, h2, h3 { color: #f8fafc; }
    h1 { margin-top: 0; }
    .subtitle { color: #9ca3af; font-size: 1.2rem; margin-top: -0.5rem; }
    a { color: #67b7ff; }
    img, video { border-radius: 8px; height: auto; max-width: 100%; }
    pre, code { background: #020617; border-radius: 6px; }
    pre { padding: 1rem; overflow: auto; }
    blockquote { border-left: 4px solid #334155; color: #cbd5e1; margin-left: 0; padding-left: 1rem; }
    .video-embed { aspect-ratio: 16 / 9; margin: 1.5rem 0; }
    .video-embed iframe { border: 0; border-radius: 8px; height: 100%; width: 100%; }
    .site-video { width: 100%; }
  </style>
</head>
<body>
  <main>
    <h1>{{WebUtility.HtmlEncode(title)}}</h1>
    {{(string.IsNullOrWhiteSpace(subtitle) ? "" : $"<p class=\"subtitle\">{WebUtility.HtmlEncode(subtitle)}</p>")}}
    {{body}}
  </main>
</body>
</html>
""";
    }

    private string RewriteAssetUrls(string html)
    {
        return Regex.Replace(
            html,
            "(?<attr>\\s(?:src|href)=\")(?<url>/assets/[^\"]+)(?<end>\")",
            match =>
            {
                string url = WebUtility.HtmlDecode(match.Groups["url"].Value);
                string localPath = Path.Combine(_repoRoot, url.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                if (!File.Exists(localPath))
                {
                    return match.Value;
                }

                return $"{match.Groups["attr"].Value}{new Uri(localPath).AbsoluteUri}{match.Groups["end"].Value}";
            },
            RegexOptions.IgnoreCase);
    }

    private static void NavigatePreview(WebBrowser browser, string html)
    {
        try
        {
            browser.NavigateToString(html);
        }
        catch
        {
            // WebBrowser can throw while it is still initializing; the next edit refreshes it.
        }
    }

    private async void GitStatus_Click(object sender, RoutedEventArgs e)
    {
        await RefreshGitStatusAsync(showPublishTab: true);
    }

    private async void CommitPush_Click(object sender, RoutedEventArgs e)
    {
        MainTabs.SelectedItem = PublishTab;
        SetGitBusy(true, "Checking working tree...");
        try
        {
            AppendLog("Checking working tree...");
            string status = await RunGitCommandAsync("status", "--short");
            if (string.IsNullOrWhiteSpace(status))
            {
                AppendLog("No changes to publish.");
                LastActionText.Text = "No changes to publish.";
                await RefreshGitStatusAsync(showPublishTab: false);
                return;
            }

            string message = CommitMessageBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(message))
            {
                message = "Update site content";
            }

            AppendLog(await RunGitForLogAsync("add", "-A"));
            AppendLog(await RunGitForLogAsync("commit", "-m", message));
            AppendLog(await RunGitForLogAsync("push", "origin", "master"));
            AppendLog("Published changes to GitHub Pages.");
            LastActionText.Text = "Published changes to GitHub Pages.";
            await RefreshGitStatusAsync(showPublishTab: false);
        }
        catch (Exception ex)
        {
            AppendLog(ex.Message);
            LastActionText.Text = "Publish failed. See activity log.";
            MessageBox.Show(ex.Message, "Publish failed", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            SetGitBusy(false);
        }
    }

    private async Task RefreshGitStatusAsync(bool showPublishTab)
    {
        if (showPublishTab)
        {
            MainTabs.SelectedItem = PublishTab;
        }

        SetGitBusy(true, "Checking git status...");
        try
        {
            string status = await RunGitCommandAsync("status", "--short", "--branch");
            UpdateGitStatus(status);
            AppendLog(FormatGitStatusForLog(status));
            LastActionText.Text = "Git status refreshed.";
        }
        catch (Exception ex)
        {
            GitSummaryText.Text = "Could not read git status";
            GitSummaryText.Foreground = (System.Windows.Media.Brush)FindResource("WarningBrush");
            GitChangesList.Items.Clear();
            GitChangesList.Items.Add(ex.Message);
            AppendLog(ex.Message);
            LastActionText.Text = "Git status failed. See activity log.";
            if (showPublishTab)
            {
                MessageBox.Show(ex.Message, "Git status failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        finally
        {
            SetGitBusy(false);
        }
    }

    private void UpdateGitStatus(string status)
    {
        string[] lines = status
            .Split(["\r\n", "\n"], StringSplitOptions.RemoveEmptyEntries)
            .Select(line => line.TrimEnd())
            .ToArray();

        string branch = lines.FirstOrDefault(line => line.StartsWith("## ", StringComparison.Ordinal)) ?? "## branch unknown";
        string[] changes = lines
            .Where(line => !line.StartsWith("## ", StringComparison.Ordinal))
            .ToArray();

        GitBranchText.Text = $"Branch: {branch[3..]}";
        GitSummaryText.Text = changes.Length == 0
            ? "Working tree clean"
            : $"{changes.Length} pending change{(changes.Length == 1 ? "" : "s")}";
        GitSummaryText.Foreground = (System.Windows.Media.Brush)FindResource(
            changes.Length == 0 ? "SuccessBrush" : "WarningBrush");

        GitChangesList.Items.Clear();
        if (changes.Length == 0)
        {
            GitChangesList.Items.Add("No pending changes.");
            return;
        }

        foreach (string change in changes)
        {
            GitChangesList.Items.Add(change);
        }
    }

    private static string FormatGitStatusForLog(string status)
    {
        string[] lines = status
            .Split(["\r\n", "\n"], StringSplitOptions.RemoveEmptyEntries)
            .Select(line => line.TrimEnd())
            .ToArray();
        string branch = lines.FirstOrDefault(line => line.StartsWith("## ", StringComparison.Ordinal)) ?? "";
        int changeCount = lines.Count(line => !line.StartsWith("## ", StringComparison.Ordinal));

        if (changeCount == 0)
        {
            return string.IsNullOrWhiteSpace(branch)
                ? "Working tree clean."
                : $"Working tree clean. {branch[3..]}";
        }

        return status;
    }

    private void SetGitBusy(bool busy, string? message = null)
    {
        HeaderGitStatusButton.IsEnabled = !busy;
        HeaderCommitPushButton.IsEnabled = !busy;
        PublishGitStatusButton.IsEnabled = !busy;
        PublishCommitPushButton.IsEnabled = !busy;

        if (!string.IsNullOrWhiteSpace(message))
        {
            LastActionText.Text = message;
        }
    }

    private async Task<string> RunGitForLogAsync(params string[] args)
    {
        string result = await RunGitCommandAsync(args);
        return string.IsNullOrWhiteSpace(result) ? $"git {string.Join(' ', args)} completed." : result;
    }

    private async Task<string> RunGitCommandAsync(params string[] args)
    {
        var info = new ProcessStartInfo("git")
        {
            WorkingDirectory = _repoRoot,
            RedirectStandardError = true,
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        foreach (string arg in args)
        {
            info.ArgumentList.Add(arg);
        }

        using Process process = Process.Start(info) ?? throw new InvalidOperationException("Could not start git.");
        string output = await process.StandardOutput.ReadToEndAsync();
        string error = await process.StandardError.ReadToEndAsync();
        await process.WaitForExitAsync();

        string combined = (output + error).Trim();
        if (process.ExitCode != 0)
        {
            throw new InvalidOperationException(string.IsNullOrWhiteSpace(combined) ? $"git {string.Join(' ', args)} failed." : combined);
        }

        return combined;
    }

    private string ProjectsPath => Path.Combine(_repoRoot, "_data", "projects.yml");

    private string RelativePath(string path)
    {
        return Path.GetRelativePath(_repoRoot, path).Replace('\\', '/');
    }

    private void AppendLog(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            return;
        }

        StatusLogBox.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}");
        StatusLogBox.ScrollToEnd();
    }

    private static string UniquePath(string path)
    {
        if (!File.Exists(path))
        {
            return path;
        }

        string dir = Path.GetDirectoryName(path)!;
        string name = Path.GetFileNameWithoutExtension(path);
        string ext = Path.GetExtension(path);
        int index = 2;
        string candidate;
        do
        {
            candidate = Path.Combine(dir, $"{name}-{index}{ext}");
            index++;
        }
        while (File.Exists(candidate));

        return candidate;
    }

}

public sealed class PageDocument
{
    private PageDocument(string filePath, Dictionary<string, string> frontMatter, List<string> keyOrder, string body)
    {
        FilePath = filePath;
        FrontMatter = frontMatter;
        KeyOrder = keyOrder;
        Body = body;
    }

    public string FilePath { get; }

    public Dictionary<string, string> FrontMatter { get; }

    public List<string> KeyOrder { get; }

    public string Body { get; set; }

    public static PageDocument Load(string filePath)
    {
        string text = File.ReadAllText(filePath);
        var frontMatter = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        var keyOrder = new List<string>();

        using var reader = new StringReader(text);
        string? first = reader.ReadLine();
        if (first?.Trim() != "---")
        {
            return new PageDocument(filePath, frontMatter, keyOrder, text);
        }

        string? line;
        while ((line = reader.ReadLine()) is not null)
        {
            if (line.Trim() == "---")
            {
                break;
            }

            int colon = line.IndexOf(':');
            if (colon <= 0)
            {
                continue;
            }

            string key = line[..colon].Trim();
            string value = line[(colon + 1)..].Trim();
            frontMatter[key] = EditorContent.UnquoteYamlValue(value);
            keyOrder.Add(key);
        }

        string body = reader.ReadToEnd();
        return new PageDocument(filePath, frontMatter, keyOrder, body);
    }

    public void Save()
    {
        var keys = new List<string>(KeyOrder);
        foreach (string key in new[] { "layout", "title", "subtitle" })
        {
            if (FrontMatter.ContainsKey(key) && !keys.Contains(key, StringComparer.OrdinalIgnoreCase))
            {
                keys.Add(key);
            }
        }

        var builder = new StringBuilder();
        builder.AppendLine("---");
        foreach (string key in keys.Distinct(StringComparer.OrdinalIgnoreCase))
        {
            if (FrontMatter.TryGetValue(key, out string? value))
            {
                builder.AppendLine($"{key}: {EditorContent.YamlValue(value)}");
            }
        }

        builder.AppendLine("---");
        builder.AppendLine();
        builder.AppendLine(Body.TrimEnd());
        File.WriteAllText(FilePath, builder.ToString(), new UTF8Encoding(false));
    }

}

public sealed class ProjectLink : INotifyPropertyChanged
{
    private string _title = "";
    private string _url = "";
    private string _description = "";

    public event PropertyChangedEventHandler? PropertyChanged;

    public string Title
    {
        get => _title;
        set
        {
            if (_title == value)
            {
                return;
            }

            _title = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Title)));
        }
    }

    public string Url
    {
        get => _url;
        set
        {
            if (_url == value)
            {
                return;
            }

            _url = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Url)));
        }
    }

    public string Description
    {
        get => _description;
        set
        {
            if (_description == value)
            {
                return;
            }

            _description = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Description)));
        }
    }

    public bool IsBlank => string.IsNullOrWhiteSpace(Title)
        && string.IsNullOrWhiteSpace(Url)
        && string.IsNullOrWhiteSpace(Description);
}
