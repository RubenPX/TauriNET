namespace TestApp {
    public class Main {
        public static string login(string? name) {
            var currentPath = Directory.GetCurrentDirectory();
            var filePath = Path.Combine(currentPath, "Test.txt");

            if (name == null && File.Exists(filePath)) {
                var txtLogin = File.ReadAllText(filePath);
                var loginName = txtLogin.Substring("Last login: ".Length);

                return $"Loged as {loginName}";
            }

            if (name == null) return "Wops... Not loged in";

            if (!File.Exists(filePath)) {
                File.Create(filePath).Close();
                File.WriteAllText(filePath, $"Last login: {name}");
            }

            return $"Welcome back {name}";
        }
    }
}
