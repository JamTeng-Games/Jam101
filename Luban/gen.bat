set WORKSPACE=.
set LUBAN_DLL=%WORKSPACE%\Tools\Luban\Luban.dll
set CONF_ROOT=.
set OUT_CODE_PATH="..\Assets\Script\Runtime\Config\Gen"
set OUT_DATA_PATH="..\Assets\Res\Config\Gen"

dotnet %LUBAN_DLL% ^
    -t all ^
    -d json ^
	-c cs-simple-json ^
    --conf %CONF_ROOT%\luban.conf ^
	-x outputCodeDir=%OUT_CODE_PATH% ^
    -x outputDataDir=%OUT_DATA_PATH%

pause