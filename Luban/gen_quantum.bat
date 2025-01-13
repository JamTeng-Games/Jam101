set WORKSPACE=.
set LUBAN_DLL=%WORKSPACE%\Tools\Luban\Luban.dll
set CONF_ROOT=.
set OUT_CODE_PATH="..\Assets\QuantumUser\Simulation\Config\Gen"
set OUT_DATA_PATH="..\Assets\QuantumUser\Resources\QuantumConfig\Gen"
set TEMPLATE_PATH=".\custom_template\quantum"

dotnet %LUBAN_DLL% ^
    -t quantum ^
    -d json ^
    -c cs-simple-json ^
    --customTemplateDir %TEMPLATE_PATH% ^
    --conf %CONF_ROOT%\luban.conf ^
    -x outputCodeDir=%OUT_CODE_PATH% ^
    -x outputDataDir=%OUT_DATA_PATH%

pause