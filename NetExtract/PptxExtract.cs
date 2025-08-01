using System.IO;
using System.Linq;
using System.Text;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Presentation;

namespace extract;

internal static class PptxExtract
{
    internal static string Extract(Stream content)
    {
        using var presentation = PresentationDocument.Open(content, false);

        var slides = presentation.PresentationPart?.SlideParts.Count() ?? 0;

        return string.Join("\n---\n", Enumerable.Range(0, slides).Select(i => GetSlideText(presentation, i)));
    }

    private static string GetSlideText(PresentationDocument ppt, int index)
    {
        var part = ppt.PresentationPart;
        var slideIds = part?.Presentation?.SlideIdList?.ChildElements ?? default;

        if (part is null || slideIds.Count == 0)
        {
            return "";
        }

        string? relId = ((SlideId)slideIds[index]).RelationshipId;

        if (relId is null)
        {
            return "";
        }

        // Get the slide part from the relationship ID.
        var slide = (SlidePart)part.GetPartById(relId);

        var builder = new StringBuilder();

        // Iterate through all the paragraphs in the slide.
        foreach (var paragraph in slide.Slide.Descendants<DocumentFormat.OpenXml.Drawing.Paragraph>())
        {
            // Create a new string builder.                    
            var paragraphText = new StringBuilder();

            // Iterate through the lines of the paragraph.
            foreach (var text in paragraph.Descendants<DocumentFormat.OpenXml.Drawing.Text>())
            {
                // Append each line to the previous lines.
                paragraphText.Append(text.Text);
            }

            if (paragraphText.Length > 0)
            {
                // Add each paragraph to the linked list.
                builder.AppendLine(paragraphText.ToString());
            }
        }

        return builder.ToString();
    }
}