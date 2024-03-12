import { TauriLib } from "./TauriComunication";

let greetInputEl: HTMLInputElement | null;
let greetMsgEl: HTMLElement | null;

async function greet(user: string): Promise<string | null> {
    // Learn more about Tauri commands at https://tauri.app/v1/guides/features/command
    let userData = {user: user, pass: "Hmm..."}

    try {
      return await TauriLib.invokePlugin<string>({ plugin: "TestApp", method: "login", data: userData });
    } catch (error) {
      return "ERR: " + error;
    }
}

window.addEventListener("DOMContentLoaded", () => {
  greetInputEl = document.querySelector("#greet-input");
  greetMsgEl = document.querySelector("#greet-msg");
  document.querySelector("#greet-form")?.addEventListener("submit", async (e) => {
    e.preventDefault();
    if (greetMsgEl) greetMsgEl.innerHTML = await greet(greetInputEl?.value ?? "") ?? "null";
  });
});
