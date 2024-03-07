using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TauriIPC; 

public static class CString {
    private static unsafe delegate*<char*, int, byte*> CopyToCString;

    [UnmanagedCallersOnly]
    public static unsafe void SetCopyToCStringFunctionPtr(delegate*<char*, int, byte*> copyToCString) => CopyToCString = copyToCString;

    /* This function parse string from a region from memory an return a string region from a pointer */
    [UnmanagedCallersOnly]
    public unsafe static byte* runUTF8(/* byte* */IntPtr textPtr, int textLength) {
        var text = Marshal.PtrToStringUTF8(textPtr, textLength);
        if (text == null || text.Length == 0) text = null;

        Console.WriteLine(text);
        var loginText = TestApp.Main.login(text);

        fixed (char* ptr = loginText) {
            return CopyToCString(ptr, loginText.Length);
        }
    }
}
