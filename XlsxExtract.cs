using System.IO;
using NPOI.SS.Converter;
using NPOI.XSSF.UserModel;

namespace extract;

internal static class XlsxExtract
{
    internal static string Extract(Stream content)
    {
        using var doc = new XSSFWorkbook(content, true);
        
        var converter = new ExcelToHtmlConverter
        {
            OutputColumnHeaders = false,
            OutputRowNumbers = false
        };
        converter.ProcessWorkbook(doc);

        return HtmlToMd.Convert(converter.Document.OuterXml);
    }
}