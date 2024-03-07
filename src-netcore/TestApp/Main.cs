using System.Diagnostics.Metrics;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TestApp {
    public static class Main {

        class User {
            public string user { get; set; }
            public string pass {  get; set; }
        }

        public static string login(string? jsonDataStr) {
            User? loginInfo = null;

            if (jsonDataStr != null) {
                try {
                    loginInfo = (User)JsonSerializer.Deserialize(jsonDataStr!, typeof(User));
                } catch (Exception ex) {
                    return $"Failed to parse JSON User object: {ex.Message}";
                }
            }

            var currentPath = Directory.GetCurrentDirectory();
            var filePath = Path.Combine(currentPath, "Test.txt");

            if (loginInfo != null && (loginInfo.user == null || loginInfo.user == "")) {
                if (File.Exists(filePath)) {
                    var txtLogin = File.ReadAllText(filePath);
                    var loginName = txtLogin.Substring("Last login: ".Length);

                    return $"Welcome back, {loginName}";
                }

                return "Woops... User is not saved";
            }

            if (!File.Exists(filePath)) File.Create(filePath).Close();
            File.WriteAllText(filePath, $"Last login: {loginInfo.user}");
            return $"Loged in {loginInfo.user}";
        }
    }
}
