use core::slice;
use std::ffi::{c_char, CString};

use lazy_static::lazy_static;
use netcorehost::{hostfxr::AssemblyDelegateLoader, nethost, pdcstr};

lazy_static! {
    static ref ASM: AssemblyDelegateLoader = {
        let hostfxr = nethost::load_hostfxr().unwrap();

        let context = hostfxr
            .initialize_for_runtime_config(pdcstr!("TauriIPC.runtimeconfig.json"))
            .expect("Wops... Invalid runtime configuration");

        context
            .get_delegate_loader_for_assembly(pdcstr!("TauriIPC.dll"))
            .expect("Wops... Failed to load DLL")
    };
}

pub fn get_instance() -> &'static AssemblyDelegateLoader {
    &ASM
}

unsafe extern "system" fn copy_to_c_string(ptr: *const u16, length: i32) -> *mut c_char {
    let wide_chars = unsafe { slice::from_raw_parts(ptr, length as usize) };
    let string = String::from_utf16_lossy(wide_chars);
    let c_string = match CString::new(string) {
        Ok(c_string) => c_string,
        Err(_) => return std::ptr::null_mut(),
    };
    c_string.into_raw()
}

pub fn run_method_utf8(string_data: &str) -> String {
    let instance = get_instance();

    let set_copy_to_c_string = instance
        .get_function_with_unmanaged_callers_only::<fn(f: unsafe extern "system" fn(*const u16, i32) -> *mut c_char)>(
            pdcstr!("TauriIPC.CString, TauriIPC"),
            pdcstr!("SetCopyToCStringFunctionPtr"),
        ).unwrap();

    set_copy_to_c_string(copy_to_c_string);

    let handler_utf8 = instance
        .get_function_with_unmanaged_callers_only::<fn(text_ptr: *const u8, text_length: i32) -> *mut c_char>(
            pdcstr!("TauriIPC.CString, TauriIPC"),
            pdcstr!("runUTF8"),
        )
        .unwrap();

    let ptr_string = handler_utf8(string_data.as_ptr(), string_data.len() as i32);
    let data = unsafe { CString::from_raw(ptr_string) };

    format!("{}", data.to_string_lossy())
}
