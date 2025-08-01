using System;
using System.IO;
using System.Text;
using XlsxToHtmlConverter;
using Array = System.Array;

namespace extract;

internal static class XlsxExtract
{
    internal static string Extract(Stream content)
    {
        var ms = new MemoryStream(1024);
        var config = ConverterConfig.DefaultSettings;
        config.Encoding = Encoding.UTF8;
        Converter.ConvertXlsx(content, ms, config);

        var buffer = ms.GetBuffer();
        var len = Array.IndexOf<byte>(buffer, 0);
        if (len == -1) len = buffer.Length;

        var html = Encoding.UTF8.GetString(buffer, 0, len);

        return Html2Md.Convert(html);
    }
}