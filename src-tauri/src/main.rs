// Prevents additional console window on Windows in release, DO NOT REMOVE!!
// #![cfg_attr(not(debug_assertions), windows_subsystem = "windows")]

mod host_loader;

// Learn more about Tauri commands at https://tauri.app/v1/guides/features/command
#[tauri::command]
fn plugin_request(data_str: &str) -> String {
    let returned_string_utf8 = host_loader::run_method_utf8(data_str);
    format!("{}", returned_string_utf8)
}

fn main() {
    // Precarga el DLL de NetHost
    host_loader::get_instance();
    host_loader::run_method_utf8("INIT");

    // Run tauri
    tauri::Builder::default()
        .invoke_handler(tauri::generate_handler![plugin_request])
        .run(tauri::generate_context!())
        .expect("error while running tauri application");
}
