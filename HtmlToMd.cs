using ReverseMarkdown;

namespace extract;

internal static class HtmlToMd
{
    internal static string Convert(string html)
    {
        var converter = new Converter(new Config
        {
            UnknownTags = Config.UnknownTagsOption.PassThrough,
            GithubFlavored = true,
            RemoveComments = true,
            SmartHrefHandling = true,
            CleanupUnnecessarySpaces = true
        });

        return converter.Convert(html);
    }
}