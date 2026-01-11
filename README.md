# Morning Launcher - Quick Start Guide

A simple Windows Forms application to launch multiple programs with one click.

## 📋 Prerequisites

- Windows OS (7, 8, 10, or 11)
- .NET Framework 4.8 (usually pre-installed on Windows 10/11)

## 🔨 How to Build

1. **Copy the files** to a folder on your Windows machine:
   - `MorningLauncher.cs` (the source code)
   - `compile.bat` (the build script)

2. **Run the batch file**:
   - Double-click `compile.bat`
   - OR open Command Prompt in the folder and run: `compile.bat`

3. **Success!** If compilation succeeds, you'll see `MorningLauncher.exe` in the same folder.

## 🚀 How to Use

### First Time Setup:

1. **Launch** `MorningLauncher.exe`
2. **Go to the Settings tab**
3. **Add applications** using either:
   - **"Browse & Add..."** - Opens a file picker to select .exe files
   - **"Add Path Manually"** - Type/paste the full path
4. **Save** - Paths are automatically saved to `apps.txt`

### Daily Use:

1. **Launch** `MorningLauncher.exe`
2. **Click** the big "RUN ALL APPLICATIONS" button
3. **Done!** All your apps start, and the launcher closes automatically

## 📁 Files Created

- `apps.txt` - Created automatically in the same folder as the .exe
  - Contains your list of application paths (one per line)
  - Can be manually edited with Notepad if needed

## 💡 Tips

- **Add to Startup**: Place a shortcut to `MorningLauncher.exe` in your Windows Startup folder
  - Press `Win + R`, type `shell:startup`, press Enter
  - Create a shortcut to the launcher in that folder

- **Portable**: The entire app is just the .exe and apps.txt - easy to move or backup

- **Troubleshooting**: If an app fails to launch, check that:
  - The path in `apps.txt` is correct
  - The executable exists and is not moved/deleted
  - You have permissions to run it

## 🛠️ Advanced: Manual Compilation

If you need to use a different .NET Framework version, edit `compile.bat` and change the path:

```batch
REM For .NET 4.7:
set CSC="C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe"

REM For 32-bit systems:
set CSC="C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe"
```

Or compile manually from Command Prompt:

```cmd
"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe" /target:winexe /out:MorningLauncher.exe /reference:System.Windows.Forms.dll /reference:System.Drawing.dll MorningLauncher.cs
```

## 📝 Example apps.txt

```
C:\Program Files\Google\Chrome\Application\chrome.exe
C:\Program Files\Microsoft Office\root\Office16\OUTLOOK.EXE
C:\Users\YourName\Desktop\MyApp.exe
```

---

**Enjoy your streamlined morning routine!** ☕
