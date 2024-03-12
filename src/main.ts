import { TauriLib } from "./TauriComunication";

let greetInputEl: HTMLInputElement | null;
let greetMsgEl: HTMLElement | null;

async function greet(user: string) {
    // Learn more about Tauri commands at https://tauri.app/v1/guides/features/command
    let userData = {user: user, pass: "Hmm..."}

    let response = await TauriLib.invokePlugin<string>({ plugin: "TestApp", method: "login", data: userData });
    console.log(response);

    if (response.data) {
      return response.data;
    } else if (response.error) {
      return "ERR: " + response.error;
    }

    return JSON.stringify(response)
}

window.addEventListener("DOMContentLoaded", () => {
  greetInputEl = document.querySelector("#greet-input");
  greetMsgEl = document.querySelector("#greet-msg");
  document.querySelector("#greet-form")?.addEventListener("submit", async (e) => {
    e.preventDefault();
    if (greetMsgEl) greetMsgEl.innerHTML = await greet(greetInputEl?.value ?? "");
  });
});
