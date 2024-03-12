using System.Runtime.InteropServices;

namespace TauriIPC;

public static class CString {
	private static unsafe delegate*<char*, int, byte*> CopyToCString;

	[UnmanagedCallersOnly]
	public static unsafe void SetCopyToCStringFunctionPtr(delegate*<char*, int, byte*> copyToCString) => CopyToCString = copyToCString;

	/* This function parse string from a region from memory an return a string region from a pointer */
	[UnmanagedCallersOnly]
	public static unsafe byte* runUTF8(/* byte* */IntPtr textPtr, int textLength) {
		var text = Marshal.PtrToStringUTF8(textPtr, textLength);
		if (text == null || text.Length == 0) text = null;

		var loginText = TauriComunication.PluginManager.processRequest(text);

		fixed (char* ptr = loginText) {
			return CopyToCString(ptr, loginText.Length);
		}
	}
}
