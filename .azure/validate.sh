#!/bin/bash
# Script para validar configuração antes do deployment

echo "🔍 Verificando pré-requisitos para deployment..."
echo ""

# Cores
GREEN='\033[0;32m'
RED='\033[0;31m'
YELLOW='\033[1;33m'
NC='\033[0m'

ERRORS=0

# Verificar Azure CLI
echo -n "1. Azure CLI... "
if command -v az &> /dev/null; then
    echo -e "${GREEN}✓${NC}"
else
    echo -e "${RED}✗ Não instalado${NC}"
    ERRORS=$((ERRORS + 1))
fi

# Verificar Docker
echo -n "2. Docker... "
if command -v docker &> /dev/null; then
    echo -e "${GREEN}✓${NC}"
else
    echo -e "${RED}✗ Não instalado${NC}"
    ERRORS=$((ERRORS + 1))
fi

# Verificar Node.js
echo -n "3. Node.js... "
if command -v node &> /dev/null; then
    VERSION=$(node -v)
    echo -e "${GREEN}✓ ($VERSION)${NC}"
else
    echo -e "${RED}✗ Não instalado${NC}"
    ERRORS=$((ERRORS + 1))
fi

# Verificar npm
echo -n "4. npm... "
if command -v npm &> /dev/null; then
    VERSION=$(npm -v)
    echo -e "${GREEN}✓ ($VERSION)${NC}"
else
    echo -e "${RED}✗ Não instalado${NC}"
    ERRORS=$((ERRORS + 1))
fi

# Verificar Dockerfile
echo -n "5. Dockerfile (backend)... "
if [ -f "backend/Dockerfile" ]; then
    echo -e "${GREEN}✓${NC}"
else
    echo -e "${RED}✗ Não encontrado${NC}"
    ERRORS=$((ERRORS + 1))
fi

# Verificar package.json
echo -n "6. package.json (frontend)... "
if [ -f "frontend/package.json" ]; then
    echo -e "${GREEN}✓${NC}"
else
    echo -e "${RED}✗ Não encontrado${NC}"
    ERRORS=$((ERRORS + 1))
fi

# Verificar CodexAPI.csproj
echo -n "7. CodexAPI.csproj... "
if [ -f "backend/CodexAPI/CodexAPI.csproj" ]; then
    echo -e "${GREEN}✓${NC}"
else
    echo -e "${RED}✗ Não encontrado${NC}"
    ERRORS=$((ERRORS + 1))
fi

# Verificar se está em git repo
echo -n "8. Git repository... "
if [ -d ".git" ]; then
    echo -e "${GREEN}✓${NC}"
else
    echo -e "${YELLOW}⚠ Não é um repositório Git${NC}"
fi

echo ""
echo "📦 Espaço em disco..."
echo -n "  backend/: "
du -sh backend/ | awk '{print $1}'
echo -n "  frontend/: "
du -sh frontend/ | awk '{print $1}'

echo ""
if [ $ERRORS -eq 0 ]; then
    echo -e "${GREEN}✅ Tudo pronto para deployment!${NC}"
    echo ""
    echo "Próximo passo: Execute deploy.sh"
    exit 0
else
    echo -e "${RED}❌ $ERRORS erro(s) encontrado(s)${NC}"
    echo ""
    echo "Instale os componentes faltantes e tente novamente."
    exit 1
fi
