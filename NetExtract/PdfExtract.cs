using System.IO;
using System.Linq;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.DocumentLayoutAnalysis.TextExtractor;

namespace extract;

internal static class PdfExtract
{
    internal static string Extract(Stream content)
    {
        using var extractor = PdfDocument.Open(content);

        var text = string.Join("\n\n",
            extractor.GetPages().Select(GetPageText));

        return text;
    }

    private static string GetPageText(Page page)
    {
        return ContentOrderTextExtractor.GetText(page, true);
    }
}