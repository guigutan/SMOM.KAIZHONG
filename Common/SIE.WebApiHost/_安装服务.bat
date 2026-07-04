@echo off
>nul 2>&1 "%SYSTEMROOT%\system32\cacls.exe" "%SYSTEMROOT%\system32\config\system"
if '%errorlevel%' NEQ '0' (
echo run as administrator...
goto UACPrompt
) else ( goto gotAdmin )
:UACPrompt
echo Set UAC = CreateObject^("Shell.Application"^) > "%temp%\getadmin.vbs"
echo UAC.ShellExecute "%~s0", "", "", "runas", 1 >> "%temp%\getadmin.vbs"
"%temp%\getadmin.vbs"
exit /B
:gotAdmin

@echo off
rem install windows service 
%~dp0/SIE.WebApiHost.exe install 
rem start service
%~dp0/SIE.WebApiHost.exe start 
rem uninstall windows service 
rem dotnet SIE.WebApiHost.dll uninstall 

pause