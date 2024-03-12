using System.Text.Json;

namespace TauriComunication.Route;

public class RouteRequest {
    public string id;
    public string plugin;
    public string method;
    public object? data;
}
