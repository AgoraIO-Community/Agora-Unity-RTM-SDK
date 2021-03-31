@echo off
::=============================================================================
:: The top level script to execute building the project and
:: extracting the end product for the Unity Windows plugin files.
::    Parameters:  #1 => Zip file path for output, if omitted, assume RTM_WinDLL.zip
::=============================================================================
echo ==================================================================
echo kicking off top level build process on Windows.
echo ==================================================================

SETLOCAL ENABLEDELAYEDEXPANSION
SET ERRORLEVEL
VERIFY > NUL

SET Local_Path=%~dp0
:: remove trailing slash
IF %Local_Path:~-1%==\ SET Local_Path=%Local_Path:~0,-1%

:: ~f1 is the absolutepath translate of %~1
SET ZipOutput_Path=%~f1
IF "%ZipOutput_Path%"=="" (
    SET ZipOutput_Path=%Local_Path%\RTM_WinDLL.zip
)
:: Calling build script
CALL :BuildForMachine 32
SET ErrLvl=!ERRORLEVEL!
if %ErrLvl%==0 (
CALL :BuildForMachine 64
SET ErrLvl=!ERRORLEVEL!
)

if %ErrLvl%==0 (
CALL :ZipRelease %ZipOutput_Path%
SET ErrLvl=!ERRORLEVEL!
)

if %ErrLvl%==0 (
    echo Done. Library files have been archived into %ZipOutput_Path%
    echo.
)

echo ================================================================================
ENDLOCAL

EXIT /B %ErrLvl%
:: ====================== End of Main batch body ==============================

:BuildForMachine
echo -------------------------------------------------------------------------------- 
set MachineArch=%~1
echo start build unity-sdk-build %MachineArch% bits in %Local_Path%
call "%Local_Path%\compile-windows.bat" %MachineArch% agoraRTMCWrapper\agoraRTMCWrapper.sln Release 2019
Set ErrorLevel=!ERRORLEVEL!
echo -------------------------------------------------------------------------------- 
EXIT /B %ErrorLevel%


:ZipRelease
echo -------------------------------------------------------------------------------- 
set ZipFile=%~1
if exist %ZipFile% del %ZipFile% > NUL

if exist %Local_Path%\unity (
    rmdir /S /Q %Local_Path%\unity 
)

mkdir %Local_Path%\unity\x86
copy %Local_Path%\agoraRTMCWrapper\Release\Win32\agoraRTMCWrapper.dll %Local_Path%\unity\x86
copy %Local_Path%\sdk\x86\dll\agora_rtm_sdk.dll %Local_Path%\unity\x86

mkdir %Local_path%\unity\x86_64
copy %Local_Path%\agoraRTMCWrapper\Release\x64\agoraRTMCWrapper.dll %Local_Path%\unity\x86_64
copy %Local_Path%\sdk\x64\dll\agora_rtm_sdk.dll %Local_Path%\unity\x86_64

powershell -command "& Compress-Archive %Local_Path%\unity %ZipFile%"
Set ErrorLevel=!ERRORLEVEL!
echo -------------------------------------------------------------------------------- 
EXIT /B %ErrorLevel%
