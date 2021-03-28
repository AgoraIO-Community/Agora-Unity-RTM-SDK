::=============================================================================
:: This script assumes the native Windows has been downloaded to the
:: current directory.  It will extract the neccessary files for the 
:: VS Project to run.
::
::  Input: %1 : x86 or x64
::         %2 : zip file name for the build specified in %1
::
::=============================================================================
echo off
SET RTM_VERSION=%~1
SET DOWNLOAD_VERSION=%RTM_VERSION%
SET OUTPUT_PACKAGE="RTM_windows.zip"
mkdir agoraRTMCWrapper\sdk
CALL :download_library x86 , %DOWNLOAD_VERSION%
echo x86 download_library error=%ERRORLEVEL%
CALL :download_library x64 , %DOWNLOAD_VERSION%
echo x64 download_library error=%ERRORLEVEL%

REM Preparing step
cd agoraRTMCWrapper
SET ARCH=x86
powershell -command "& .\prep-win-project.bat %ARCH% ..\Agora_RTM_SDK_for_Windows_%ARCH%_Unity_%DOWNLOAD_VERSION%.zip"
echo -------------- prep %ARCh% error=%ERRORLEVEL%
SET ARCH=x64
powershell -command "& .\prep-win-project.bat %ARCH% ..\Agora_RTM_SDK_for_Windows_%ARCH%_Unity_%DOWNLOAD_VERSION%.zip"
echo -------------- prep %ARCh% error=%ERRORLEVEL%

powershell -command "& .\unity-sdk-build-windows-release.bat ..\RTM_WinDLL.zip"

EXIT /B %ERRORLEVEL%

:download_library
SET ARCH=%~1
SET DOWNLOAD_VERSION=%~2
SET DOWNLOAD_URL=https://download.agora.io/rtmsdk/release
SET ZIP_FILE=Agora_RTM_SDK_for_Windows_%ARCH%_Unity_%DOWNLOAD_VERSION%.zip
echo %DOWNLOAD_URL%/%ZIP_FILE%
powershell -command "& wget %DOWNLOAD_URL%/%ZIP_FILE% -OutFile %ZIP_FILE%"
echo wget error=%ERRORLEVEL%
EXIT /B %ERRORLEVEL%
