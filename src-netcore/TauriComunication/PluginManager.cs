using Newtonsoft.Json;
using TauriComunication.Route;

namespace TauriComunication {
    internal class PluginManager {
        private static PluginManager instance;

        private readonly PluginInfo[] plugins;

        public PluginManager() {
            this.plugins = this.loadPluginsRoutes().ToArray();
        }

        private List<PluginInfo> loadPluginsRoutes() {
            var pluginsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "plugins");

            if (!Directory.Exists(pluginsDirectory)) {
                Console.WriteLine("Plugins directory does not exist");
                return [];
            }

            string[] dllList = Directory.GetFiles(pluginsDirectory, "*.dll");

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

        private RouteResponse handleRoute(RouteRequest routeRequest) {
            if (routeRequest == null) return new RouteResponse() { error = "Object RouteRequest is required" };
            if (routeRequest.id == null) return new RouteResponse() { error = "string parameter id is required" };
            if (routeRequest.plugin == null) return new RouteResponse() { error = "string parameter plugin is required" };
            if (routeRequest.method == null) return new RouteResponse() { error = "string parameter method is required" };

            // TODO: @RubenPX

            return new RouteResponse() { error = "OK! ;)" };
        }

        /// <summary>
        /// This method handle all requests and redirect to any RouteHandler
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        internal static string processRequest(string? inputData) {
            if (inputData != null || inputData == "") return JsonConvert.SerializeObject(new RouteResponse() { error = "Input is empty..." });

            RouteRequest request = JsonConvert.DeserializeObject<RouteRequest>(inputData);

            if (instance == null) instance = new PluginManager();

            return null;
        }
    }
}
