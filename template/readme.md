# TauriNET Template

This project is a TauriNET template creator (similar to this one):  
https://github.com/RubenPX/TauriNET/

# Quick Start

1. Install pnpm, the .NET 8 SDK, and the Rust toolchain.

2. Clone the TauriNET project  

https://github.com/RubenPX/TauriNET/

3. Create the NuGet TauriNET template package  

- On Windows (cmd.exe)
```bat
pack.bat
```

- On macOS or Linux
```sh
./pack.sh
```

4.	Install the generated NuGet template package

- On Windows

```bat
dotnet new install .\bin\Release\TauriNET.Template.1.0.0.nupkg
```

- On macOS or Linux
```sh
dotnet new install ./bin/Release/TauriNET.Template.1.0.0.nupkg
```

5. Create a new TauriNET project  

```bat
dotnet new tauri-net
```

6. Remove unnecessary folders (It's setup scripts)  

- On Windows (cmd.exe)  
```bat
rmdir /s /q .\setup
```

- On macOS or Linux  
```sh
rm -rf ./setup
```

# Warning

This script generates a vanilla base source code, but the Tauri packages you’ve selected are already configured (e.g., in package.json), so feel free to customize the source code to work with the packages you want to use.

# How It Works

![](./docs/flow.drawio.svg)

It’s very simple. First, run the `create-tauri-app` script via npx from the `setup` C# project.  
Second, copy the TauriNET sources from the packaged `taurinet-sources` into the newly created project folder inside the `setup` project.







