using System;
using System.IO;
using System.Linq;
using System.Diagnostics;

int RunCommand(string fileName, string arguments, string? workDir = null)
{
    var process = new Process();
    process.StartInfo.FileName = fileName;
    process.StartInfo.Arguments = arguments;
    process.StartInfo.WorkingDirectory = workDir ?? Directory.GetCurrentDirectory();
    process.StartInfo.UseShellExecute = false;
    process.StartInfo.RedirectStandardInput = false;
    process.StartInfo.RedirectStandardOutput = false;
    process.StartInfo.RedirectStandardError = false;
    process.Start();

    process.WaitForExit();
    return process.ExitCode;
}

/// Running create-tauri-app
var beforeDirectories = Directory.GetDirectories("./");
Console.WriteLine("Running 'create-tauri-app'...");
if (RunCommand("pnpm", "create tauri-app") != 0)
{
    Console.Error.WriteLine("Error: 'create-tauri-app' command failed.");
    Environment.Exit(-1);
}

/// Get new directory name
var afterDirectories = Directory.GetDirectories("./");
var newDirectories = afterDirectories.Except(beforeDirectories).ToArray();
if (newDirectories.Length != 1)
{
    Console.Error.WriteLine("Error: internal error, expected exactly one new directory.");
    Environment.Exit(-1);
}

/// Remove unnecessary files
Directory.Delete(newDirectories[0] + "/src", true);

/// Copy TauriNET template sources
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

CopyDirectory("./taurinet-sources", $"./{newDirectories[0]}");

/// append dependencies package for cargo.toml 
RunCommand("cargo", "add netcorehost lazy_static", $"./{newDirectories[0]}/src-tauri");

/// remove template files
Directory.Delete("./taurinet-sources", true);

Console.WriteLine("Complete!");
