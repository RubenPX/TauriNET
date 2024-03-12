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
  
  public static async invokePlugin<T>(data: Omit<PluginRequest, 'id'>): Promise<RouteResponse<T>> {
    let generatedRequest = {...data, id: this.generateID()};
    let invokeResponse = await invoke('plugin_request', { dataStr: JSON.stringify(generatedRequest) }) as string
    return JSON.parse(invokeResponse) as RouteResponse<T>
  }
}
