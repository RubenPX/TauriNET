namespace TauriComunication.Route;

public class RouteResponse {
    public string id;
    public string? error;
    public object? data;

    internal RouteResponse() { }

    public RouteResponse Ok(object? data = null) {
        this.data = data;
        return this;
    }

    public RouteResponse Error(string error) {
        this.data = error;
        return this;
    }
}
