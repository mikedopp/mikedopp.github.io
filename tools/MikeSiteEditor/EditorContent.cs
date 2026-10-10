using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace MikeSiteEditor;

internal static class EditorContent
{
    public static string BuildPostMarkdown(
        string title,
        string subtitle,
        DateTime date,
        IEnumerable<string> tags,
        string author,
        string body)
    {
        var builder = new StringBuilder();
        builder.AppendLine("---");
        builder.AppendLine("layout: post");
        builder.AppendLine($"title: {YamlValue(title)}");
        if (!string.IsNullOrWhiteSpace(subtitle))
        {
            builder.AppendLine($"subtitle: {YamlValue(subtitle)}");
        }

        builder.AppendLine($"date: {date:yyyy-MM-dd}");

        List<string> tagList = tags
            .Where(tag => !string.IsNullOrWhiteSpace(tag))
            .Select(tag => tag.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
        if (tagList.Count > 0)
        {
            builder.AppendLine($"tags: [{string.Join(", ", tagList.Select(YamlValue))}]");
        }

        if (!string.IsNullOrWhiteSpace(author))
        {
            builder.AppendLine($"author: {YamlValue(author)}");
        }

        builder.AppendLine("---");
        builder.AppendLine();
        builder.AppendLine(body.TrimEnd());
        return builder.ToString();
    }

    public static string BuildEmbedSnippet(string url)
    {
        if (TryGetYouTubeId(url, out string? youtubeId))
        {
            return $"""
<div class="video-embed">
  <iframe src="https://www.youtube.com/embed/{WebUtility.HtmlEncode(youtubeId)}" title="YouTube video" allowfullscreen></iframe>
</div>
""";
        }

        if (TryGetVimeoId(url, out string? vimeoId))
        {
            return $"""
<div class="video-embed">
  <iframe src="https://player.vimeo.com/video/{WebUtility.HtmlEncode(vimeoId)}" title="Vimeo video" allowfullscreen></iframe>
</div>
""";
        }

        return $"[{WebUtility.HtmlEncode(url)}]({url})";
    }

    public static bool TryGetYouTubeId(string url, out string? id)
    {
        id = null;
        if (!Uri.TryCreate(url, UriKind.Absolute, out Uri? uri))
        {
            return false;
        }

        string host = uri.Host.ToLowerInvariant();
        if (host.EndsWith("youtu.be", StringComparison.OrdinalIgnoreCase))
        {
            id = uri.AbsolutePath.Trim('/').Split('/').FirstOrDefault();
            return !string.IsNullOrWhiteSpace(id);
        }

        if (!host.Contains("youtube", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        Dictionary<string, string> query = ParseQuery(uri.Query);
        if (query.TryGetValue("v", out string? value) && !string.IsNullOrWhiteSpace(value))
        {
            id = value;
            return true;
        }

        string[] segments = uri.AbsolutePath.Trim('/').Split('/', StringSplitOptions.RemoveEmptyEntries);
        if (segments.Length >= 2 && segments[0] is "embed" or "shorts")
        {
            id = segments[1];
            return true;
        }

        return false;
    }

    public static bool TryGetVimeoId(string url, out string? id)
    {
        id = null;
        if (!Uri.TryCreate(url, UriKind.Absolute, out Uri? uri))
        {
            return false;
        }

        if (!uri.Host.Contains("vimeo", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        id = uri.AbsolutePath.Trim('/').Split('/').LastOrDefault();
        return !string.IsNullOrWhiteSpace(id) && id.All(char.IsDigit);
    }

    public static List<string> SplitTags(string tags)
    {
        return tags
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Where(tag => tag.Length > 0)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    public static string Slugify(string value)
    {
        string normalized = value.ToLowerInvariant();
        normalized = Regex.Replace(normalized, @"[^a-z0-9]+", "-").Trim('-');
        return string.IsNullOrWhiteSpace(normalized) ? "post" : normalized;
    }

    public static string SafeFileName(string fileName)
    {
        string name = Path.GetFileNameWithoutExtension(fileName);
        string ext = Path.GetExtension(fileName).ToLowerInvariant();
        string safeName = Regex.Replace(name.ToLowerInvariant(), @"[^a-z0-9]+", "-").Trim('-');
        return $"{(string.IsNullOrWhiteSpace(safeName) ? "media" : safeName)}{ext}";
    }

    public static string YamlValue(string value)
    {
        value = value.Trim();
        if (string.IsNullOrEmpty(value))
        {
            return "\"\"";
        }

        if (value is "true" or "false" || Regex.IsMatch(value, @"^[A-Za-z0-9_./@-]+$"))
        {
            return value;
        }

        return $"\"{value.Replace("\\", "\\\\").Replace("\"", "\\\"")}\"";
    }

    public static string UnquoteYamlValue(string value)
    {
        if (value.Length >= 2 && value[0] == '"' && value[^1] == '"')
        {
            return value[1..^1].Replace("\\\"", "\"").Replace("\\\\", "\\");
        }

        if (value.Length >= 2 && value[0] == '\'' && value[^1] == '\'')
        {
            return value[1..^1].Replace("''", "'");
        }

        return value;
    }

    private static Dictionary<string, string> ParseQuery(string query)
    {
        return query
            .TrimStart('?')
            .Split('&', StringSplitOptions.RemoveEmptyEntries)
            .Select(part => part.Split('=', 2))
            .Where(parts => parts.Length == 2)
            .ToDictionary(
                parts => WebUtility.UrlDecode(parts[0]),
                parts => WebUtility.UrlDecode(parts[1]),
                StringComparer.OrdinalIgnoreCase);
    }
}
