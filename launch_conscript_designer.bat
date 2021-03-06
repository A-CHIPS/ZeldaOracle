@echo off

REM # Update the bin folder's content and binaries files
echo GameBuilder.exe -debug -out "ZeldaOracle\ConscriptDesigner\bin" -game "ZeldaOracle\Game\bin" -editor "ZeldaOracle\GameEditor\bin" -content "ZeldaOracle\GameContent\ZeldaContent.contentproj"
GameBuilder.exe -debug -out "ZeldaOracle\ConscriptDesigner\bin" -game "ZeldaOracle\Game\bin" -editor "ZeldaOracle\GameEditor\bin" -content "ZeldaOracle\GameContent\ZeldaContent.contentproj"

REM # Check if there was an error
if %ERRORLEVEL% NEQ 0 (
  echo.
  echo ERROR: Failure with GameBuilder.exe
  pause
  exit
)

REM # Launch the designer
cd "ZeldaOracle\ConscriptDesigner\bin\Debug"
start ConscriptDesigner.exe "..\..\..\GameContent\ZeldaContent.contentproj"
exit