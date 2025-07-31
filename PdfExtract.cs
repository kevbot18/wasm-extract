using System.IO;
using System.Linq;
using UglyToad.PdfPig;
using UglyToad.PdfPig.DocumentLayoutAnalysis.TextExtractor;

namespace extract;

static class PdfExtract
{
    internal static string Extract(Stream content)
    {
        using var extractor = PdfDocument.Open(content);

        var text = string.Join("\n\n",
            extractor.GetPages().Select(page => ContentOrderTextExtractor.GetText(page)));

        return text;
    }
}