# Codex API - Backend .NET 9

Backend da plataforma Codex de educação em programação, desenvolvido em .NET 9 com arquitetura limpa e segurança em primeiro lugar.

## 🚀 Características

- **Autenticação JWT**: Sistema seguro com tokens JWT
- **3 Ambientes**: Desenvolvimento (SQLite), Homolog (Neon), Produção (Neon)
- **Entity Framework Core**: ORM moderno com suporte a múltiplos bancos
- **API RESTful**: Endpoints bem documentados com Swagger
- **CORS Configurável**: Proteção contra requisições não autorizadas
- **Segurança**: Headers de segurança, hashing de senhas, validações
- **Logging**: Sistema de logs estruturados
- **Health Checks**: Monitoramento de saúde da aplicação

## 📋 Pré-requisitos

- **.NET 9 SDK** - [Download](https://dotnet.microsoft.com/download)
- **Visual Studio 2022** ou **VS Code** (opcional)
- **PostgreSQL** - Para Homolog e Produção (Neon)
- **Git**

## 🛠️ Instalação e Configuração

### 1. Clonar o repositório

```bash
git clone https://github.com/seu-usuario/codex.git
cd codex/backend
```

### 2. Configurar variáveis de ambiente

```bash
# Copiar arquivo de exemplo
cp .env.example .env

# Editar .env com seus valores
# Em desenvolvimento, você pode deixar os padrões
```

### 3. Restaurar dependências

```bash
dotnet restore
```

### 4. Aplicar migrations do banco

```bash
# Desenvolvimento (SQLite)
dotnet ef database update

# Ou com ambiente específico
dotnet ef database update --environment Development
```

## 🏃 Executando a aplicação

### Desenvolvimento (SQLite)

```bash
# Com hot reload
dotnet watch run

# Ou normalmente
dotnet run
```

A API estará disponível em `https://localhost:7000` (HTTPS) ou `http://localhost:5000` (HTTP)

Swagger disponível em `http://localhost:5000` ou `https://localhost:7000`

### Homolog (Neon PostgreSQL)

```bash
dotnet run --launch-profile Homolog
# ou
ASPNETCORE_ENVIRONMENT=Homolog dotnet run
```

### Produção (Neon PostgreSQL)

```bash
ASPNETCORE_ENVIRONMENT=Production dotnet run
```

## 📚 Estrutura do Projeto

```
CodexAPI/
├── Controllers/          # Endpoints HTTP
│   ├── AuthController.cs
│   ├── TopicosController.cs
│   └── ContentController.cs
├── Services/            # Lógica de negócio
│   ├── AuthService.cs
│   └── IAuthService.cs
├── Repositories/        # Acesso a dados
│   └── GenericRepositories.cs
├── Models/             # Entidades do domínio
│   └── DomainModels.cs
├── DTOs/               # Data Transfer Objects
│   └── DtoModels.cs
├── Data/               # Entity Framework
│   └── CodexDbContext.cs
├── Middleware/         # Middleware customizado
│   └── SecurityHeadersMiddleware.cs
└── Program.cs          # Configuração principal
```

## 🔐 Autenticação e Segurança

### Login

```bash
POST /api/auth/login
Content-Type: application/json

{
  "email": "usuario@example.com",
  "senha": "senha123"
}

Response:
{
  "sucesso": true,
  "dados": {
    "id": 1,
    "email": "usuario@example.com",
    "nome": "João",
    "token": "eyJhbGciOiJIUzI1NiIs...",
    "tokenExpiresAt": "2024-05-14T10:00:00Z"
  }
}
```

### Registro

```bash
POST /api/auth/register
Content-Type: application/json

{
  "email": "novo@example.com",
  "nome": "João",
  "sobrenome": "Silva",
  "senha": "senha123",
  "confirmarSenha": "senha123"
}
```

### Usando o Token

Adicionar header em todas as requisições autenticadas:

```
Authorization: Bearer eyJhbGciOiJIUzI1NiIs...
```

### Variáveis de Ambiente de Segurança

| Variável | Descrição | Desenvolvimento | Homolog | Produção |
|----------|-----------|---|---|---|
| `JWT_SECRET_KEY` | Chave secreta JWT | Local | Via Vercel Secrets | Via Vercel Secrets |
| `DATABASE_URL_*` | URL do banco | SQLite local | Neon | Neon |
| `CORS_ORIGINS` | Origens CORS permitidas | localhost | *.vercel.app | *.codex.app |

## 📡 Endpoints Principais

### Auth
- `POST /api/auth/login` - Login
- `POST /api/auth/register` - Registro
- `GET /api/auth/me` - Dados do usuário atual
- `GET /api/auth/verify` - Verificar token

### Areas (Público)
- `GET /api/areas` - Listar todas as áreas
- `GET /api/areas/{id}` - Obter área específica

### Disciplinas (Público)
- `GET /api/disciplinas/area/{areaId}` - Disciplinas de uma área
- `GET /api/disciplinas/{id}` - Obter disciplina

### Tópicos (Público)
- `GET /api/topicos/disciplina/{disciplinaId}` - Tópicos de uma disciplina
- `GET /api/topicos/{id}` - Obter tópico

### Progresso (Autenticado)
- `POST /api/topicos/{id}/progresso` - Atualizar progresso
- `GET /api/topicos/{id}/meu-progresso` - Obter meu progresso

## 🗄️ Banco de Dados

### Ambiente: Desenvolvimento (SQLite)

Banco local para testes. Arquivo: `codex_develop.db`

```bash
# Reset do banco
rm codex_develop.db
dotnet ef database update
```

### Ambientes: Homolog e Produção (Neon PostgreSQL)

Usar PostgreSQL hospedado no [Neon](https://neon.tech)

Bancos:
- **Homolog**: `codex-homolog`
- **Produção**: `codex-prd`

String de conexão formato:
```
postgresql://user:password@host:5432/database-name?sslmode=require
```

## 🚀 Deploy no Vercel

### 1. Preparar o projeto

```bash
# Criar arquivo vercel.json
cat > vercel.json << 'EOF'
{
  "buildCommand": "dotnet publish -c Release -o out",
  "outputDirectory": "out",
  "env": {
    "DOTNET_ROOT": "/usr/local/bin/dotnet"
  }
}
EOF
```

### 2. Variáveis de Ambiente

Na dashboard do Vercel, adicionar:

```
ASPNETCORE_ENVIRONMENT=Homolog (ou Production)
DATABASE_URL_HOMOLOG=postgresql://...
DATABASE_URL_PRODUCTION=postgresql://...
JWT_SECRET_KEY=seu-secreto-muito-longo-e-seguro
CORS_ALLOWED_ORIGINS=https://seu-frontend.vercel.app
```

### 3. Deploy

```bash
vercel deploy
```

## 📊 Monitoramento

### Health Check

```bash
GET /health

Response:
{
  "status": "Healthy"
}
```

### Logs

Ver logs em tempo real:

```bash
dotnet run --verbosity debug
```

## 🧪 Testes

```bash
# Executar testes
dotnet test

# Com cobertura
dotnet test /p:CollectCoverage=true
```

## 📝 Migrations do Banco

### Criar nova migration

```bash
dotnet ef migrations add NomeDaMigration
```

### Aplicar migrations

```bash
dotnet ef database update
```

### Remover última migration

```bash
dotnet ef migrations remove
```

## 🔗 Integração com Frontend

O frontend Angular deve fazer requisições para:

**Desenvolvimento**: `http://localhost:5000/api`
**Homolog**: `https://api-homolog.vercel.app/api`
**Produção**: `https://api.codex.app/api`

Exemplo com Angular:

```typescript
// auth.service.ts
login(email: string, password: string): Observable<AuthResponse> {
  return this.http.post<AuthResponse>('/api/auth/login', {
    email, 
    senha: password
  });
}
```

## 🐛 Troubleshooting

### Erro: "Connection string not found"
- Verificar arquivo `appsettings.json` ou variáveis de ambiente
- Executar: `echo $DATABASE_URL`

### Erro: "JWT SecretKey not configured"
- Adicionar `JWT_SECRET_KEY` no `.env` ou variáveis de ambiente
- Mínimo 32 caracteres

### Erro: "CORS policy"
- Verificar `CORS_ALLOWED_ORIGINS` nas settings
- Frontend URL deve estar na lista branca

### Erro ao conectar ao Neon
- Verificar se URL tem `?sslmode=require`
- Checar firewall/VPN
- Testar: `psql conexão-string`

## 📞 Contato e Suporte

Para dúvidas ou problemas, abra uma issue no repositório.

## 📄 Licença

MIT License - veja LICENSE.md

---

**Desenvolvido com ❤️ usando .NET 9 e C#**
