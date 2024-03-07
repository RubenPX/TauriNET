# Tauri + Typescript + C#

This template should help get you started developing with Tauri, .NET and TypeScript in Vite. 

Latest working example: https://github.com/RubenPX/TauriNET/releases/download/0.1.2/build.zip

## TODOS

- [X] Invoke from Tauri to NetHostFX
- [ ] Allow Tauri pass to C# String parameters
- [ ] Allow to send & parse JSON Data betwen processes

## Extra

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

