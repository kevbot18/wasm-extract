using System;
using System.Runtime.InteropServices.JavaScript;

namespace extract;

internal static partial class Html2Md
{
    [JSImport("globalThis.html2md")]
    private static partial string InternalConvert(string status);

    public static string Convert(string status)
    {
        var res = InternalConvert(status);
        if (res[0] == 'e')
        {
            throw new InvalidOperationException(res[1..]);
        }

        return res[1..];
    }
}