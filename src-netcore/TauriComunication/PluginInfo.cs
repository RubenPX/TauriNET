using System;
using System.Collections.Generic;
using System.Diagnostics;
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

		static byte[] loadFile(string filename) {
			FileStream fs = new FileStream(filename, FileMode.Open);
			byte[] buffer = new byte[(int)fs.Length];
			fs.Read(buffer, 0, buffer.Length);
			fs.Close();

			return buffer;
		}

		public PluginInfo(string dllPath) {
            this.assembly = AppDomain.CurrentDomain.Load(loadFile(dllPath));
            this.PluginName = assembly.GetName().Name;

			AppDomain.CurrentDomain.AssemblyResolve += (object? sender, ResolveEventArgs args) => AssemblyDependency.AssemblyResolve(sender, args, this.PluginName);
            // Debugger.Launch();

			Console.WriteLine($"Loaded dll {Path.GetFileNameWithoutExtension(dllPath)}. Name: {PluginName}");

            this.types = this.assembly.GetTypes();

            foreach (var type in this.types) {

                //Console.WriteLine($"Class: {type.Name}");
                //foreach (var item in type.GetMethods(BindingFlags.Static | BindingFlags.Public)) {
                //    string methds = string.Join(',', item.GetParameters().Select(t => $"{t.Name}, {t.ParameterType.FullName}"));
                //    Console.WriteLine($"Method: {item.Name}, {item.Attributes}, {methds}");
                //}

                var compatibleMethods = type.GetMethods(BindingFlags.Static | BindingFlags.Public).Where(m => {
                    // Verify custom RouteMethodAttribute atribute
                    if (m.GetCustomAttribute<RouteMethodAttribute>() == null) return false;

                    // Verify parameters
                    ParameterInfo[] ps = m.GetParameters();
                    if (ps.Length != 1 && ps.Length != 2) return false;
                    if (ps[0].ParameterType.FullName != typeof(RouteRequest).FullName) return false;
                    if (ps[1] != null && ps[1].ParameterType.FullName != typeof(RouteResponse).FullName) return false;

					// verify return
					if (m.ReturnType.FullName != typeof(RouteResponse).FullName) return false;

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
