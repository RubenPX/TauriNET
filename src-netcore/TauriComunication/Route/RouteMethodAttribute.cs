namespace TauriComunication.Route;

/// <summary>
/// This atribute specifies entrypoint of any Route handler. This is searched by reflexion
/// <br /><br />
/// <code>
/// [<see cref="RouteHandler"/>]<br />
/// public static <see cref="RouteResponse"/> methodName(<see cref="RouteRequest"/> route)
/// </code>
/// </summary>
public class RouteMethodAttribute : Attribute {
	public string? methodName {  get; }

	public RouteMethodAttribute(string methodName) {
		this.methodName = methodName;
	}

	public RouteMethodAttribute() {}
}