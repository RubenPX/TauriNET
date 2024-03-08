using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TauriComunication.Route;

namespace TauriComunication {
    public class PluginInfo {
        public string dllPath { get; private set; }
        public Assembly assembly { get; private set; }
        public Type[] types { get; private set; }
        public List<MethodInfo> methods { get; private set; } = new List<MethodInfo>();

        public string PluginName { get; private set; }

        public PluginInfo(string dllPath) {
            this.assembly = Assembly.LoadFrom(dllPath);
            Console.WriteLine($"Loaded dll {Path.GetFileNameWithoutExtension(dllPath)}");

            this.PluginName = assembly.GetName().Name;

            this.types = this.assembly.GetTypes();

            foreach (var type in this.types) {
                var compatibleMethods = type.GetMethods(BindingFlags.Static | BindingFlags.Public).Where(m => {
                    // Verify custom RouteMethodAttribute atribute
                    if (m.GetCustomAttribute<RouteMethodAttribute>() != null) return false;

                    // Verify parameters
                    ParameterInfo[] ps = m.GetParameters();
                    if (ps.Length != 1) return false;
                    if (ps[0].ParameterType != typeof(RouteRequest)) return false;

                    // verify return
                    if (m.ReturnType != typeof(RouteResponse)) return false;

                    return true;
                }).ToArray();

                foreach (var mthd in compatibleMethods) {
                    Console.WriteLine($"RouteMethod found: {mthd.GetType().Name}: {mthd.Name}");
                }

                this.methods.AddRange(compatibleMethods);
            }
        }

        public MethodInfo? GetMethodName(string name) => methods.FirstOrDefault(m => m.Name == name, null);
    }
}
