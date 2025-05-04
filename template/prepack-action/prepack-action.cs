
void CopyDirectory(string sourceDir, string destinationDir)
{
    Directory.CreateDirectory(destinationDir);

    // Copy files
    foreach (string filePath in Directory.GetFiles(sourceDir))
    {
        string fileName = Path.GetFileName(filePath);
        string destFilePath = Path.Combine(destinationDir, fileName);
        File.Copy(filePath, destFilePath, true);
    }

    // Copy subdirectories
    foreach (string subDir in Directory.GetDirectories(sourceDir))
    {
        string directoryName = Path.GetFileName(subDir);
        string destSubDir = Path.Combine(destinationDir, directoryName);
        CopyDirectory(subDir, destSubDir);
    }
}


// Copy tauri sources for template content folder
string baseDir = "./content/taurinet-sources";
string srcTauriDir = Path.Combine(baseDir, "src-tauri");

Directory.CreateDirectory(srcTauriDir);

CopyDirectory("../src", Path.Combine(baseDir, "src"));
File.Copy("../index.html", Path.Combine(baseDir, "index.html"), true);
CopyDirectory("../src-netcore", Path.Combine(baseDir, "src-netcore"));
CopyDirectory("../src-tauri/src", Path.Combine(srcTauriDir, "src"));
