using MikeSiteEditor;

var tests = new (string Name, Action Body)[]
{
    ("post markdown front matter", TestPostMarkdownFrontMatter),
    ("video embed snippets", TestVideoEmbedSnippets),
    ("slug and media file names", TestSlugAndMediaFileNames),
    ("tag splitting", TestTagSplitting),
    ("page document load and save", TestPageDocumentLoadAndSave)
};

int failed = 0;
foreach ((string name, Action body) in tests)
{
    try
    {
        body();
        Console.WriteLine($"PASS {name}");
    }
    catch (Exception ex)
    {
        failed++;
        Console.Error.WriteLine($"FAIL {name}");
        Console.Error.WriteLine(ex.Message);
    }
}

if (failed > 0)
{
    Environment.Exit(1);
}

Console.WriteLine($"{tests.Length} tests passed.");

static void TestPostMarkdownFrontMatter()
{
    string markdown = EditorContent.BuildPostMarkdown(
        "A Post: with \"quotes\"",
        "Sub title",
        new DateTime(2026, 5, 31),
        EditorContent.SplitTags("AI, github, AI, C#"),
        "Mike Dopp",
        "Hello\r\n\r\nWorld\r\n");

    const string expected = """
---
layout: post
title: "A Post: with \"quotes\""
subtitle: "Sub title"
date: 2026-05-31
tags: [AI, github, "C#"]
author: "Mike Dopp"
---

Hello

World

""";

    Equal(Normalize(expected), Normalize(markdown));
}

static void TestVideoEmbedSnippets()
{
    string youtube = EditorContent.BuildEmbedSnippet("https://www.youtube.com/watch?v=dQw4w9WgXcQ&feature=share");
    Contains("https://www.youtube.com/embed/dQw4w9WgXcQ", youtube);
    Contains("title=\"YouTube video\"", youtube);

    string shorts = EditorContent.BuildEmbedSnippet("https://youtube.com/shorts/abc123");
    Contains("https://www.youtube.com/embed/abc123", shorts);

    string vimeo = EditorContent.BuildEmbedSnippet("https://vimeo.com/123456789");
    Contains("https://player.vimeo.com/video/123456789", vimeo);

    string fallback = EditorContent.BuildEmbedSnippet("https://example.com/watch?x=1&y=2");
    Equal("[https://example.com/watch?x=1&amp;y=2](https://example.com/watch?x=1&y=2)", fallback);
}

static void TestSlugAndMediaFileNames()
{
    Equal("hello-from-mike-2026", EditorContent.Slugify("Hello from Mike, 2026!"));
    Equal("post", EditorContent.Slugify("!!!"));
    Equal("my-photo.jpg", EditorContent.SafeFileName("My Photo.JPG"));
    Equal("media.mp4", EditorContent.SafeFileName("!!!.MP4"));
}

static void TestTagSplitting()
{
    string[] tags = EditorContent.SplitTags(" AI, github, ai, C# ,, ").ToArray();
    SequenceEqual(new[] { "AI", "github", "C#" }, tags);
}

static void TestPageDocumentLoadAndSave()
{
    string dir = Path.Combine(Path.GetTempPath(), "MikeSiteEditor.Tests", Guid.NewGuid().ToString("N"));
    Directory.CreateDirectory(dir);

    try
    {
        string path = Path.Combine(dir, "about.md");
        File.WriteAllText(path, Normalize("""
---
layout: page
title: "Old Title"
custom: keep
---

Original body

"""));

        PageDocument page = PageDocument.Load(path);
        Equal("Old Title", page.FrontMatter["title"]);
        Equal("keep", page.FrontMatter["custom"]);

        page.FrontMatter["title"] = "Fresh Title";
        page.FrontMatter["subtitle"] = "Overview";
        page.Body = "Updated body";
        page.Save();

        const string expected = """
---
layout: page
title: "Fresh Title"
custom: keep
subtitle: Overview
---

Updated body

""";

        Equal(Normalize(expected), Normalize(File.ReadAllText(path)));
    }
    finally
    {
        Directory.Delete(dir, recursive: true);
    }
}

static string Normalize(string value) => value.Replace("\r\n", "\n");

static void Equal<T>(T expected, T actual)
{
    if (!EqualityComparer<T>.Default.Equals(expected, actual))
    {
        throw new InvalidOperationException($"Expected: {expected}{Environment.NewLine}Actual:   {actual}");
    }
}

static void Contains(string expected, string actual)
{
    if (!actual.Contains(expected, StringComparison.Ordinal))
    {
        throw new InvalidOperationException($"Expected to find '{expected}' in:{Environment.NewLine}{actual}");
    }
}

static void SequenceEqual<T>(IReadOnlyList<T> expected, IReadOnlyList<T> actual)
{
    if (expected.Count != actual.Count || expected.Where((item, index) => !EqualityComparer<T>.Default.Equals(item, actual[index])).Any())
    {
        throw new InvalidOperationException(
            $"Expected: {string.Join(", ", expected)}{Environment.NewLine}Actual:   {string.Join(", ", actual)}");
    }
}
