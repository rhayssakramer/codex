<div align="center">

# ☁️ CODEX Azure Deployment

**Guia Completo para Deploy da Aplicação CODEX no Microsoft Azure**

Este documento descreve a arquitetura, custos, serviços e passo a passo para deploying CODEX no Azure com otimização de custos.

[![Azure](https://img.shields.io/badge/Cloud-Microsoft%20Azure-0078D4?style=for-the-badge&logo=microsoft-azure)](https://azure.microsoft.com)
[![Container Apps](https://img.shields.io/badge/Backend-Container%20Apps-0078D4?style=for-the-badge&logo=microsoft-azure)](https://azure.microsoft.com/services/container-apps)
[![App Service](https://img.shields.io/badge/Frontend-App%20Service-0078D4?style=for-the-badge&logo=microsoft-azure)](https://azure.microsoft.com/services/app-service)

</div>

---

## 📋 Índice

- [Visão Geral](#-visão-geral)
- [Arquitetura Azure](#-arquitetura-azure)
- [Serviços Utilizados](#-serviços-utilizados)
- [Custos Detalhados](#-custos-detalhados)
- [Pré-requisitos](#-pré-requisitos)
- [Setup Inicial](#-setup-inicial)
- [Guia de Deployment](#-guia-de-deployment)
- [Configuração Pós-Deployment](#-configuração-pós-deployment)
- [Monitoramento e Logs](#-monitoramento-e-logs)
- [Segurança](#-segurança)
- [Performance e Otimização](#-performance-e-otimização)
- [CI/CD Pipeline](#-cicd-pipeline)
- [Troubleshooting](#-troubleshooting)
- [Limpeza de Recursos](#-limpeza-de-recursos)

---

## 🌟 Visão Geral

O CODEX é deployado na **Microsoft Azure** usando uma arquitetura serverless e otimizada para custos:

### Características do Deployment

- ✅ **Backend:** Container Apps (serverless, auto-scaling)
- ✅ **Frontend:** App Service Free Tier (estático + SSR)
- ✅ **Database:** PostgreSQL Neon (externo)
- ✅ **Container Registry:** Azure Container Registry (armazenar imagens)
- ✅ **Secrets:** Azure Key Vault (gerenciar credenciais)
- ✅ **Monitoramento:** Application Insights + Log Analytics
- ✅ **CDN:** Azure Front Door (opcional, para performance)
- ✅ **Networking:** VNet e NSG para segurança

### Modelo de Custos

- **Desenvolvimento:** ~R$0-10/mês (Free Tiers)
- **Produção:** ~R$50-150/mês (escalável)

---

## 🏛️ Arquitetura Azure

```
┌─────────────────────────────────────────────────────────────────┐
│                         AZURE CLOUD                              │
├─────────────────────────────────────────────────────────────────┤
│                                                                   │
│  ┌──────────────┐                                                │
│  │   Frontend   │                                                │
│  │ App Service  │  (Free F1 - Estático + SSR)                  │
│  │ codex-web    │  https://codex-web.azurewebsites.net         │
│  └──────┬───────┘                                                │
│         │                                                         │
│         │ HTTPS (TLS 1.2+)                                       │
│         │                                                         │
│  ┌──────▼──────────────────────────────────────────────┐        │
│  │  Application Gateway / Front Door (CDN)             │        │
│  │  ✓ Load Balancing                                   │        │
│  │  ✓ WAF (Web Application Firewall)                  │        │
│  │  ✓ SSL/TLS Termination                             │        │
│  └──────┬──────────────────────────────────────────────┘        │
│         │                                                         │
│         │ Internal VNet                                          │
│         │                                                         │
│  ┌──────▼──────────────────────────────────────────────┐        │
│  │  Container Apps (Backend)                           │        │
│  │  codex-api (Docker Container)                       │        │
│  │  ✓ Auto-scaling (0-2 replicas)                      │        │
│  │  ✓ Health checks                                    │        │
│  │  ✓ Internal ingress                                 │        │
│  └──────┬──────────────────────────────────────────────┘        │
│         │                                                         │
│         │ Connection String                                      │
│         │                                                         │
│  ┌──────▼──────────────────────────────────────────────┐        │
│  │  Azure Key Vault                                    │        │
│  │  ✓ Armazenar secrets                                │        │
│  │  ✓ JWT Secret                                       │        │
│  │  ✓ Database credentials                             │        │
│  │  ✓ API Keys                                         │        │
│  └──────────────────────────────────────────────────────┘        │
│                                                                   │
│  ┌──────────────────────────────────────────────────────┐        │
│  │  Logging & Monitoring                               │        │
│  │  ┌─────────────────────────────────────────────┐    │        │
│  │  │ Application Insights (Analytics)           │    │        │
│  │  │ ✓ Request tracking                         │    │        │
│  │  │ ✓ Performance metrics                      │    │        │
│  │  │ ✓ Exception logging                        │    │        │
│  │  └─────────────────────────────────────────────┘    │        │
│  │  ┌─────────────────────────────────────────────┐    │        │
│  │  │ Log Analytics                               │    │        │
│  │  │ ✓ Structured logs (Serilog)                │    │        │
│  │  │ ✓ Custom queries (KQL)                     │    │        │
│  │  │ ✓ Alertas automáticos                      │    │        │
│  │  └─────────────────────────────────────────────┘    │        │
│  └──────────────────────────────────────────────────────┘        │
│                                                                   │
│  ┌──────────────────────────────────────────────────────┐        │
│  │  Container Registry (ACR)                           │        │
│  │  codexacr.azurecr.io                                │        │
│  │  ✓ Armazenar imagens Docker                         │        │
│  │  ✓ Replicação geo                                   │        │
│  │  ✓ Vulnerabilidade scanning                         │        │
│  └──────────────────────────────────────────────────────┘        │
│                                                                   │
└─────────────────────────────────────────────────────────────────┘

┌──────────────────────────────────────────┐
│         EXTERNAL SERVICES                 │
├──────────────────────────────────────────┤
│  ┌────────────────────────────────────┐  │
│  │ Neon PostgreSQL (Externo)          │  │
│  │ postgres://host/codex_db           │  │
│  │ ✓ Backup automático                │  │
│  │ ✓ High Availability                │  │
│  └────────────────────────────────────┘  │
│  ┌────────────────────────────────────┐  │
│  │ Cloudinary (Storage)               │  │
│  │ ✓ Upload de mídias                 │  │
│  │ ✓ Transformação de imagens         │  │
│  └────────────────────────────────────┘  │
│  ┌────────────────────────────────────┐  │
│  │ SMTP (Gmail/Outlook)               │  │
│  │ ✓ Envio de e-mails                 │  │
│  └────────────────────────────────────┘  │
└──────────────────────────────────────────┘
```

---

## ☁️ Serviços Utilizados

### 1. Container Apps (Backend)

**Função:** Hospedagem do backend .NET 9.0

| Propriedade | Valor |
|-----------|-------|
| **Nome** | `codex-api` |
| **Imagem** | `codexacr.azurecr.io/codex-api:latest` |
| **CPU** | 0.25 - 0.5 vCPU |
| **Memória** | 512 MB - 1 GB |
| **Replicas** | 1-2 (auto-scaling) |
| **HTTPS** | ✅ Automático |
| **URL** | `https://codex-api.brazilsouth.azurecontainerapps.io` |

**Recursos Inclusos:**
- Auto-scaling baseado em CPU
- Health checks automáticos
- Logs estruturados (Serilog → Log Analytics)
- Secrets via Key Vault
- VNet integrada

### 2. App Service (Frontend)

**Função:** Hospedagem do frontend Angular + SSR

| Propriedade | Valor |
|-----------|-------|
| **Nome** | `codex-web` |
| **Tier** | Free F1 (Compartilhado) |
| **Runtime** | Node.js 20.x |
| **HTTPS** | ✅ Automático (*.azurewebsites.net) |
| **URL** | `https://codex-web.azurewebsites.net` |
| **Limite** | 60 min/dia de tempo de execução |

**Recursos Inclusos:**
- SSL/TLS automático
- Git integration (CI/CD)
- Environment variables
- Application Insights

### 3. Container Registry (ACR)

**Função:** Armazenar imagens Docker do backend

| Propriedade | Valor |
|-----------|-------|
| **Nome** | `codexacr` |
| **Tier** | Basic (Compartilhado) |
| **URL** | `codexacr.azurecr.io` |
| **Quota** | 10 GB storage |
| **Replicações** | Brasil South (única) |

**Funcionalidades:**
- Pull/push de imagens
- Webhook para deploy automático
- Scan de vulnerabilidades
- Retention policy (7 dias)

### 4. Key Vault

**Função:** Armazenar e gerenciar secrets

| Propriedade | Valor |
|-----------|-------|
| **Nome** | `codex-kv` |
| **Tier** | Standard |
| **Access Policy** | RBAC (Managed Identity) |

**Secrets Armazenados:**
- `jwt-secret` - Chave JWT
- `database-connection-string` - Neon PostgreSQL
- `cloudinary-cloud-name` - Cloudinary config
- `cloudinary-api-key` - Cloudinary API
- `cloudinary-api-secret` - Cloudinary secret
- `mail-password` - Senha SMTP

### 5. Application Insights

**Função:** Monitoramento de performance e diagnóstico

| Propriedade | Valor |
|-----------|-------|
| **Tipo** | Application Insights (workspace-based) |
| **Retenção** | 30 dias (grátis) |
| **Amostragem** | 100% (development) / 5% (production) |

**Métricas Coletadas:**
- Request duration
- Exception rates
- Dependency calls
- Custom events
- Page views (frontend)

### 6. Log Analytics Workspace

**Função:** Armazenar logs estruturados

| Propriedade | Valor |
|-----------|-------|
| **Nome** | `codex-logs` |
| **Tier** | Pay-as-you-go |
| **Retenção** | 30 dias (grátis) / 90 dias (paid) |

**Logs Coletados:**
- Container Apps logs (Serilog)
- App Service logs
- Platform diagnostics
- Custom queries (KQL)

### 7. Virtual Network (VNet)

**Função:** Isolar e proteger recursos

| Propriedade | Valor |
|-----------|-------|
| **CIDR** | `10.0.0.0/16` |
| **Subnets** | 2 (apps + database) |
| **NSG Rules** | Apenas HTTPS (443) inbound |

### 8. Azure Front Door (Opcional - CDN)

**Função:** Cache global e proteção DDoS

| Propriedade | Valor |
|-----------|-------|
| **Tier** | Standard (para começar) |
| **WAF** | Detection mode |
| **Endpoints** | Frontend + Backend |

---

## 💰 Custos Detalhados

### Simulação Mensal (Brasil South)

#### Desenvolvimento (Free Tier)

| Serviço | SKU | Quantidade | Custo/Mês |
|---------|-----|-----------|-----------|
| Container Apps | Free | 1 app | **Grátis** |
| App Service | F1 Free | 1 site | **Grátis** |
| Key Vault | Standard | 10 secrets | ~R$3 |
| Container Registry | Basic | 10 GB | ~R$8 |
| Application Insights | Free tier | 1 instância | **Grátis** (30 dias) |
| Log Analytics | Pay-as-you-go | <1 GB | **Grátis** (1 mês) |
| Virtual Network | Standard | 1 VNet | **Grátis** |
| **TOTAL** | | | **R$11-15/mês** |

#### Produção (Escalado)

| Serviço | SKU | Quantidade | Custo/Mês |
|---------|-----|-----------|-----------|
| Container Apps | Standard (2 replicas) | 2 apps x 2 vCPU | ~R$80 |
| App Service | B2 Basic | 1 site | ~R$50 |
| Key Vault | Standard | 20 secrets | ~R$3 |
| Container Registry | Premium | 250 GB | ~R$30 |
| Application Insights | Standard | 1 GB/dia | ~R$15 |
| Log Analytics | Premium | 5 GB/dia | ~R$150 |
| Virtual Network | Standard + NSG | 3 VNets | ~R$20 |
| Front Door | Premium + WAF | 1 endpoint | ~R$60 |
| **TOTAL** | | | **R$408/mês** |

### Dicas de Economia

1. **Use Free Tiers para desenvolvimento:**
   - Container Apps: até 2 vCPU + 4 GB grátis
   - App Service: F1 para prototipagem
   - Application Insights: grátis primeiro mês

2. **Optimize App Service:**
   - Não use Always On (economia ~30%)
   - Scale down em horários baixos
   - Use shared tier para início

3. **Configure retenção de logs:**
   - Application Insights: 7 dias (dev) / 30 dias (prod)
   - Log Analytics: 7 dias (dev) / 90 dias (prod)
   - Isso reduz custos em 60-80%

4. **Use Neon externo:**
   - Não coloque database no Azure (muito caro)
   - Neon PostgreSQL: R$5-50/mês
   - Azure PostgreSQL: R$150+/mês

5. **CDN condicional:**
   - Ative Azure Front Door apenas em produção
   - Use DNS simples para staging

6. **Monitoramento seletivo:**
   - Amostragem de eventos em produção (5% dos requests)
   - Alertas automáticos apenas para erros críticos

---

## 📌 Pré-requisitos

### Conta e Acessos

- ✅ **Conta Microsoft Azure** - [azure.microsoft.com](https://azure.microsoft.com)
  - Free account: $200 crédito + 12 meses free
- ✅ **Acesso à Subscription** - com permissão de criar recursos
- ✅ **Conta Neon** - Database PostgreSQL
- ✅ **Conta Cloudinary** - Storage de mídias
- ✅ **Conta SMTP** - Gmail/Outlook para e-mails

### Software Obrigatório

```bash
# 1. .NET 9.0 SDK
dotnet --version  # Deve retornar 9.0.x ou superior

# 2. Node.js 20+
node --version
npm --version

# 3. Docker Desktop
docker --version

# 4. Azure CLI
az --version  # Se não tiver, instale

# 5. Git
git --version
```

### Instalação do Azure CLI

**Windows (PowerShell):**
```powershell
# Com Chocolatey
choco install azure-cli

# Ou download direto
Invoke-WebRequest -Uri https://aka.ms/installazurecliwindows -OutFile AzureCLI.msi
```

**Linux:**
```bash
curl -sL https://aka.ms/InstallAzureCLIDeb | sudo bash
```

**macOS:**
```bash
brew install azure-cli
```

### Verificar Instalação

```bash
# Versão do Azure CLI
az --version

# Login no Azure
az login

# Listar subscriptions
az account list --output table
```

---

## 🔧 Setup Inicial

### 1. Clonar o Repositório

```bash
git clone https://github.com/seu-usuario/codex.git
cd codex
```

### 2. Criar Arquivo .env.azure

Na raiz do projeto, crie `.env.azure`:

```env
# Azure Configuration
AZURE_SUBSCRIPTION_ID=sua_subscription_id
AZURE_RESOURCE_GROUP=codex-rg
AZURE_LOCATION=brazilsouth
AZURE_ENVIRONMENT=Production

# Container Registry
AZURE_REGISTRY_NAME=codexacr
AZURE_REGISTRY_URL=codexacr.azurecr.io

# Services
BACKEND_NAME=codex-api
FRONTEND_NAME=codex-web
KEYVAULT_NAME=codex-kv

# Database (Neon)
DATABASE_URL=postgres://user:password@host/database?sslmode=require

# JWT & Security
JWT_SECRET=seu_secreto_minimo_32_caracteres
CORS_ALLOWED_ORIGINS=https://codex-web.azurewebsites.net

# Cloudinary
CLOUDINARY_CLOUD_NAME=seu_cloud_name
CLOUDINARY_API_KEY=sua_api_key
CLOUDINARY_API_SECRET=seu_api_secret

# Email
MAIL_HOST=smtp.gmail.com
MAIL_USERNAME=seu@gmail.com
MAIL_PASSWORD=sua_app_password
```

### 3. Fazer Login no Azure

```bash
az login
```

Será aberto um browser para autenticação. Após isso:

```bash
# Verificar subscription ativa
az account show

# Se tiver múltiplas, selecionar
az account set --subscription "seu_subscription_id"
```

### 4. Criar Resource Group

```bash
az group create \
  --name codex-rg \
  --location brazilsouth
```

### 5. Criar Container Registry

```bash
az acr create \
  --resource-group codex-rg \
  --name codexacr \
  --sku Basic
```

### 6. Criar Key Vault

```bash
az keyvault create \
  --name codex-kv \
  --resource-group codex-rg \
  --location brazilsouth
```

### 7. Popular Secrets no Key Vault

```bash
# JWT Secret
az keyvault secret set \
  --vault-name codex-kv \
  --name jwt-secret \
  --value "seu_secreto_minimo_32_caracteres"

# Database Connection String
az keyvault secret set \
  --vault-name codex-kv \
  --name database-connection-string \
  --value "postgres://user:password@host/db?sslmode=require"

# Cloudinary
az keyvault secret set \
  --vault-name codex-kv \
  --name cloudinary-cloud-name \
  --value "seu_cloud_name"

# ... (repetir para outras credenciais)
```

---

## 🚀 Guia de Deployment

### Opção 1: Deploy Automatizado (Recomendado)

**Windows:**
```bash
cd .azure
deploy.bat
```

**Linux/macOS:**
```bash
chmod +x .azure/deploy.sh
./.azure/deploy.sh
```

O script fará:
1. ✅ Validar pré-requisitos
2. ✅ Build da imagem Docker (backend)
3. ✅ Push para Azure Container Registry
4. ✅ Provision de infraestrutura (Bicep)
5. ✅ Build do frontend Angular
6. ✅ Deploy no App Service
7. ✅ Configurar secrets no Key Vault

**Tempo estimado:** 15-20 minutos

### Opção 2: Deploy Manual (Passo a Passo)

#### Passo 1: Build e Push da Imagem Docker

```bash
# Build da imagem
docker build -f backend/Dockerfile \
  -t codexacr.azurecr.io/codex-api:latest \
  backend/

# Login no ACR
az acr login --name codexacr

# Push para Azure
docker push codexacr.azurecr.io/codex-api:latest
```

#### Passo 2: Deploy do Backend (Container Apps)

```bash
# Usar template Bicep
az deployment group create \
  --resource-group codex-rg \
  --template-file .azure/main.bicep \
  --parameters \
    location=brazilsouth \
    containerRegistryUrl=codexacr.azurecr.io \
    imageName=codex-api:latest
```

#### Passo 3: Build do Frontend

```bash
cd frontend

# Build de produção
npm run build

# Output em dist/codex/browser/
```

#### Passo 4: Deploy do Frontend

```bash
# Deploy para App Service via ZIP
cd dist/codex/browser

# Criar ZIP
zip -r codex-web.zip .

# Deploy
az webapp deployment source config-zip \
  --resource-group codex-rg \
  --name codex-web \
  --src-path codex-web.zip
```

---

## ⚙️ Configuração Pós-Deployment

### 1. Configurar Custom Domain (Opcional)

```bash
# Adicionar domínio custom ao App Service
az webapp config hostname add \
  --webapp-name codex-web \
  --resource-group codex-rg \
  --hostname seu-dominio.com.br
```

### 2. Configurar SSL/TLS

```bash
# Gerar certificado free (Let's Encrypt)
az webapp config ssl create \
  --name codex-web \
  --resource-group codex-rg \
  --certificate-name codex-cert
```

### 3. Configurar Environment Variables

```bash
# Backend (Container Apps)
az containerapp env variable set \
  --name codex-api \
  --resource-group codex-rg \
  --environment-name codex-env \
  --variables \
    ASPNETCORE_ENVIRONMENT=Production \
    CORS_ALLOWED_ORIGINS=https://seu-dominio.com

# Frontend (App Service)
az webapp config appsettings set \
  --name codex-web \
  --resource-group codex-rg \
  --settings \
    NG_APP_API_BASE_URL=https://codex-api.brazilsouth.azurecontainerapps.io/api \
    NODE_ENV=production
```

### 4. Configurar Backup (Database)

Neon já faz backup automático. Verificar:
```bash
# Dashboard Neon → Backups
https://console.neon.tech/app/projects
```

### 5. Configurar CDN (Front Door)

```bash
az afd profile create \
  --profile-name codex-cdn \
  --resource-group codex-rg \
  --sku Standard_AzureFrontDoor

# Adicionar endpoints
az afd endpoint create \
  --profile-name codex-cdn \
  --resource-group codex-rg \
  --endpoint-name codex-web-endpoint \
  --enabled true
```

---

## 📊 Monitoramento e Logs

### 1. Application Insights - Analytics

```bash
# Ver requisições últimas 1 hora
az monitor app-insights metrics show \
  --app codex-insights \
  --resource-group codex-rg \
  --metric requests/count \
  --interval PT1H

# Ver exceptions
az monitor app-insights query \
  --app codex-insights \
  --resource-group codex-rg \
  --analytics-query "exceptions | summarize count() by type"
```

### 2. Log Analytics - Consultas KQL

**Via Portal:**
1. Azure Portal → Log Analytics Workspace → Logs
2. Execute queries KQL:

```kusto
// Errors últimas 24h
ContainerAppConsoleLogs_CL
| where LogLevel_s == "Error"
| summarize count() by Exception_s
| sort by count_ desc

// P95 latency
ContainerAppConsoleLogs_CL
| where isnotempty(DurationMs_d)
| summarize percentile(DurationMs_d, 95) by bin(TimeGenerated, 1h)

// Requests por endpoint
ContainerAppConsoleLogs_CL
| where LogLevel_s == "Information"
| summarize count() by Path_s
```

### 3. Container Apps - Logs em Tempo Real

```bash
# Ver logs do backend
az containerapp logs show \
  --name codex-api \
  --resource-group codex-rg \
  --follow  # Tempo real

# Filtrar por level
az containerapp logs show \
  --name codex-api \
  --resource-group codex-rg \
  --container codex-api \
  --format json
```

### 4. App Service - Logs

```bash
# Ativar logging
az webapp log config \
  --name codex-web \
  --resource-group codex-rg \
  --application-logging filesystem

# Ver logs
az webapp log tail \
  --name codex-web \
  --resource-group codex-rg
```

### 5. Criar Alertas

```bash
# Alerta: Taxa de erro > 5%
az monitor metrics alert create \
  --name error-rate-alert \
  --resource-group codex-rg \
  --scopes /subscriptions/.../codex-api \
  --condition "avg Failed > 5" \
  --description "Alert when error rate exceeds 5%"

# Alerta: CPU > 80%
az monitor metrics alert create \
  --name cpu-alert \
  --resource-group codex-rg \
  --scopes /subscriptions/.../codex-api \
  --condition "avg Percentage CPU > 80" \
  --window-size 5m
```

### 6. Dashboard Personalizado

```bash
# Criar dashboard via portal
az portal dashboard create \
  --name codex-dashboard \
  --resource-group codex-rg \
  --input-path dashboard-config.json
```

---

## 🔐 Segurança

### 1. Managed Identity (RBAC)

```bash
# Habilitar Managed Identity no Container Apps
az containerapp identity assign \
  --name codex-api \
  --resource-group codex-rg \
  --system-assigned

# Dar permissão de ler Key Vault
az keyvault set-policy \
  --name codex-kv \
  --object-id <managed-identity-principal-id> \
  --secret-permissions get list
```

### 2. Network Security Group (NSG)

```bash
# Criar NSG
az network nsg create \
  --name codex-nsg \
  --resource-group codex-rg

# Permitir apenas HTTPS (443)
az network nsg rule create \
  --nsg-name codex-nsg \
  --resource-group codex-rg \
  --name AllowHTTPS \
  --priority 100 \
  --source-address-prefixes Internet \
  --destination-port-ranges 443 \
  --access Allow \
  --protocol Tcp
```

### 3. SSL/TLS

```bash
# Verificar certificado
az webapp config ssl list \
  --resource-group codex-rg \
  --certificates

# Forçar HTTPS
az webapp update \
  --name codex-web \
  --resource-group codex-rg \
  --https-only true
```

### 4. WAF (Web Application Firewall)

```bash
# Criar WAF policy
az network front-door waf-policy create \
  --name codex-waf \
  --resource-group codex-rg

# Modo Detection
az network front-door waf-policy update \
  --name codex-waf \
  --resource-group codex-rg \
  --mode Detection
```

### 5. DDoS Protection

```bash
# Habilitar DDoS Standard (pago)
az network ddos-protection create \
  --name codex-ddos \
  --resource-group codex-rg
```

### 6. Secrets Management

```bash
# Nunca colocar secrets em appsettings.json
# Usar Key Vault sempre

# Referência no código:
# connectionString = await keyVaultClient.GetSecretAsync("database-connection-string");
```

---

## ⚡ Performance e Otimização

### 1. Auto-scaling do Backend

```bash
# Configurar auto-scale (0-2 replicas)
az containerapp create \
  --name codex-api \
  --resource-group codex-rg \
  --min-replicas 1 \
  --max-replicas 2 \
  --scale-rule-name cpu-scale \
  --scale-rule-type cpu \
  --scale-rule-comparison "GreaterThan" \
  --scale-rule-threshold 70
```

### 2. Caching (Azure Cache for Redis)

```bash
# Criar Redis (opcional, para escalas maiores)
az redis create \
  --name codex-cache \
  --resource-group codex-rg \
  --sku basic \
  --vm-size c0

# Configurar no backend
// Program.cs
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "codex-cache.redis.cache.windows.net:6380,ssl=true";
});
```

### 3. CDN para Assets Estáticos

```bash
# Armazenar JS/CSS/images em Storage Account
az storage account create \
  --name codexstg \
  --resource-group codex-rg \
  --sku Standard_LRS

# Habilitar CDN
az cdn endpoint create \
  --name codex-cdn \
  --profile-name codex-profile \
  --resource-group codex-rg \
  --origin codexstg.blob.core.windows.net
```

### 4. Compression (gzip)

```bash
# Enabled automaticamente em App Service
# Verificar:
az webapp config set \
  --name codex-web \
  --resource-group codex-rg \
  --web-sockets-enabled true \
  --always-on true
```

### 5. Database Connection Pooling

```csharp
// Program.cs
builder.Services.AddDbContext<CodexDbContext>(options =>
    options.UseNpgsql(connectionString, npgsqlOptions =>
    {
        npgsqlOptions.CommandTimeout(60);
        npgsqlOptions.EnableRetryOnFailure(
            maxRetryCount: 3,
            maxRetialDelay: TimeSpan.FromSeconds(10),
            errorCodesToAdd: null);
    }));
```

---

## 🔄 CI/CD Pipeline

### GitHub Actions - Automatic Deploy

Criar `.github/workflows/deploy-azure.yml`:

```yaml
name: Deploy to Azure

on:
  push:
    branches: [main, develop]
  pull_request:
    branches: [main]

jobs:
  build-and-deploy:
    runs-on: ubuntu-latest
    
    steps:
      - uses: actions/checkout@v3
      
      - name: Azure Login
        uses: azure/login@v1
        with:
          creds: ${{ secrets.AZURE_CREDENTIALS }}
      
      - name: Build Backend Docker Image
        run: |
          docker build -f backend/Dockerfile \
            -t codexacr.azurecr.io/codex-api:${{ github.sha }} \
            backend/
      
      - name: Push to ACR
        run: |
          az acr login --name codexacr
          docker push codexacr.azurecr.io/codex-api:${{ github.sha }}
      
      - name: Deploy Container App
        run: |
          az containerapp update \
            --name codex-api \
            --resource-group codex-rg \
            --image codexacr.azurecr.io/codex-api:${{ github.sha }}
      
      - name: Build Frontend
        run: |
          cd frontend
          npm install
          npm run build
      
      - name: Deploy Frontend
        run: |
          az webapp deployment source config-zip \
            --resource-group codex-rg \
            --name codex-web \
            --src-path frontend/dist.zip
```

---

## 🆘 Troubleshooting

### Erro: "Container image not found"

```bash
# Verificar se imagem existe no ACR
az acr repository show \
  --name codexacr \
  --repository codex-api

# Push novamente
docker push codexacr.azurecr.io/codex-api:latest
```

### Erro: "Container App failed to start"

```bash
# Ver logs
az containerapp logs show \
  --name codex-api \
  --resource-group codex-rg

# Verificar secrets
az keyvault secret show \
  --vault-name codex-kv \
  --name jwt-secret
```

### Erro: "Cannot read Key Vault secrets"

```bash
# Verificar Managed Identity
az containerapp identity show \
  --name codex-api \
  --resource-group codex-rg

# Dar permissões
az keyvault set-policy \
  --name codex-kv \
  --object-id <principal-id> \
  --secret-permissions get list
```

### Erro: "App Service deployment failed"

```bash
# Ver logs de deployment
az webapp deployment log show \
  --name codex-web \
  --resource-group codex-rg

# Reiniciar app
az webapp restart \
  --name codex-web \
  --resource-group codex-rg
```

### Erro: "Database connection timeout"

```bash
# Verificar string de conexão
az keyvault secret show \
  --vault-name codex-kv \
  --name database-connection-string

# Testar via psql
psql "postgres://user:pass@host/db?sslmode=require"
```

### Erro: "Free tier App Service already exists"

```bash
# Azure permite apenas 1 App Service Free
# Opção 1: Deletar o antigo
az webapp delete \
  --name codex-web-old \
  --resource-group codex-rg

# Opção 2: Upgrade para B1 Basic (~R$30/mês)
az appservice plan create \
  --name codex-plan-b1 \
  --resource-group codex-rg \
  --sku B1
```

---

## 🧹 Limpeza de Recursos

### Deletar Tudo (Cuidado!)

```bash
# Deletar resource group (remove TODOS os recursos)
az group delete \
  --name codex-rg \
  --yes \
  --no-wait
```

### Deletar Recursos Específicos

```bash
# Deletar apenas Container App
az containerapp delete \
  --name codex-api \
  --resource-group codex-rg

# Deletar apenas App Service
az webapp delete \
  --name codex-web \
  --resource-group codex-rg

# Deletar Key Vault
az keyvault delete \
  --name codex-kv \
  --resource-group codex-rg

# Deletar Container Registry
az acr delete \
  --name codexacr \
  --resource-group codex-rg
```

### Parar Recursos (Economizar)

```bash
# Parar App Service (quando não em uso)
az webapp stop \
  --name codex-web \
  --resource-group codex-rg

# Iniciar novamente
az webapp start \
  --name codex-web \
  --resource-group codex-rg

# Scale down Container App
az containerapp update \
  --name codex-api \
  --resource-group codex-rg \
  --min-replicas 0
```

---

## 📈 Monitoramento de Custos

### 1. Portal Azure - Cost Management

1. Azure Portal → **Cost Management + Billing**
2. Selecionar **Resource Group: codex-rg**
3. Ver custos por serviço

### 2. Azure CLI - Estimativa

```bash
# Listar recursos e SKUs
az resource list \
  --resource-group codex-rg \
  --query "[*].[name,type,sku.name]" \
  --output table
```

### 3. Alertas de Custo

```bash
# Criar alerta quando custo mensal > R$100
az costmanagement alert create \
  --alert-type BudgetExpired \
  --budget-name codex-budget \
  --threshold 100 \
  --threshold-type Forecasted
```

### 4. Reduzir Custos

**Ações recomendadas:**
- ✅ Reducir retenção de logs (7-14 dias)
- ✅ Desabilitar Application Insights em dev
- ✅ Auto-scaling: min replicas = 0
- ✅ Usar App Service Free (limitado)
- ✅ Database externo (Neon é mais barato)
- ✅ CDN apenas em produção

---

## 📚 Recursos Adicionais

- [Azure Container Apps Docs](https://learn.microsoft.com/azure/container-apps/)
- [App Service Best Practices](https://learn.microsoft.com/azure/app-service/best-practices)
- [Azure Key Vault](https://learn.microsoft.com/azure/key-vault/)
- [Bicep Language](https://learn.microsoft.com/azure/azure-resource-manager/bicep/)
- [Neon PostgreSQL](https://neon.tech/docs/introduction)
- [Azure CLI Reference](https://learn.microsoft.com/cli/azure/reference-index)

---

## ✅ Checklist Pré-Deploy

- [ ] Conta Azure criada e ativa
- [ ] Subscription selecionada
- [ ] Azure CLI instalado e testado
- [ ] Docker Desktop instalado e rodando
- [ ] .NET 9.0 SDK instalado
- [ ] Node.js 20+ instalado
- [ ] Repository clonado localmente
- [ ] Arquivo `.env.azure` configurado
- [ ] Secrets do Key Vault preparados
- [ ] Neon PostgreSQL funcionando
- [ ] Cloudinary configurado
- [ ] SMTP credentials preparadas
- [ ] Scripts de deploy executáveis
- [ ] Preparado para esperar 15-20 minutos

---

<div align="center">
  <p>Pronto para deploy? Execute o script! 🚀</p>
  <p><strong>Organize, compartilhe e evolua seu conhecimento técnico no Azure.</strong></p>
  <sub>© 2026 CODEX Azure Deployment. Todos os direitos reservados.</sub>
</div>

