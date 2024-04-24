@echo off
:BEGIN
echo (1) SchemeSelector
echo (2) PolicyAndSchemes
CHOICE /N /C:123 /M "Pick mode"
IF ERRORLEVEL == 2 GOTO PolicyAndSchemes
IF ERRORLEVEL == 1 GOTO SchemeSelector
GOTO exit
:SchemeSelector
dotnet run --mode=scheme-selector
GOTO exit
:PolicyAndSchemes
dotnet run
GOTO exit
:exit
@exit