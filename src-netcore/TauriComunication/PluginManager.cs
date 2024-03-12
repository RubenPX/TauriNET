using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Reflection;
using System.Text.Json.Serialization;
using TauriComunication.Route;

namespace TauriComunication {
    public class PluginManager {
        private static PluginManager instance;

        private readonly PluginInfo[] plugins;

        private PluginManager() {
            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) => AssemblyDependency.AssemblyResolve(sender, args, Directory.GetCurrentDirectory());
			this.plugins = this.loadPluginsRoutes().ToArray();
		}

        private List<PluginInfo> loadPluginsRoutes() {
            var pluginsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "plugins");

            if (!Directory.Exists(pluginsDirectory)) {
                Console.WriteLine("Plugins directory does not exist");
                return [];
            }

            string[] dllList = Directory.GetFiles(pluginsDirectory, "*.plugin.dll");

            // Instance with list of nulls
            List<PluginInfo> plugins = new List<PluginInfo>();

            foreach (var dllPath in dllList) {
                try {
                    plugins.Add(new PluginInfo(dllPath));
                } catch (Exception ex) {
                    Console.WriteLine($"Failed to load {Path.GetFileName(dllPath)}: {ex.Message}");
                }
            }

            return plugins;
        }

        internal RouteResponse handleRoute(RouteRequest routeRequest) {
			RouteResponse preResponse = new RouteResponse() { id = routeRequest.id };

            if (routeRequest == null) return preResponse.Error("Object RouteRequest is required");
            if (routeRequest.id == null) return preResponse.Error("String parameter id is required");
            if (routeRequest.plugin == null) return preResponse.Error("string parameter plugin is required");
            if (routeRequest.method == null) return preResponse.Error("string parameter method is required");

            // Convert to object
            if (routeRequest.data.GetType().FullName == typeof(JObject).FullName) routeRequest.data = ((JObject)routeRequest.data).ToObject(typeof(object));

			PluginInfo? foundPlugin = null;
			try {
				foundPlugin = this.plugins.Where(x => x.PluginName == routeRequest.plugin || x.PluginName == $"{routeRequest.plugin}.plugin").First();
			} catch (InvalidOperationException) {
                Console.WriteLine($"[PluginManager] Plugin {routeRequest.plugin} not found...");
                return preResponse.Error($"Plugin {routeRequest.plugin} not found...");
            }
            MethodInfo? foundMethod;
			try {
                foundMethod = foundPlugin.methods.Where(m => m.Name == routeRequest.method).First();
			} catch (InvalidOperationException) {
                Console.WriteLine($"[{routeRequest.plugin}] Method {routeRequest.method} not found...");
				return preResponse.Error($"Method {routeRequest.method} not found...");
            }

            try {
                return (RouteResponse?)foundMethod.Invoke(null, [routeRequest, preResponse]); ;
			} catch (Exception ex) {
                Console.WriteLine($"[{routeRequest.plugin}][{routeRequest.method}] error: {ex.ToString()}");
                return preResponse.Error($"{ex.Message}");
			}
        }

        /// <summary>
        /// This method handle all requests and redirect to any RouteHandler
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public static string processRequest(string? inputData) {
            if (inputData is null or "") return JsonConvert.SerializeObject(new RouteResponse() { error = "Input is empty..." });
            if (instance == null) instance = new PluginManager();

			// If is init, avoid send process
			if (inputData is "INIT") return JsonConvert.SerializeObject(new RouteResponse() { data = "OK!" });

            RouteRequest request;

			try {
				request = JsonConvert.DeserializeObject<RouteRequest>(inputData);
			} catch (Exception) {
                return JsonConvert.SerializeObject(new RouteResponse() { error = "Failed to parse request JSON" });
			}

            try {
				RouteResponse response = instance.handleRoute(request);
				return JsonConvert.SerializeObject(response);
			} catch (Exception ex) {
				Console.WriteLine($"[PluginManager] Failed to process request. {ex.Message}");
				return JsonConvert.SerializeObject(new RouteResponse() { error = $"Failed to process request. {ex.Message}" });
			}
        }
    }
}
