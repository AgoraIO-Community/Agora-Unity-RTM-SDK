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
SET ARCH=%~1
SET InputZip=%~2

echo ==================================================================
echo preparing project files for %ARCH%
echo ==================================================================

SET Local_Path=%~dp0
:: remove trailing slash
IF %Local_Path:~-1%==\ SET Local_Path=%Local_Path:~0,-1%

echo %InputZip% == %ARCH%
SET TargetDir=sdk
mkdir %TargetDir%
SET FromDir=Src-%ARCH%
powershell -command "& Expand-Archive -Path %InputZip% -DestinationPath %FromDir% -Force"
move %FromDir%\product_sdk\sdk %TargetDir% 
cd %TargetDir%
ren sdk %ARCH%
echo ==================================================================
echo done for %ARCH% preparation.
echo ==================================================================

