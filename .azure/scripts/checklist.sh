#!/bin/bash
# Setup Checklist interativo

echo ""
echo "╔════════════════════════════════════════════════════════════╗"
echo "║          CODEX DEPLOYMENT - SETUP CHECKLIST               ║"
echo "╚════════════════════════════════════════════════════════════╝"
echo ""

# Cores
GREEN='\033[0;32m'
RED='\033[0;31m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m'

declare -A checklist=(
    ["azure_cli"]="Azure CLI instalado"
    ["docker"]="Docker instalado"
    ["node"]="Node.js v20+"
    ["npm"]="npm instalado"
    ["dockerfile"]="Dockerfile existe"
    ["package_json"]="package.json existe"
    ["csproj"]="CodexAPI.csproj existe"
    ["neon_string"]="Connection string Neon copiada"
    ["azure_account"]="Conta Azure criada"
)

# Verificações
echo -e "${BLUE}VERIFICANDO REQUISITOS...${NC}"
echo ""

# 1. Azure CLI
if command -v az &> /dev/null; then
    checklist["azure_cli"]=1
    echo -e "${GREEN}✓${NC} Azure CLI instalado"
else
    checklist["azure_cli"]=0
    echo -e "${RED}✗${NC} Azure CLI NOT instalado"
fi

# 2. Docker
if command -v docker &> /dev/null; then
    checklist["docker"]=1
    echo -e "${GREEN}✓${NC} Docker instalado"
else
    checklist["docker"]=0
    echo -e "${RED}✗${NC} Docker NOT instalado"
fi

# 3. Node.js
if command -v node &> /dev/null; then
    VERSION=$(node -v | cut -d'v' -f2 | cut -d'.' -f1)
    if [ "$VERSION" -ge 20 ]; then
        checklist["node"]=1
        echo -e "${GREEN}✓${NC} Node.js v$(node -v | cut -d'v' -f2)"
    else
        checklist["node"]=0
        echo -e "${RED}✗${NC} Node.js versão antiga (need v20+)"
    fi
else
    checklist["node"]=0
    echo -e "${RED}✗${NC} Node.js NOT instalado"
fi

# 4. npm
if command -v npm &> /dev/null; then
    checklist["npm"]=1
    echo -e "${GREEN}✓${NC} npm instalado"
else
    checklist["npm"]=0
    echo -e "${RED}✗${NC} npm NOT instalado"
fi

# 5. Dockerfile
if [ -f "backend/Dockerfile" ]; then
    checklist["dockerfile"]=1
    echo -e "${GREEN}✓${NC} Dockerfile existe"
else
    checklist["dockerfile"]=0
    echo -e "${RED}✗${NC} Dockerfile NOT encontrado"
fi

# 6. package.json
if [ -f "frontend/package.json" ]; then
    checklist["package_json"]=1
    echo -e "${GREEN}✓${NC} package.json existe"
else
    checklist["package_json"]=0
    echo -e "${RED}✗${NC} package.json NOT encontrado"
fi

# 7. CodexAPI.csproj
if [ -f "backend/CodexAPI/CodexAPI.csproj" ]; then
    checklist["csproj"]=1
    echo -e "${GREEN}✓${NC} CodexAPI.csproj existe"
else
    checklist["csproj"]=0
    echo -e "${RED}✗${NC} CodexAPI.csproj NOT encontrado"
fi

echo ""
echo -e "${BLUE}VERIFICAÇÕES MANUAIS...${NC}"
echo ""

# Neon connection string
read -p "Você copiou a connection string do Neon? (s/n): " -r
if [[ $REPLY =~ ^[Ss]$ ]]; then
    checklist["neon_string"]=1
    echo -e "${GREEN}✓${NC} Connection string pronta"
else
    checklist["neon_string"]=0
    echo -e "${RED}✗${NC} Connection string NOT copiada"
fi

# Azure account
read -p "Você tem uma conta Azure criada? (s/n): " -r
if [[ $REPLY =~ ^[Ss]$ ]]; then
    checklist["azure_account"]=1
    echo -e "${GREEN}✓${NC} Conta Azure pronta"
else
    checklist["azure_account"]=0
    echo -e "${RED}✗${NC} Conta Azure NOT criada"
fi

# Resumo
echo ""
echo "╔════════════════════════════════════════════════════════════╗"
echo "║                      RESUMO FINAL                          ║"
echo "╚════════════════════════════════════════════════════════════╝"
echo ""

TOTAL=0
DONE=0

for key in "${!checklist[@]}"; do
    TOTAL=$((TOTAL + 1))
    if [ ${checklist[$key]} -eq 1 ]; then
        DONE=$((DONE + 1))
    fi
done

echo "Status: $DONE / $TOTAL ✓"
echo ""

if [ $DONE -eq $TOTAL ]; then
    echo -e "${GREEN}╔════════════════════════════════════════════════════════════╗${NC}"
    echo -e "${GREEN}║  ✅ TUDO PRONTO! VOCÊ PODE FAZER DEPLOY!                ║${NC}"
    echo -e "${GREEN}╚════════════════════════════════════════════════════════════╝${NC}"
    echo ""
    echo "Próximo passo:"
    echo "  Windows: .azure\\deploy.bat"
    echo "  Linux:   ./.azure/deploy.sh"
    echo ""
    exit 0
else
    echo -e "${RED}╔════════════════════════════════════════════════════════════╗${NC}"
    echo -e "${RED}║  ❌ FALTAM REQUISITOS!                                    ║${NC}"
    echo -e "${RED}╚════════════════════════════════════════════════════════════╝${NC}"
    echo ""
    echo "Por favor, instale os componentes faltantes:"
    echo ""
    
    if [ ${checklist["azure_cli"]} -eq 0 ]; then
        echo -e "${YELLOW}• Azure CLI${NC}"
        echo "  https://docs.microsoft.com/cli/azure/install-azure-cli"
    fi
    
    if [ ${checklist["docker"]} -eq 0 ]; then
        echo -e "${YELLOW}• Docker${NC}"
        echo "  https://www.docker.com/products/docker-desktop"
    fi
    
    if [ ${checklist["node"]} -eq 0 ]; then
        echo -e "${YELLOW}• Node.js v20+${NC}"
        echo "  https://nodejs.org"
    fi
    
    echo ""
    exit 1
fi
