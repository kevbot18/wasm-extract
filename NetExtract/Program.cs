using System;
using System.IO;
using System.Runtime.InteropServices.JavaScript;
using extract;

Console.WriteLine("Started");

partial class ExtractUtil
{
    [JSExport]
    internal static string ExtractText(string filename, byte[] content)
    {
        if (content.Length == 0)
        {
            return "";
        }
        
        var stream = new BinaryData(content).ToStream();

        // zip file signature
        if (content is [0x50, 0x4B, 0x03, 0x04, ..])
        {
            if (filename.EndsWith(".docx")) return DocxExtract.Extract(stream);
            if (filename.EndsWith(".xlsx")) return XlsxExtract.Extract(stream);
            if (filename.EndsWith(".pptx")) return PptxExtract.Extract(stream);
        }
        
        // pdf signature
        if (content is [0x25, 0x50, 0x44, 0x46, 0x2D, ..]) return PdfExtract.Extract(stream);
        
        // html file
        if (filename.EndsWith(".html"))
        {
            using var reader = new StreamReader(stream);
            return Html2Md.Convert(reader.ReadToEnd());
        }
        
        // else treat as plain text
        {
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
    }
}
