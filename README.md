<div align="center">

# 📚 CODEX

**A Biblioteca Definitiva de Conteúdo de Programação**

CODEX é uma plataforma centralizada para organizar, compartilhar e acessar conteúdo educacional de programação. Transforma conhecimento técnico disperso em uma biblioteca estruturada e intuitiva, permitindo que desenvolvedores compartilhem tópicos, conteúdos, pesquisem recursos e colaborem de forma prática e eficiente.

[![Backend](https://img.shields.io/badge/Backend-.NET%209.0-512BD4?style=for-the-badge&logo=dotnet)](https://dotnet.microsoft.com)
[![Frontend](https://img.shields.io/badge/Frontend-Angular%2019-DD0031?style=for-the-badge&logo=angular)](https://angular.io)
[![Deploy Backend](https://img.shields.io/badge/Deploy-Render-46E3B7?style=for-the-badge&logo=render)](https://render.com)
[![Deploy Frontend](https://img.shields.io/badge/Deploy-Vercel-000000?style=for-the-badge&logo=vercel)](https://vercel.com)

</div>

---

## 📋 Índice

- [Sobre o Projeto](#-sobre-o-projeto)
- [Funcionalidades](#-funcionalidades)
- [Arquitetura](#-arquitetura)
- [Tecnologias](#-tecnologias)
- [Estrutura do Repositório](#-estrutura-do-repositório)
- [Pré-requisitos](#-pré-requisitos)
- [Instalação e Configuração](#-instalação-e-configuração)
- [Executando o Projeto](#-executando-o-projeto)
- [API Endpoints](#-api-endpoints)
- [Variáveis de Ambiente](#-variáveis-de-ambiente)
- [Deploy](#-deploy)
- [Modelos de Dados](#-modelos-de-dados)
- [Créditos](#-créditos)

---

## 🌟 Sobre o Projeto

O **CODEX** revoluciona a forma como desenvolvedores organizam e compartilham conhecimento técnico. É uma plataforma colaborativa que centraliza conteúdo de programação em um único lugar.

A plataforma permite que usuários:

- 📖 Publicar e compartilhar tópicos e conteúdos de programação
- 🔍 Buscar e descobrir recursos por tags, categorias e palavras-chave
- ⬆️ Fazer upload de materiais educacionais (PDFs, imagens, documentos)
- 👥 Colaborar em comunidades de aprendizado
- 🎯 Organizar conteúdo por tópicos estruturados
- 💾 Guardar e acessar recursos em uma biblioteca pessoal

Ideal para **desenvolvedores**, **educadores**, **equipes técnicas** e **organizações** que desejam manter um repositório centralizado de conhecimento programático.

---

## ✨ Funcionalidades

### Para Administradores
- ✅ Gerenciar usuários e suas permissões
- ✅ Moderação de conteúdo postado
- ✅ Criar e gerenciar categorias de tópicos
- ✅ Visualizar estatísticas e relatórios da plataforma
- ✅ Configurar parâmetros globais do sistema

### Para Criadores de Conteúdo
- ✅ Registrar-se e fazer login com autenticação JWT
- ✅ Criar e publicar tópicos de programação
- ✅ Fazer upload de mídias (imagens, documentos, vídeos)
- ✅ Organizar conteúdo com tags e descrições detalhadas
- ✅ Editar e atualizar conteúdo publicado
- ✅ Redefinir senha via e-mail

### Para Usuários Gerais
- ✅ Buscar conteúdo por múltiplos critérios (tags, categorias, título)
- ✅ Visualizar detalhes de tópicos e conteúdos
- ✅ Acessar uma galeria organizada de recursos
- ✅ Fazer download de materiais educacionais
- ✅ Consultar documentação através de busca avançada

### Geral
- ✅ Autenticação stateless com JWT (validade de 24h)
- ✅ Upload de mídias seguro (Cloudinary)
- ✅ Envio de e-mails transacionais
- ✅ Documentação automática da API via Swagger/OpenAPI
- ✅ Suporte a múltiplos tipos de conteúdo
- ✅ Sistema de busca inteligente e filtros avançados

---

## 🏛️ Arquitetura

O projeto é uma aplicação **full stack** dividida em dois serviços independentes:

```
CODEX/
├── backend/   → API REST em ASP.NET Core 9.0 (C#)
└── frontend/  → SPA em Angular 19 com SSR
```

### Backend — Clean Architecture

```
Controllers  →  Services  →  Repositories  →  DbContext (EF Core)
                    ↓
              Models / DTOs / Exceptions
```

- **Controllers**: Recebem as requisições HTTP e delegam para os serviços
- **Services**: Contêm a lógica de negócio isolada e testável
- **Repositories**: Abstração da camada de dados com padrão Repository
- **Models**: Entidades de domínio (Usuario, Conteudo, Topico, Upload)
- **DTOs**: Objetos de transferência de dados para requisições e respostas
- **Exceptions**: Exceções customizadas (`BusinessException`, `ResourceNotFoundException`, `UnauthorizedException`)
- **Middleware**: `SecurityHeadersMiddleware` para tratamento de segurança

### Banco de Dados por Ambiente

| Ambiente | Banco de Dados |
|----------|----------------|
| Development | SQLite (arquivo `codex.db`) |
| Homolog | PostgreSQL (Neon) |
| Production | PostgreSQL |

---

## 💻 Tecnologias

### Backend

| Categoria | Tecnologia | Versão |
|-----------|-----------|--------|
| Framework | ASP.NET Core | 9.0 |
| Linguagem | C# | 13 |
| ORM | Entity Framework Core | 9.0 |
| Banco (dev) | SQLite | — |
| Banco (prod) | PostgreSQL | 15+ |
| Autenticação | JWT Bearer | 9.0 |
| Hash de Senha | BCrypt.Net-Next | 4.0.3 |
| Mapeamento | AutoMapper | 12.0.1 |
| Validação | FluentValidation | 11.9.2 |
| Logging | Serilog | 8.0.1 |
| Storage | Cloudinary SDK | 1.28.0 |
| E-mail | MailKit | 4.9.0 |
| Documentação | Swashbuckle (Swagger) | 6.5.0 |
| Containerização | Docker | — |

### Frontend

| Categoria | Tecnologia | Versão |
|-----------|-----------|--------|
| Framework | Angular | 19.1.0 |
| Linguagem | TypeScript | 5.7 |
| SSR | Angular SSR | 19.1.5 |
| Reatividade | RxJS | 7.8.0 |
| Build | Angular CLI | 19.1.5 |

### DevOps & Infraestrutura

| Serviço | Finalidade |
|---------|-----------|
| Render | Deploy do backend (Docker) |
| Vercel | Deploy do frontend |
| Cloudinary | Armazenamento de imagens, documentos e vídeos |
| PostgreSQL (Neon) | Banco de dados em produção/homolog |
| GitHub | Controle de versão e CI/CD |

---

## 📁 Estrutura do Repositório

```
CODEX/
├── README.md                          # Este arquivo
│
├── backend/                           # API REST ASP.NET Core 9.0
│   ├── CodexAPI.csproj                # Arquivo de projeto .NET
│   ├── Program.cs                     # Ponto de entrada e configuração do app
│   ├── appsettings.json               # Configurações (desenvolvimento)
│   ├── appsettings.Homolog.json       # Configurações de homologação
│   ├── appsettings.Production.json    # Configurações de produção
│   ├── Dockerfile                     # Imagem Docker do backend
│   ├── docker-compose.yml             # Orquestração local com PostgreSQL
│   ├── global.json                    # Versão do SDK .NET
│   ├── nuget.config                   # Configuração de feeds NuGet
│   ├── Controllers/                   # AuthController, ContentController,
│   │                                  #   SearchController, TopicosController,
│   │                                  #   UploadsController
│   ├── Services/                      # AuthService e demais serviços
│   ├── Models/                        # DomainModels (entidades de domínio)
│   ├── DTOs/                          # DtoModels (objetos de transferência)
│   ├── Data/                          # DbContext e Repositories
│   │   ├── CodexDbContext.cs          # DbContext do EF Core
│   │   └── Repositories/              # Implementações do padrão Repository
│   ├── Middleware/                    # SecurityHeadersMiddleware
│   ├── Migrations/                    # Migrações do Entity Framework Core
│   ├── Properties/                    # launchSettings.json
│   ├── scripts/                       # Scripts de setup (setup.sh, setup.bat)
│   ├── wwwroot/                       # Arquivos estáticos (uploads)
│   └── Documentation/                 # Documentação interna do projeto
│
└── frontend/                          # SPA Angular 19
    ├── angular.json                   # Configuração do Angular CLI
    ├── package.json                   # Dependências npm
    ├── tsconfig.json                  # Configuração TypeScript
    ├── proxy.conf.json                # Proxy reverso para desenvolvimento
    ├── vercel.json                    # Configuração de deploy na Vercel
    └── src/
        ├── main.ts                    # Bootstrap do Angular
        ├── index.html                 # Template HTML
        ├── styles.css                 # Estilos globais
        └── app/                       # Componentes, serviços e rotas
            ├── components/            # Componentes reutilizáveis
            ├── pages/                 # Páginas da aplicação
            ├── services/              # Serviços (HTTP, autenticação, etc)
            ├── guards/                # Guards de rota
            ├── layouts/               # Layouts principais
            └── modules/               # Módulos da aplicação
```

---

## 📌 Pré-requisitos

### Para rodar o backend localmente
- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- Git

> O banco de dados em desenvolvimento é **SQLite** — não é necessário instalar nada extra.

### Para rodar o frontend localmente
- [Node.js 18+](https://nodejs.org/)
- npm 10+

### Opcional (PostgreSQL local ou Docker)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- PostgreSQL 15+

---

## 🔧 Instalação e Configuração

### 1. Clone o repositório

```bash
git clone https://github.com/seu-usuario/codex.git
cd codex
```

### 2. Configuração do Backend

```bash
cd backend

# Restaurar dependências
dotnet restore

# Aplicar migrações (cria o banco SQLite automaticamente em desenvolvimento)
dotnet ef database update
```

#### Variáveis de Ambiente (opcional para desenvolvimento)

Em desenvolvimento, os padrões já funcionam sem variáveis extras (SQLite local).
Para customizar, configure no sistema operacional ou via `dotnet user-secrets`:

```env
JWT_SECRET=sua_chave_secreta_minimo_32_caracteres

CLOUDINARY_CLOUD_NAME=seu_cloud_name
CLOUDINARY_API_KEY=sua_api_key
CLOUDINARY_API_SECRET=seu_api_secret

MAIL_HOST=smtp.gmail.com
MAIL_USERNAME=seu@email.com
MAIL_PASSWORD=sua_senha_de_app
MAIL_FROM=seu@email.com

FRONTEND_URL=http://localhost:4200
```

### 3. Configuração do Frontend

```bash
cd frontend
npm install
```

---

## 🚀 Executando o Projeto

### Backend

```bash
cd backend

# Modo desenvolvimento com hot reload
dotnet watch run

# Ou sem hot reload
dotnet run
```

Disponível em:
- **API:** `http://localhost:5000`
- **Swagger UI:** `http://localhost:5000/swagger`

### Frontend

```bash
cd frontend

# Servidor de desenvolvimento (proxy aponta /api/** para localhost:5000)
npm start
```

Disponível em: **`http://localhost:4200`**

### Com Docker Compose (Backend + PostgreSQL local)

```bash
cd backend
docker-compose up --build
```

---

## 📡 API Endpoints

A documentação interativa completa está em `http://localhost:5000/swagger`.

### Autenticação — `/auth`

| Método | Endpoint | Descrição | Auth |
|--------|----------|-----------|------|
| POST | `/auth/registrar` | Registrar novo usuário | ❌ |
| POST | `/auth/login` | Login e obtenção do JWT | ❌ |
| POST | `/auth/solicitar-redefinicao-senha` | Solicitar redefinição de senha por e-mail | ❌ |
| POST | `/auth/redefinir-senha` | Redefinir senha com token | ❌ |

**Exemplo de login:**
```bash
curl -X POST http://localhost:5000/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email": "usuario@exemplo.com", "senha": "senha123"}'
```

**Resposta:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "tipo": "Bearer",
  "usuario": {
    "id": 1,
    "nome": "João Silva",
    "email": "joao@exemplo.com",
    "tipo": "Admin"
  }
}
```

### Conteúdo — `/api/content`

| Método | Endpoint | Descrição | Auth |
|--------|----------|-----------|------|
| POST | `/api/content` | Criar novo conteúdo | ✅ |
| GET | `/api/content` | Listar todos os conteúdos | ❌ |
| GET | `/api/content/{id}` | Buscar conteúdo por ID | ❌ |
| PUT | `/api/content/{id}` | Atualizar conteúdo | ✅ |
| DELETE | `/api/content/{id}` | Deletar conteúdo | ✅ |
| GET | `/api/content/user/{userId}` | Conteúdos de um usuário | ❌ |

### Tópicos — `/api/topicos`

| Método | Endpoint | Descrição | Auth |
|--------|----------|-----------|------|
| POST | `/api/topicos` | Criar novo tópico | ✅ |
| GET | `/api/topicos` | Listar todos os tópicos | ❌ |
| GET | `/api/topicos/{id}` | Buscar tópico por ID | ❌ |
| PUT | `/api/topicos/{id}` | Atualizar tópico | ✅ |
| DELETE | `/api/topicos/{id}` | Deletar tópico | ✅ |
| GET | `/api/topicos/categoria/{categoriaId}` | Tópicos de uma categoria | ❌ |

### Busca — `/api/search`

| Método | Endpoint | Descrição | Auth |
|--------|----------|-----------|------|
| GET | `/api/search?q={query}` | Buscar por palavra-chave | ❌ |
| GET | `/api/search/advanced` | Busca avançada com filtros | ❌ |
| GET | `/api/search/tags/{tag}` | Buscar por tag | ❌ |

### Upload de Arquivos — `/api/uploads`

| Método | Endpoint | Descrição | Auth |
|--------|----------|-----------|------|
| POST | `/api/uploads` | Upload de arquivo | ✅ |
| GET | `/api/uploads` | Listar meus uploads | ✅ |
| DELETE | `/api/uploads/{id}` | Deletar arquivo | ✅ |

### Usuários — `/api/usuarios`

| Método | Endpoint | Descrição | Auth |
|--------|----------|-----------|------|
| GET | `/api/usuarios` | Listar todos os usuários | Admin |
| GET | `/api/usuarios/me` | Dados do usuário autenticado | ✅ |
| GET | `/api/usuarios/{id}` | Buscar usuário por ID | ✅ |
| PUT | `/api/usuarios/{id}` | Atualizar usuário | ✅ |
| DELETE | `/api/usuarios/{id}` | Deletar usuário | Admin |

Endpoints marcados com ✅ requerem o header:
```
Authorization: Bearer <seu_jwt_token>
```

---

## 🔐 Variáveis de Ambiente

### Backend

| Variável | Descrição | Padrão (dev) |
|----------|-----------|-------------|
| `JWT_SECRET` | Chave secreta para assinatura do JWT | valor interno (não usar em produção) |
| `DB_HOST` | Host do PostgreSQL (prod/homolog) | `postgres` |
| `DB_PORT` | Porta do PostgreSQL | `5432` |
| `DB_NAME` | Nome do banco | `codex_prd_db` |
| `DB_USER` | Usuário do banco | `postgres` |
| `DB_PASSWORD` | Senha do banco | `postgres` |
| `CLOUDINARY_CLOUD_NAME` | Nome do cloud no Cloudinary | — |
| `CLOUDINARY_API_KEY` | Chave da API do Cloudinary | — |
| `CLOUDINARY_API_SECRET` | Segredo da API do Cloudinary | — |
| `MAIL_HOST` | Servidor SMTP | `smtp.gmail.com` |
| `MAIL_USERNAME` | Usuário do e-mail | — |
| `MAIL_PASSWORD` | Senha do e-mail (senha de app) | — |
| `MAIL_FROM` | Endereço de remetente | — |
| `FRONTEND_URL` | URL do frontend (para links nos e-mails) | `http://localhost:4200` |

> **Provedores de e-mail suportados:** Gmail, Outlook, Hotmail e iCloud (todos via SMTP + STARTTLS na porta 587).

---

## 🚢 Deploy

### Backend — Render (Docker)

O backend é containerizado e deployado automaticamente no **Render** via `Dockerfile` na raiz do diretório backend.

```bash
# Build manual da imagem
cd backend
docker build -t codex-backend .

# Publicar para produção manualmente
dotnet publish -c Release -o ./publish
```

### Frontend — Vercel

O frontend é deployado automaticamente na **Vercel** a cada push na branch principal. A configuração de deployment está em `frontend/vercel.json`.

#### Deployment Automático

1. **Configuração:** Conecte o repositório GitHub à Vercel
2. **Trigger:** Cada push na branch `main` inicia o deployment
3. **Build:** Vercel executa `npm run build` automaticamente
4. **Output:** Saída otimizada em `dist/codex/browser/`

#### Build Manual

```bash
cd frontend

# Build para produção
npm run build

# Build com SSR
npm run build:ssr

# Teste localmente
npm run serve
```

#### Deploy Manual (se necessário)

```bash
cd frontend

# Fazer login na Vercel
vercel login

# Deploy para produção
vercel --prod

# Deploy para preview
vercel
```

### URLs dos Ambientes

| Ambiente | Backend | Frontend |
|----------|---------|---------|
| Development | `http://localhost:5000` | `http://localhost:4200` |
| Homolog | Render (Staging) | Vercel Preview |
| Production | `https://api.codex.com` | `https://codex.vercel.app` |

---

## 📐 Modelos de Dados

### Usuario

| Campo | Tipo | Descrição |
|-------|------|-----------|
| `Id` | long | Identificador único |
| `Nome` | string | Nome completo |
| `Email` | string | E-mail único |
| `Senha` | string | Hash BCrypt |
| `FotoPerfil` | string? | URL da foto de perfil |
| `Tipo` | enum | `Admin` (0) ou `Usuario` (1) |
| `Ativo` | bool | Se o usuário está ativo |
| `CriadoEm` | DateTime | Data de criação |
| `AtualizadoEm` | DateTime | Data da última atualização |

### Conteudo

| Campo | Tipo | Descrição |
|-------|------|-----------|
| `Id` | long | Identificador único |
| `UsuarioId` | long | FK para Usuario (criador) |
| `TopicoId` | long | FK para Topico |
| `Titulo` | string | Título do conteúdo |
| `Descricao` | string | Descrição detalhada |
| `Corpo` | string | Corpo/conteúdo principal |
| `Tags` | string | Tags para categorização |
| `Status` | enum | `Rascunho`, `Publicado`, `Arquivado` |
| `Visualizacoes` | int | Número de visualizações |
| `CriadoEm` | DateTime | Data de criação |
| `AtualizadoEm` | DateTime | Data da última atualização |

### Topico

| Campo | Tipo | Descrição |
|-------|------|-----------|
| `Id` | long | Identificador único |
| `Nome` | string | Nome do tópico |
| `Descricao` | string? | Descrição do tópico |
| `Categoria` | string | Categoria principal |
| `IconUrl` | string? | URL do ícone |
| `CriadoEm` | DateTime | Data de criação |
| `AtualizadoEm` | DateTime | Data da última atualização |

### Upload

| Campo | Tipo | Descrição |
|-------|------|-----------|
| `Id` | long | Identificador único |
| `UsuarioId` | long | FK para Usuario |
| `NomeOriginal` | string | Nome original do arquivo |
| `UrlCloudinary` | string | URL do arquivo no Cloudinary |
| `TipoMidia` | string | Tipo de mídia (image, document, video) |
| `Tamanho` | long | Tamanho do arquivo em bytes |
| `CriadoEm` | DateTime | Data do upload |

---

## 👥 Créditos

### Equipe CODEX

<table>
  <tr>
    <td align="center">
      <a href="https://github.com/seu-usuario">
        <img src="https://github.com/seu-usuario.png" width="80px" alt="Seu Nome"/>
        <br />
        <sub><b>Seu Nome</b></sub>
      </a>
      <br />
      <small>Tech Lead & Full Stack Developer</small>
    </td>
    <td align="center">
      <a href="https://github.com/outro-usuario">
        <img src="https://github.com/outro-usuario.png" width="80px" alt="Outro Nome"/>
        <br />
        <sub><b>Outro Nome</b></sub>
      </a>
      <br />
      <small>Developer</small>
    </td>
  </tr>
</table>

---

<div align="center">
  <p>Feito com 💜 pela equipe CODEX</p>
  <p><strong>Organize, compartilhe e evolua seu conhecimento técnico.</strong></p>
  <sub>© 2026 CODEX. Todos os direitos reservados.</sub>
</div>
