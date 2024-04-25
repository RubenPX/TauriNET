using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TauriComunication {
	public class Utils {
		public static T ParseObject<T>(object data) {
			if (data == null) throw new ArgumentNullException("parameter data is null");
			return ((JObject)data).ToObject<T>()!;
		}
	}
}
