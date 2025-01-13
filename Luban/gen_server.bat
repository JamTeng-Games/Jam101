set WORKSPACE=.
set LUBAN_DLL=%WORKSPACE%\Tools\Luban\Luban.dll
set CONF_ROOT=.
@REM set OUT_CODE_PATH="\\192.168.31.128\jam\dev\jam101_server\lualib\Config\Code"
@REM set OUT_DATA_PATH="\\192.168.31.128\jam\dev\jam101_server\lualib\Config\Data"
set OUT_CODE_PATH="\\106.14.59.20\root\dev\jam101_server\lualib\Config\Code"
set OUT_DATA_PATH="\\106.14.59.20\root\dev\jam101_server\lualib\Config\Data"

dotnet %LUBAN_DLL% ^
    -t server ^
    -d lua ^
    -c lua-lua ^
    --conf %CONF_ROOT%\luban.conf ^
    -x outputCodeDir=%OUT_CODE_PATH% ^
    -x outputDataDir=%OUT_DATA_PATH%

pause