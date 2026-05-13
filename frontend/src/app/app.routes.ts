import { Routes } from '@angular/router';
import { authGuard } from './guards/auth.guard';
import { LoginComponent } from './pages/login/login.component';
import { DashboardLayoutComponent } from './layouts/dashboard-layout/dashboard-layout.component';
import { DashboardHomeComponent } from './components/dashboard-home/dashboard-home.component';
import { DisciplinaDetailComponent } from './components/disciplina-detail/disciplina-detail.component';
import { TopicoContentComponent } from './components/topico-content/topico-content.component';
import { ContentManagerComponent } from './admin/content-manager/content-manager.component';

export const routes: Routes = [
  {
    path: '',
    redirectTo: '/login',
    pathMatch: 'full'
  },
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: 'dashboard',
    component: DashboardLayoutComponent,
    canActivate: [authGuard],
    children: [
      {
        path: '',
        component: DashboardHomeComponent
      },
      {
        path: 'admin',
        component: ContentManagerComponent
      },
      {
        path: 'fundamentos',
        loadComponent: () => import('./modules/fundamentos/fundamentos.component').then(m => m.FundamentosComponent),
        children: [
          {
            path: 'disciplina/:id',
            component: DisciplinaDetailComponent,
            data: { area: 'fundamentos' },
            children: [
              {
                path: 'topico/:topico',
                component: TopicoContentComponent,
                data: { area: 'fundamentos' }
              }
            ]
          }
        ]
      },
      {
        path: 'frontend',
        loadComponent: () => import('./modules/frontend/frontend.component').then(m => m.FrontendComponent),
        children: [
          {
            path: 'disciplina/:id',
            component: DisciplinaDetailComponent,
            data: { area: 'frontend' },
            children: [
              {
                path: 'topico/:topico',
                component: TopicoContentComponent,
                data: { area: 'frontend' }
              }
            ]
          }
        ]
      },
      {
        path: 'backend',
        loadComponent: () => import('./modules/backend/backend.component').then(m => m.BackendComponent),
        children: [
          {
            path: 'disciplina/:id',
            component: DisciplinaDetailComponent,
            data: { area: 'backend' },
            children: [
              {
                path: 'topico/:topico',
                component: TopicoContentComponent,
                data: { area: 'backend' }
              }
            ]
          }
        ]
      },
      {
        path: 'certificacoes',
        loadComponent: () => import('./modules/certificacoes/certificacoes.component').then(m => m.CertificacoesComponent),
        children: [
          {
            path: 'disciplina/:id',
            component: DisciplinaDetailComponent,
            data: { area: 'certificacoes' },
            children: [
              {
                path: 'topico/:topico',
                component: TopicoContentComponent,
                data: { area: 'certificacoes' }
              }
            ]
          }
        ]
      },
      {
        path: 'devops',
        loadComponent: () => import('./modules/devops/devops.component').then(m => m.DevopsComponent),
        children: [
          {
            path: 'disciplina/:id',
            component: DisciplinaDetailComponent,
            data: { area: 'devops' },
            children: [
              {
                path: 'topico/:topico',
                component: TopicoContentComponent,
                data: { area: 'devops' }
              }
            ]
          }
        ]
      }
    ]
  },
  {
    path: '**',
    redirectTo: '/login'
  }
];
