using System.IO;
using Mammoth;

namespace extract;

internal static class DocxExtract
{
    internal static string Extract(Stream content)
    {
        var converter = new DocumentConverter();
        var html = converter.ConvertToHtml(content);

        return HtmlToMd.Convert(html.Value);
    }
}