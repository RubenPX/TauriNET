namespace TestApp {
    public class Main {
        public static string getStringFromFile() {
            var currentPath = Directory.GetCurrentDirectory();
            var filePath = Path.Combine(currentPath, "Test.txt");

            if (!File.Exists(filePath)) {
                File.Create(filePath).Close();
                File.WriteAllText(filePath, "This is an example text from this file");
            }

            return File.ReadAllText(filePath);
        }
    }
}
