@echo off
echo ========================================
echo   BedrockLauncher - Compilador
echo   por AlbertDevX
echo ========================================
echo.

echo [1/3] Restaurando paquetes NuGet...
dotnet restore

echo.
echo [2/3] Compilando proyecto...
dotnet build -c Release --no-restore

echo.
echo [3/3] Publicando ejecutable standalone...
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o ./publish

echo.
echo ========================================
echo   Compilacion completada!
echo   Ejecutable en: .\publish\BedrockLauncher.exe
echo ========================================
pause
