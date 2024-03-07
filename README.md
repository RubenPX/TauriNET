# Tauri + Typescript + C#

This template should help get you started developing with Tauri, .NET and TypeScript in Vite. 

## TODOS
- [X] Call C# code from Tauri
- [X] Call C# code from tauri with parameters and return string
- [X] Allow comunication betwen JSON data
- [ ] Call Tauri code from C#
- [ ] Hide exposed dlls to make a single exe ([Embed files](https://tauri.app/v1/guides/building/resources/))

## Debuging

You can debug .net dll [ataching VisualStudio to process](https://learn.microsoft.com/en-us/visualstudio/debugger/attach-to-running-processes-with-the-visual-studio-debugger) to loaded tauri app

## How it works

You can replace frontend as you want

To invoke NetHost code, you need install tauri API and run this code

```javascript
import { invoke } from "@tauri-apps/api/tauri"

let name = "";
let greetMsg = ""

async function greet(){
  // Learn more about Tauri commands at https://tauri.app/v1/guides/features/command
  greetMsg = await invoke("greet", { name })
}
```

## Example working

```yaml
[✔] Environment
    - OS: Windows 10.0.22631 X64
    ✔ WebView2: 122.0.2365.66
    ✔ rustc: 1.74.1 (a28077b28 2023-12-04)
    ✔ cargo: 1.74.1 (ecb9851af 2023-10-18)
    ✔ rustup: 1.26.0 (5af9b9484 2023-04-05)
    ✔ Rust toolchain: stable-x86_64-pc-windows-msvc (default)
    - node: 21.6.2
    - pnpm: 8.6.1
    - npm: 9.6.6

[-] Packages
    - tauri [RUST]: 1.6.1
    - tauri-build [RUST]: 1.5.1
    - wry [RUST]: 0.24.7
    - tao [RUST]: 0.16.7
    - @tauri-apps/api [NPM]: 1.5.3
    - @tauri-apps/cli [NPM]: 1.5.10

[-] App
    - build-type: bundle
    - CSP: unset
    - distDir: ../dist
    - devPath: http://localhost:1420/
    - bundler: Vite
```

Latest working example: [Download](https://github.com/RubenPX/TauriNET/releases/download/0.1.3/TauriNET_example.zip)

## References

- [Tauri](https://tauri.app)
- [netcorehost](https://github.com/OpenByteDev/netcorehost)
