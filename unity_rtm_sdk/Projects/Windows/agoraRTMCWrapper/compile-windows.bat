::=================================================================
:: Batch script to execute building on a VisualStudio .sln project.
::=================================================================

@echo off
echo ==========================================================
echo compile-windows.bat start building windows solutions.
echo ==========================================================

SETLOCAL

IF "%1"=="32" (set Machine=x86) else (set Machine=x64)
set ProjName=%2
set Config=%3
set platformTool=%4

echo Machine: %Machine%
echo ProjName: %ProjName%
echo Config: %Config%
echo platformTool: %platformTool%

set Local_Path=%~dp0%
echo LocalPath: %Local_Path%
echo.
echo ((((((((((( ============== Compiling for =============== %Machine% )))))))))))
echo.
call "C:\Program Files (x86)\Microsoft Visual Studio\%platformTool%\Community\VC\Auxiliary\Build\vcvarsall.bat" %Machine%
msbuild %ProjName% /t:Rebuild /p:Configuration=%Config% /p:Platform=%Machine%
