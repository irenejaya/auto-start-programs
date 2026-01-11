@echo off
echo ========================================
echo Morning Launcher - Build Script
echo ========================================
echo.

REM Set the path to the C# compiler
set CSC="C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe"

REM Check if compiler exists
if not exist %CSC% (
    echo ERROR: C# compiler not found at %CSC%
    echo Please ensure .NET Framework 4.8 is installed.
    pause
    exit /b 1
)

echo Compiling MorningLauncher.cs...
echo.

REM Compile the application
%CSC% /target:winexe /out:MorningLauncher.exe /reference:System.Windows.Forms.dll /reference:System.Drawing.dll MorningLauncher.cs

if %ERRORLEVEL% EQU 0 (
    echo.
    echo ========================================
    echo BUILD SUCCESSFUL!
    echo ========================================
    echo.
    echo Output: MorningLauncher.exe
    echo.
    echo You can now run MorningLauncher.exe
    echo.
) else (
    echo.
    echo ========================================
    echo BUILD FAILED!
    echo ========================================
    echo.
    echo Please check the error messages above.
    echo.
)

pause
