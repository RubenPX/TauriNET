# Tauri + Svelte + Typescript + C#

This template should help get you started developing with Tauri, Svelte and TypeScript in Vite. 

This dll can talk with NetHostFX apps

[Latest running example](https://github.com/RubenPX/TauriNET/releases/download/0.1.1/TauriNET_Example.zip)

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

