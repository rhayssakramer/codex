<div align="center">

# 🎨 CODEX Frontend

**Interface Moderna para a Biblioteca de Conteúdo de Programação**

Frontend SPA (Single Page Application) em Angular 19 com Server-Side Rendering (SSR) para a plataforma **CODEX**, oferecendo uma experiência intuitiva e responsiva para gerenciar, descobrir e compartilhar conteúdos de programação.

[![Framework](https://img.shields.io/badge/Framework-Angular%2019-DD0031?style=for-the-badge&logo=angular)](https://angular.io)
[![Language](https://img.shields.io/badge/Language-TypeScript%205.7-3178C6?style=for-the-badge&logo=typescript)](https://www.typescriptlang.org)
[![Build Tool](https://img.shields.io/badge/Build-Angular%20CLI%2019-DD0031?style=for-the-badge&logo=angular)](https://angular.io/cli)
[![Deploy](https://img.shields.io/badge/Deploy-Vercel-000000?style=for-the-badge&logo=vercel)](https://vercel.com)

</div>

---

## 📋 Índice

- [Sobre o Projeto](#-sobre-o-projeto)
- [Arquitetura](#-arquitetura)
- [Tecnologias](#-tecnologias)
- [Estrutura do Projeto](#-estrutura-do-projeto)
- [Pré-requisitos](#-pré-requisitos)
- [Instalação e Configuração](#-instalação-e-configuração)
- [Executando o Projeto](#-executando-o-projeto)
- [Componentes Principais](#-componentes-principais)
- [Serviços](#-serviços)
- [Roteamento](#-roteamento)
- [Autenticação e Autorização](#-autenticação-e-autorização)
- [Integração com Backend](#-integração-com-backend)
- [Variáveis de Ambiente](#-variáveis-de-ambiente)
- [Build e Deploy](#-build-e-deploy)
- [Padrões e Convenções](#-padrões-e-convenções)
- [Troubleshooting](#-troubleshooting)
- [Créditos](#-créditos)

---

## 🌟 Sobre o Projeto

O **CODEX Frontend** é uma aplicação web moderna que permite aos usuários:

- 📖 **Explorar Conteúdo** - Navegar por uma biblioteca organizada de tópicos de programação
- 🔍 **Buscar** - Encontrar recursos através de busca avançada e filtros inteligentes
- 📤 **Compartilhar** - Publicar e gerenciar conteúdos próprios
- 👤 **Gerenciar Perfil** - Administração de conta, preferências e histórico
- 📊 **Dashboard** - Visualizar estatísticas e progresso de aprendizado
- 💾 **Coleções** - Salvar e organizar conteúdos favoritos

### Características Técnicas

- ✅ **Single Page Application (SPA)** - Navegação sem recarregamento de página
- ✅ **Server-Side Rendering (SSR)** - Otimização para SEO e performance
- ✅ **Componentes Standalone** - Arquitetura moderna e modular do Angular 18+
- ✅ **Roteamento com Lazy Loading** - Carregamento sob demanda de módulos
- ✅ **Reatividade com RxJS** - Fluxo de dados reativo e eficiente
- ✅ **Design Responsivo** - Funciona perfeitamente em desktop, tablet e mobile
- ✅ **PWA Ready** - Suporte para Progressive Web App
- ✅ **Temas Customizáveis** - Sistema de temas claro/escuro

---

## 🏛️ Arquitetura

O frontend segue uma arquitetura **modular e componentizada** com padrões de design recomendados pelo Angular:

```
App (Root Component)
    ├── Layouts
    │   ├── Admin Layout (sidebar + navbar)
    │   └── Public Layout (navbar simples)
    │
    ├── Pages
    │   ├── Dashboard
    │   ├── Content Manager
    │   ├── Search
    │   ├── Profile
    │   └── Auth Pages
    │
    ├── Components Compartilhados
    │   ├── Header
    │   ├── Sidebar
    │   ├── Card
    │   ├── Modal
    │   └── ...
    │
    ├── Services
    │   ├── AuthService
    │   ├── ContentService
    │   ├── ApiService
    │   └── ...
    │
    ├── Guards
    │   ├── AuthGuard
    │   ├── AdminGuard
    │   └── UnsavedChangesGuard
    │
    └── Interceptors
        ├── ErrorInterceptor
        └── AuthInterceptor
```

### Fluxo de Dados

```
User Interaction
    ↓
Component Handler
    ↓
Service (Business Logic + HTTP)
    ↓
Backend API
    ↓
Response Processing
    ↓
Store/State Update
    ↓
Component Update (via RxJS Observable)
    ↓
Template Render
```

---

## 💻 Tecnologias

### Framework & Linguagem

| Tecnologia | Versão | Descrição |
|-----------|--------|-----------|
| **Angular** | 19.1.0 | Framework frontend reativo |
| **TypeScript** | 5.7 | Linguagem com tipagem estática |
| **RxJS** | 7.8.0 | Programação reativa com Observables |
| **Angular CLI** | 19.1.5 | Ferramenta de build e desenvolvimento |

### Rendering & Performance

| Tecnologia | Versão | Descrição |
|-----------|--------|-----------|
| **Angular SSR** | 19.1.5 | Server-Side Rendering para SEO |
| **Express** | ^4.21.2 | Servidor Node.js para SSR |

### Utilitários & Bibliotecas

| Biblioteca | Versão | Finalidade |
|-----------|--------|-----------|
| **Axios** | ^1.8.1 | HTTP Client (alternativa ao HttpClient) |
| **date-fns** | ^3.0.0 | Manipulação de datas |
| **zone.js** | ^0.15.0 | Gerenciamento de zona de execução |
| **tslib** | ^2.3.0 | Biblioteca de utilitários TypeScript |

### Desenvolvimento

| Ferramenta | Versão | Finalidade |
|-----------|--------|-----------|
| **TypeScript Compiler** | 5.7 | Compilação TypeScript |
| **Node.js** | 18+ | Runtime JavaScript |
| **npm** | 10+ | Gerenciador de pacotes |

---

## 📁 Estrutura do Projeto

```
frontend/
├── src/
│   ├── main.ts                        # Bootstrap da aplicação Angular
│   ├── main.server.ts                 # Bootstrap do servidor SSR
│   ├── server.ts                      # Configuração Express para SSR
│   ├── index.html                     # Template HTML principal
│   ├── styles.css                     # Estilos globais
│   ├── app/
│   │   ├── app.component.ts           # Componente raiz
│   │   ├── app.component.html         # Template raiz
│   │   ├── app.component.css          # Estilos raiz
│   │   ├── app.config.ts              # Configuração da aplicação
│   │   ├── app.routes.ts              # Definição de rotas
│   │   │
│   │   ├── layouts/                   # Layouts reutilizáveis
│   │   │   ├── admin-layout/
│   │   │   │   ├── admin-layout.component.ts
│   │   │   │   ├── admin-layout.component.html
│   │   │   │   └── admin-layout.component.css
│   │   │   └── public-layout/
│   │   │       ├── public-layout.component.ts
│   │   │       ├── public-layout.component.html
│   │   │       └── public-layout.component.css
│   │   │
│   │   ├── pages/                     # Páginas da aplicação
│   │   │   ├── auth/
│   │   │   │   ├── login/
│   │   │   │   ├── register/
│   │   │   │   ├── forgot-password/
│   │   │   │   └── reset-password/
│   │   │   ├── dashboard/             # Dashboard principal
│   │   │   ├── content-manager/       # Gerenciador de conteúdos
│   │   │   ├── content-detail/        # Detalhe de conteúdo
│   │   │   ├── search/                # Página de busca
│   │   │   ├── disciplina-detail/     # Detalhe de disciplina
│   │   │   ├── profile/               # Perfil do usuário
│   │   │   ├── admin/                 # Painel administrativo
│   │   │   └── not-found/             # Página 404
│   │   │
│   │   ├── components/                # Componentes reutilizáveis
│   │   │   ├── header/
│   │   │   ├── sidebar/
│   │   │   ├── navbar/
│   │   │   ├── card/
│   │   │   ├── modal/
│   │   │   ├── pagination/
│   │   │   ├── loading-spinner/
│   │   │   ├── toast/
│   │   │   └── breadcrumb/
│   │   │
│   │   ├── services/                  # Serviços
│   │   │   ├── api.service.ts         # Cliente HTTP genérico
│   │   │   ├── auth.service.ts        # Autenticação e autorização
│   │   │   ├── content.service.ts     # Gerenciamento de conteúdos
│   │   │   ├── user.service.ts        # Gerenciamento de usuários
│   │   │   ├── search.service.ts      # Busca e filtros
│   │   │   ├── storage.service.ts     # LocalStorage/SessionStorage
│   │   │   └── notification.service.ts # Notificações do usuário
│   │   │
│   │   ├── guards/                    # Guards de rota
│   │   │   ├── auth.guard.ts
│   │   │   ├── admin.guard.ts
│   │   │   └── unsaved-changes.guard.ts
│   │   │
│   │   ├── interceptors/              # HTTP Interceptors
│   │   │   ├── auth.interceptor.ts
│   │   │   └── error.interceptor.ts
│   │   │
│   │   ├── models/                    # Interfaces e modelos TypeScript
│   │   │   ├── user.model.ts
│   │   │   ├── content.model.ts
│   │   │   ├── api-response.model.ts
│   │   │   └── auth.model.ts
│   │   │
│   │   ├── pipes/                     # Pipes customizados
│   │   │   ├── safe-html.pipe.ts
│   │   │   ├── highlight.pipe.ts
│   │   │   └── format-date.pipe.ts
│   │   │
│   │   ├── directives/                # Diretivas customizadas
│   │   │   ├── highlight.directive.ts
│   │   │   └── click-outside.directive.ts
│   │   │
│   │   └── utils/                     # Funções utilitárias
│   │       ├── validators.ts
│   │       ├── helpers.ts
│   │       └── constants.ts
│   │
│   ├── assets/                        # Arquivos estáticos
│   │   ├── images/
│   │   ├── icons/
│   │   ├── fonts/
│   │   └── data/
│   │
│   ├── environments/                  # Configurações por ambiente
│   │   ├── environment.ts             # Development
│   │   └── environment.prod.ts        # Production
│   │
│   ├── styles/                        # Estilos globais
│   │   ├── variables.css
│   │   ├── mixins.css
│   │   ├── normalize.css
│   │   └── theme.css
│   │
│   └── server/                        # Configuração SSR (se aplicável)
│       └── prerender-routes.ts        # Rotas para pré-renderização
│
├── angular.json                       # Configuração Angular CLI
├── tsconfig.json                      # Configuração TypeScript
├── tsconfig.app.json                  # Config TypeScript para aplicação
├── tsconfig.spec.json                 # Config TypeScript para testes
├── package.json                       # Dependências npm
├── package-lock.json                  # Lock file npm
├── proxy.conf.json                    # Proxy reverso para desenvolvimento
├── vercel.json                        # Configuração deploy Vercel
├── .angular/                          # Cache do Angular CLI
├── dist/                              # Output de build
├── node_modules/                      # Dependências instaladas
└── README.md                          # Este arquivo
```

---

## 📌 Pré-requisitos

### Sistema Operacional
- Windows, macOS ou Linux

### Software Necessário
- [Node.js 18+](https://nodejs.org/) - Runtime JavaScript
- [npm 10+](https://www.npmjs.com/) - Gerenciador de pacotes
- [Git](https://git-scm.com/) - Controle de versão

### Conhecimentos Recomendados
- Angular 19+
- TypeScript
- RxJS
- HTML5 & CSS3
- REST APIs

---

## 🔧 Instalação e Configuração

### 1. Clone o Repositório

```bash
git clone https://github.com/seu-usuario/codex.git
cd codex/frontend
```

### 2. Instale as Dependências

```bash
npm install
```

Isso irá:
- Baixar todas as dependências do `package.json`
- Instalar o Angular CLI globalmente (opcional)
- Criar a pasta `node_modules/`

### 3. Configuração do Ambiente

Crie um arquivo `.env.local` na raiz do frontend (opcional):

```env
NG_APP_API_BASE_URL=http://localhost:5000/api
NG_APP_ENVIRONMENT=development
NG_APP_JWT_TOKEN_KEY=codex_token
NG_APP_UPLOAD_CHUNK_SIZE=5242880
```

Ou configure via `src/environments/environment.ts`:

```typescript
export const environment = {
  production: false,
  apiBaseUrl: 'http://localhost:5000/api',
  jwtTokenKey: 'codex_token',
  uploadChunkSize: 5 * 1024 * 1024, // 5MB
};
```

### 4. Verificar Instalação

```bash
ng version
npm list
```

---

## 🚀 Executando o Projeto

### Modo Desenvolvimento com Hot Reload

```bash
npm start
# ou
ng serve
```

**Disponível em:** `http://localhost:4200`

O servidor automaticamente recarrega quando você salva mudanças em arquivos.

### Modo Desenvolvimento com Proxy

```bash
ng serve --proxy-config proxy.conf.json
```

Isso roteia as requisições para `/api/**` para o backend em `http://localhost:5000`.

### Build para Produção

```bash
npm run build
# ou
ng build --configuration production
```

**Output:** `dist/codex/browser/`

### Build com SSR (Server-Side Rendering)

```bash
npm run build:ssr
# ou
ng build --configuration production && npm run build:ssr
```

### Executar Servidor SSR Localmente

```bash
npm run serve:ssr
```

**Disponível em:** `http://localhost:4000`

### Executar Testes Unitários

```bash
npm test
# ou
ng test
```

### Executar Testes E2E

```bash
npm run e2e
# ou
ng e2e
```

### Lint e Verificação de Código

```bash
ng lint
```

---

## 🎨 Componentes Principais

### AppComponent

Componente raiz que inicializa toda a aplicação.

```typescript
// src/app/app.component.ts
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'CODEX';
  
  constructor(private authService: AuthService) {}
  
  ngOnInit() {
    this.authService.initializeApp();
  }
}
```

### HeaderComponent

Componente de navegação superior com menu e busca.

```typescript
// src/app/components/header/header.component.ts
@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
  standalone: true
})
export class HeaderComponent {
  isLoggedIn$ = this.authService.isLoggedIn$;
  user$ = this.authService.user$;
  
  constructor(
    private authService: AuthService,
    private searchService: SearchService
  ) {}
  
  logout() {
    this.authService.logout();
  }
}
```

### ContentManagerComponent

Gerenciador de conteúdos com CRUD.

```typescript
// src/app/pages/content-manager/content-manager.component.ts
@Component({
  selector: 'app-content-manager',
  templateUrl: './content-manager.component.html',
  styleUrls: ['./content-manager.component.css'],
  standalone: true
})
export class ContentManagerComponent implements OnInit {
  contents$ = this.contentService.getMyContents();
  
  constructor(private contentService: ContentService) {}
  
  deleteContent(id: number) {
    this.contentService.deleteContent(id).subscribe(() => {
      // Atualizar lista
    });
  }
}
```

### SearchComponent

Componente de busca avançada com filtros.

```typescript
// src/app/pages/search/search.component.ts
@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css'],
  standalone: true
})
export class SearchComponent implements OnInit {
  results$: Observable<Content[]>;
  
  constructor(
    private searchService: SearchService,
    private route: ActivatedRoute
  ) {}
  
  ngOnInit() {
    this.results$ = this.route.queryParams.pipe(
      switchMap(params => this.searchService.search(params.q))
    );
  }
}
```

---

## 🔧 Serviços

### AuthService

Gerencia autenticação, login, logout e autorização.

```typescript
// src/app/services/auth.service.ts
@Injectable({ providedIn: 'root' })
export class AuthService {
  private user$ = new BehaviorSubject<User | null>(null);
  isLoggedIn$ = this.user$.pipe(map(u => !!u));
  
  constructor(private api: ApiService) {}
  
  login(email: string, password: string): Observable<AuthResponse> {
    return this.api.post<AuthResponse>('/auth/login', { email, password });
  }
  
  logout() {
    this.user$.next(null);
    localStorage.removeItem('token');
  }
}
```

### ContentService

Gerencia operações com conteúdos (CRUD).

```typescript
// src/app/services/content.service.ts
@Injectable({ providedIn: 'root' })
export class ContentService {
  constructor(private api: ApiService) {}
  
  getContents(): Observable<Content[]> {
    return this.api.get<Content[]>('/content');
  }
  
  getContent(id: number): Observable<Content> {
    return this.api.get<Content>(`/content/${id}`);
  }
  
  createContent(data: CreateContentRequest): Observable<Content> {
    return this.api.post<Content>('/content', data);
  }
  
  updateContent(id: number, data: UpdateContentRequest): Observable<Content> {
    return this.api.put<Content>(`/content/${id}`, data);
  }
  
  deleteContent(id: number): Observable<void> {
    return this.api.delete(`/content/${id}`);
  }
}
```

### SearchService

Realiza buscas e filtros de conteúdo.

```typescript
// src/app/services/search.service.ts
@Injectable({ providedIn: 'root' })
export class SearchService {
  constructor(private api: ApiService) {}
  
  search(query: string, filters?: SearchFilters): Observable<Content[]> {
    return this.api.get<Content[]>('/search', { params: { q: query, ...filters } });
  }
}
```

### ApiService

Cliente HTTP genérico para comunicação com o backend.

```typescript
// src/app/services/api.service.ts
@Injectable({ providedIn: 'root' })
export class ApiService {
  constructor(private http: HttpClient) {}
  
  get<T>(endpoint: string, options?: any): Observable<T> {
    return this.http.get<T>(this.buildUrl(endpoint), options);
  }
  
  post<T>(endpoint: string, body: any, options?: any): Observable<T> {
    return this.http.post<T>(this.buildUrl(endpoint), body, options);
  }
  
  private buildUrl(endpoint: string): string {
    return `${environment.apiBaseUrl}${endpoint}`;
  }
}
```

---

## 🛣️ Roteamento

### Definição de Rotas

```typescript
// src/app/app.routes.ts
export const routes: Routes = [
  {
    path: '',
    component: PublicLayoutComponent,
    children: [
      { path: '', component: DashboardComponent },
      { path: 'search', component: SearchComponent },
      { path: 'content/:id', component: ContentDetailComponent }
    ]
  },
  {
    path: 'auth',
    children: [
      { path: 'login', component: LoginComponent },
      { path: 'register', component: RegisterComponent },
      { path: 'forgot-password', component: ForgotPasswordComponent }
    ]
  },
  {
    path: 'admin',
    component: AdminLayoutComponent,
    canActivate: [AdminGuard],
    children: [
      { path: 'dashboard', component: AdminDashboardComponent },
      { path: 'users', component: UsersManagementComponent },
      { path: 'content', component: ContentManagerComponent }
    ]
  },
  { path: '404', component: NotFoundComponent },
  { path: '**', redirectTo: '404' }
];
```

### Lazy Loading

```typescript
{
  path: 'courses',
  loadChildren: () => import('./modules/courses/courses.routes')
    .then(m => m.COURSES_ROUTES)
}
```

---

## 🔐 Autenticação e Autorização

### Auth Guard

```typescript
// src/app/guards/auth.guard.ts
@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
  constructor(
    private authService: AuthService,
    private router: Router
  ) {}
  
  canActivate(route: ActivatedRouteSnapshot): Observable<boolean> {
    return this.authService.isLoggedIn$.pipe(
      tap(isLoggedIn => {
        if (!isLoggedIn) {
          this.router.navigate(['/auth/login']);
        }
      })
    );
  }
}
```

### Admin Guard

```typescript
// src/app/guards/admin.guard.ts
@Injectable({ providedIn: 'root' })
export class AdminGuard implements CanActivate {
  canActivate(): Observable<boolean> {
    return this.authService.user$.pipe(
      map(user => user?.type === 'Admin'),
      tap(isAdmin => {
        if (!isAdmin) this.router.navigate(['/']);
      })
    );
  }
}
```

### JWT Token

O token é armazenado em `localStorage` com a chave `codex_token`:

```typescript
// Salvar token após login
localStorage.setItem('codex_token', response.token);

// Recuperar token
const token = localStorage.getItem('codex_token');

// Enviar em requests
headers.set('Authorization', `Bearer ${token}`);
```

---

## 🔗 Integração com Backend

### Endpoints Consumidos

| Serviço | Método | Endpoint | Descrição |
|---------|--------|----------|-----------|
| Auth | POST | `/auth/login` | Login do usuário |
| Auth | POST | `/auth/register` | Registro de novo usuário |
| Content | GET | `/api/content` | Listar conteúdos |
| Content | POST | `/api/content` | Criar conteúdo |
| Content | PUT | `/api/content/{id}` | Atualizar conteúdo |
| Content | DELETE | `/api/content/{id}` | Deletar conteúdo |
| Search | GET | `/api/search?q={query}` | Buscar conteúdo |
| Uploads | POST | `/api/uploads` | Upload de arquivo |

### Exemplo de Consumo

```typescript
this.http.get(`${environment.apiBaseUrl}/api/content`)
  .pipe(
    map(response => response.data),
    catchError(error => {
      console.error('Erro ao buscar conteúdos', error);
      return of([]);
    })
  )
  .subscribe(contents => {
    this.contents = contents;
  });
```

---

## 📦 Variáveis de Ambiente

### Desenvolvimento (environment.ts)

```typescript
export const environment = {
  production: false,
  apiBaseUrl: 'http://localhost:5000/api',
  jwtTokenKey: 'codex_token',
  uploadChunkSize: 5 * 1024 * 1024,
  logLevel: 'debug'
};
```

### Produção (environment.prod.ts)

```typescript
export const environment = {
  production: true,
  apiBaseUrl: 'https://api.codex.com/api',
  jwtTokenKey: 'codex_token',
  uploadChunkSize: 10 * 1024 * 1024,
  logLevel: 'error'
};
```

---

## 🏗️ Build e Deploy

### Build de Desenvolvimento

```bash
ng build
```

### Build de Produção

```bash
ng build --configuration production
```

**Output:** `dist/codex/browser/`

### Deploy na Vercel

A aplicação é deployada automaticamente na Vercel a cada push na branch principal.

```bash
# Configuração manual (se necessário)
vercel --prod
```

### Build com SSR

```bash
ng build --configuration production
npm run build:ssr
```

### Executar Build Local

```bash
npm run serve
```

---

## 📐 Padrões e Convenções

### Naming Conventions

- **Componentes:** `component-name.component.ts`
- **Serviços:** `service-name.service.ts`
- **Guards:** `guard-name.guard.ts`
- **Pipes:** `pipe-name.pipe.ts`
- **Diretivas:** `directive-name.directive.ts`
- **Interfaces:** `model-name.model.ts`

### Estrutura de Componente

```typescript
@Component({
  selector: 'app-component-name',
  templateUrl: './component-name.component.html',
  styleUrls: ['./component-name.component.css'],
  standalone: true,
  imports: [CommonModule, ...]
})
export class ComponentNameComponent implements OnInit, OnDestroy {
  @Input() data: any;
  @Output() event = new EventEmitter<any>();
  
  private destroy$ = new Subject<void>();
  
  constructor(private service: SomeService) {}
  
  ngOnInit() {
    // Inicialização
  }
  
  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
```

### RxJS Best Practices

```typescript
// ✅ Use unsubscribe pattern
subscription: Subscription;

ngOnInit() {
  this.subscription = this.service.getData().subscribe(...);
}

ngOnDestroy() {
  this.subscription.unsubscribe();
}

// ✅ Ou use takeUntil
private destroy$ = new Subject<void>();

ngOnInit() {
  this.service.getData()
    .pipe(takeUntil(this.destroy$))
    .subscribe(...);
}
```

---

## 🆘 Troubleshooting

### Problema: "Cannot find module 'app.routes'"

**Solução:**
```bash
npm install
ng serve
```

### Problema: "Angular module not found"

**Solução:**
```bash
rm -rf node_modules package-lock.json
npm install
```

### Problema: "Port 4200 already in use"

**Solução:**
```bash
ng serve --port 4300
```

### Problema: "CORS error na requisição"

**Solução:** Configure o proxy em `proxy.conf.json`:
```json
{
  "/api/*": {
    "target": "http://localhost:5000",
    "secure": false,
    "changeOrigin": true
  }
}
```

### Problema: "Autenticação expirada"

**Solução:** Implementar refresh token:
```typescript
this.http.post('/auth/refresh', {
  refreshToken: localStorage.getItem('refresh_token')
});
```

---

## 📚 Recursos Adicionais

- [Documentação Angular](https://angular.io/docs)
- [RxJS Documentation](https://rxjs.dev)
- [TypeScript Handbook](https://www.typescriptlang.org/docs)
- [Angular Best Practices](https://angular.io/guide/styleguide)

---

## 👥 Créditos

### Equipe Frontend

<table>
  <tr>
    <td align="center">
      <a href="https://github.com/seu-usuario">
        <img src="https://github.com/seu-usuario.png" width="80px" alt="Seu Nome"/>
        <br />
        <sub><b>Seu Nome</b></sub>
      </a>
      <br />
      <small>Frontend Lead Developer</small>
    </td>
  </tr>
</table>

---

<div align="center">
  <p>Feito com 💜 pela equipe CODEX</p>
  <p><strong>Organize, compartilhe e evolua seu conhecimento técnico.</strong></p>
  <sub>© 2026 CODEX Frontend. Todos os direitos reservados.</sub>
</div>
