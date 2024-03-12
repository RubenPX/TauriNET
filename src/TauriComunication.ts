import { invoke } from "@tauri-apps/api"

export type PluginRequest = {
  id: string
  plugin: string
  method: string
  data?: object
}

export type RouteResponse<T> = {
  id: string
  error?: string
  data?: T
}

export class TauriLib {
  private static num: number = 0;
  private static generateID() {
    return this.num++;
  }
  
  public static async invokePlugin<T>(data: Omit<PluginRequest, 'id'>): Promise<T | null> {
    let generatedRequest = {...data, id: this.generateID()};
    let invokeResponse = await invoke('plugin_request', { dataStr: JSON.stringify(generatedRequest) }) as string
    let jsonResponse = JSON.parse(invokeResponse) as RouteResponse<T>

    if (jsonResponse.error) throw new Error(jsonResponse.error)

    return jsonResponse.data ?? null as T | null;
  }
}
