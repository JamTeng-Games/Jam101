set WORKSPACE=.
set LUBAN_DLL=%WORKSPACE%\Tools\Luban\Luban.dll
set CONF_ROOT=.
set OUT_CODE_PATH="..\Assets\Script\Runtime\Config\Gen"
set OUT_DATA_PATH="..\Assets\Res\Config\Gen"
set TEMPLATE_PATH=".\custom_template\client"

dotnet %LUBAN_DLL% ^
    -t client ^
    -d json ^
    -c cs-simple-json ^
    --customTemplateDir %TEMPLATE_PATH% ^
    --conf %CONF_ROOT%\luban.conf ^
    -x outputCodeDir=%OUT_CODE_PATH% ^
    -x outputDataDir=%OUT_DATA_PATH%

pause