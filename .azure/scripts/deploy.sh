#!/bin/bash
# Deploy script para Azure

set -e

# Cores para output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m'

echo -e "${YELLOW}=== Codex Azure Deployment ===${NC}"

# Verificar se AZ CLI está instalado
if ! command -v az &> /dev/null; then
    echo -e "${RED}Azure CLI não está instalado. Instalando...${NC}"
    curl -sL https://aka.ms/InstallAzureCLIDeb | sudo bash
fi

# Configurações
PROJECT_NAME="codex"
RESOURCE_GROUP="${PROJECT_NAME}-rg"
LOCATION="brazilsouth"
REGISTRY_NAME="${PROJECT_NAME}acr"

# Pedir connection string do Neon
read -p "Entre com a connection string do Neon (postgres://...): " NEON_CONNECTION_STRING

# Fazer login no Azure
echo -e "${YELLOW}Fazendo login no Azure...${NC}"
az login

# Obter subscription ID
SUBSCRIPTION_ID=$(az account show --query id -o tsv)
echo -e "${GREEN}Subscription ID: ${SUBSCRIPTION_ID}${NC}"

# Criar resource group
echo -e "${YELLOW}Criando Resource Group: ${RESOURCE_GROUP}${NC}"
az group create --name ${RESOURCE_GROUP} --location ${LOCATION}

# Build e push da imagem Docker
echo -e "${YELLOW}Build da imagem Docker...${NC}"
cd ../.. # Voltar para raiz do projeto
docker build -f backend/Dockerfile -t ${REGISTRY_NAME}.azurecr.io/codexapi:latest backend/

# Login no ACR
echo -e "${YELLOW}Fazendo login no Azure Container Registry...${NC}"
az acr login --name ${REGISTRY_NAME} 2>/dev/null || true

# Push da imagem
echo -e "${YELLOW}Push da imagem para ACR...${NC}"
docker push ${REGISTRY_NAME}.azurecr.io/codexapi:latest

# Deploy da infraestrutura com Bicep
echo -e "${YELLOW}Deployando infraestrutura...${NC}"
az deployment sub create \
  --location ${LOCATION} \
  --template-file .azure/infra/main.bicep \
  --parameters \
    location=${LOCATION} \
    projectName=${PROJECT_NAME} \
    environment=prod \
    neonConnectionString="${NEON_CONNECTION_STRING}"

# Build e deploy do frontend
echo -e "${YELLOW}Build do frontend...${NC}"
cd frontend
npm install
npm run build

# Deploy do frontend para App Service
APP_SERVICE_NAME="${PROJECT_NAME}-web"
echo -e "${YELLOW}Deployando frontend para App Service: ${APP_SERVICE_NAME}...${NC}"
az webapp deployment source config-zip \
  --resource-group ${RESOURCE_GROUP} \
  --name ${APP_SERVICE_NAME} \
  --src-path dist.zip

echo -e "${GREEN}=== Deployment concluído com sucesso! ===${NC}"
