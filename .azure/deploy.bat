@echo off
REM Deploy script para Azure (Windows)

setlocal enabledelayedexpansion

REM Cores (emuladas)
REM Usando echo com formatação
echo.
echo ============================================
echo === Codex Azure Deployment (Windows) ===
echo ============================================
echo.

REM Verificar se AZ CLI está instalado
az --version >nul 2>&1
if errorlevel 1 (
    echo [ERROR] Azure CLI nao esta instalado.
    echo Faça download em: https://aka.ms/installazurecliwindows
    pause
    exit /b 1
)

REM Configurações
set PROJECT_NAME=codex
set RESOURCE_GROUP=%PROJECT_NAME%-rg
set LOCATION=brazilsouth
set REGISTRY_NAME=%PROJECT_NAME%acr

REM Pedir connection string do Neon
echo.
set /p NEON_CONNECTION_STRING="Entre com a connection string do Neon (postgres://...): "

REM Fazer login no Azure
echo.
echo [INFO] Fazendo login no Azure...
call az login

REM Obter subscription ID
for /f "tokens=*" %%i in ('az account show --query id -o tsv') do set SUBSCRIPTION_ID=%%i
echo [INFO] Subscription ID: %SUBSCRIPTION_ID%

REM Criar resource group
echo.
echo [INFO] Criando Resource Group: %RESOURCE_GROUP%...
call az group create --name %RESOURCE_GROUP% --location %LOCATION%

REM Build e push da imagem Docker
echo.
echo [INFO] Build da imagem Docker...
cd /d "%~dp0..\.."
call docker build -f backend\Dockerfile -t %REGISTRY_NAME%.azurecr.io/codexapi:latest backend\

if errorlevel 1 (
    echo [ERROR] Falha no build da imagem Docker
    pause
    exit /b 1
)

REM Login no ACR
echo.
echo [INFO] Fazendo login no Azure Container Registry...
call az acr login --name %REGISTRY_NAME% 2>nul || true

REM Push da imagem
echo.
echo [INFO] Push da imagem para ACR...
call docker push %REGISTRY_NAME%.azurecr.io/codexapi:latest

if errorlevel 1 (
    echo [ERROR] Falha ao fazer push da imagem
    pause
    exit /b 1
)

REM Deploy da infraestrutura com Bicep
echo.
echo [INFO] Deployando infraestrutura...
call az deployment sub create ^
  --location %LOCATION% ^
  --template-file .azure\infra\main.bicep ^
  --parameters ^
    location=%LOCATION% ^
    projectName=%PROJECT_NAME% ^
    environment=prod ^
    neonConnectionString="%NEON_CONNECTION_STRING%"

if errorlevel 1 (
    echo [ERROR] Falha no deployment da infraestrutura
    pause
    exit /b 1
)

REM Build do frontend
echo.
echo [INFO] Build do frontend...
cd /d "%~dp0..\..\frontend"
call npm install
call npm run build

if errorlevel 1 (
    echo [ERROR] Falha no build do frontend
    pause
    exit /b 1
)

REM Preparar ZIP do frontend
echo.
echo [INFO] Preparando arquivo ZIP do frontend...
powershell -Command "Compress-Archive -Path 'dist\*' -DestinationPath 'dist.zip' -Force"

if errorlevel 1 (
    echo [ERROR] Falha ao criar ZIP
    pause
    exit /b 1
)

REM Deploy do frontend para App Service
set APP_SERVICE_NAME=%PROJECT_NAME%-web
echo.
echo [INFO] Deployando frontend para App Service: %APP_SERVICE_NAME%...
call az webapp deployment source config-zip ^
  --resource-group %RESOURCE_GROUP% ^
  --name %APP_SERVICE_NAME% ^
  --src-path dist.zip

if errorlevel 1 (
    echo [ERROR] Falha no deployment do frontend
    pause
    exit /b 1
)

echo.
echo ============================================
echo [SUCCESS] Deployment concluido com sucesso!
echo ============================================
echo.
echo URLs geradas:
echo  - API: https://%PROJECT_NAME%-api.brazilsouth.azurecontainerapps.io
echo  - Frontend: https://%PROJECT_NAME%-web.azurewebsites.net
echo.
pause
