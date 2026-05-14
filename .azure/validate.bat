@echo off
REM Script para validar configuração antes do deployment

setlocal enabledelayedexpansion

echo.
echo ============================================
echo Verificando pre-requisitos para deployment...
echo ============================================
echo.

set ERRORS=0

REM Verificar Azure CLI
echo -n "1. Azure CLI... "
az --version >nul 2>&1
if errorlevel 1 (
    echo [ERRO] Nao instalado
    set /a ERRORS=ERRORS+1
) else (
    echo [OK]
)

REM Verificar Docker
echo -n "2. Docker... "
docker --version >nul 2>&1
if errorlevel 1 (
    echo [ERRO] Nao instalado
    set /a ERRORS=ERRORS+1
) else (
    echo [OK]
)

REM Verificar Node.js
echo -n "3. Node.js... "
node --version >nul 2>&1
if errorlevel 1 (
    echo [ERRO] Nao instalado
    set /a ERRORS=ERRORS+1
) else (
    for /f "tokens=*" %%i in ('node --version') do echo [OK] %%i
)

REM Verificar npm
echo -n "4. npm... "
npm --version >nul 2>&1
if errorlevel 1 (
    echo [ERRO] Nao instalado
    set /a ERRORS=ERRORS+1
) else (
    for /f "tokens=*" %%i in ('npm --version') do echo [OK] %%i
)

REM Verificar Dockerfile
echo -n "5. Dockerfile (backend)... "
if exist "backend\Dockerfile" (
    echo [OK]
) else (
    echo [ERRO] Nao encontrado
    set /a ERRORS=ERRORS+1
)

REM Verificar package.json
echo -n "6. package.json (frontend)... "
if exist "frontend\package.json" (
    echo [OK]
) else (
    echo [ERRO] Nao encontrado
    set /a ERRORS=ERRORS+1
)

REM Verificar CodexAPI.csproj
echo -n "7. CodexAPI.csproj... "
if exist "backend\CodexAPI\CodexAPI.csproj" (
    echo [OK]
) else (
    echo [ERRO] Nao encontrado
    set /a ERRORS=ERRORS+1
)

REM Verificar appsettings.json
echo -n "8. appsettings.json... "
if exist "backend\CodexAPI\appsettings.json" (
    echo [OK]
) else (
    echo [ERRO] Nao encontrado
    set /a ERRORS=ERRORS+1
)

echo.

if %ERRORS% equ 0 (
    echo ============================================
    echo [SUCCESS] Tudo pronto para deployment!
    echo ============================================
    echo.
    echo Proximo passo: Execute deploy.bat
    echo.
    exit /b 0
) else (
    echo ============================================
    echo [ERROR] %ERRORS% erro(s) encontrado(s)
    echo ============================================
    echo.
    echo Instale os componentes faltantes e tente novamente.
    echo.
    pause
    exit /b 1
)
