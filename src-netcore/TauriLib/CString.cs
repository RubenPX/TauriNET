using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TauriIPC; 

public static class CString {
    private static unsafe delegate*<char*, int, byte*> CopyToCString;

    [UnmanagedCallersOnly]
    public static unsafe void SetCopyToCStringFunctionPtr(delegate*<char*, int, byte*> copyToCString) => CopyToCString = copyToCString;

    [UnmanagedCallersOnly]
    public unsafe static byte* GetNameAsCString() {
        var name = TestApp.Main.getStringFromFile();
        fixed (char* ptr = name) {
            return CopyToCString(ptr, name.Length);
        }
    }
}
