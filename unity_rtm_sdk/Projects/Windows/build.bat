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
SET OUTPUT_PACKAGE=RTM_windows.zip
SET URL_CONFIG=%CURDIR%\url_config.txt
mkdir agoraRTMCWrapper\sdk
CALL :download_library x86
echo x86 download_library error=%ERRORLEVEL%
CALL :download_library x64
echo x64 download_library error=%ERRORLEVEL%

REM Preparing step
cd agoraRTMCWrapper
SET ARCH=x86
for /F "delims=" %%a in ('python ..\GetUrl.py %ARCH% filename') do SET ZIP_FILE=%%a
powershell -command "& .\prep-win-project.bat %ARCH% ..\%ZIP_FILE%"
echo -------------- prep %ARCh% error=%ERRORLEVEL%
SET ARCH=x64
for /F "delims=" %%a in ('python ..\GetUrl.py %ARCH% filename') do SET ZIP_FILE=%%a
powershell -command "& .\prep-win-project.bat %ARCH% ..\%ZIP_FILE%"
echo -------------- prep %ARCh% error=%ERRORLEVEL%

powershell -command "& .\unity-sdk-build-windows-release.bat ..\RTM_WinDLL.zip"

EXIT /B %ERRORLEVEL%

:download_library
SET ARCH=%~1
for /F "delims=" %%a in ('python .\GetUrl.py %ARCH%') do SET DOWNLOAD_URL=%%a
for /F "delims=" %%a in ('python .\GetUrl.py %ARCH% filename') do SET ZIP_FILE=%%a
echo %DOWNLOAD_URL%
echo %ZIP_FILE%
powershell -command "& wget %DOWNLOAD_URL% -OutFile %ZIP_FILE%"
echo wget error=%ERRORLEVEL%
EXIT /B %ERRORLEVEL%
