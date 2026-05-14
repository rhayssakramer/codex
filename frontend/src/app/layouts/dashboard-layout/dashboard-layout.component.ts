import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule, Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { SearchService, SearchResult } from '../../services/search.service';
import { ProfileEditComponent } from '../../components/profile-edit/profile-edit.component';

@Component({
  selector: 'app-dashboard-layout',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, ProfileEditComponent],
  templateUrl: './dashboard-layout.component.html',
  styleUrls: ['./dashboard-layout.component.css']
})
export class DashboardLayoutComponent implements OnInit {
  user: any = null;
  isAdmin = false;
  sidebarOpen = true;
  userMenuOpen = false;
  showProfileEditModal = false;
  notificationsOpen = false;
  unreadNotifications = 0;
  notifications: any[] = [];
  searchQuery = '';
  searchResults: SearchResult[] = [];
  showSearchResults = false;
  showNoResultsModal = false;
  noResultsQuery = '';

  areas = [
    { id: 'fundamentos', name: 'Fundamentos' },
    { id: 'frontend', name: 'Frontend'},
    { id: 'backend', name: 'Backend'},
    { id: 'devops', name: 'DevOps' },
    { id: 'certificacoes', name: 'Certificações' }
  ];

  constructor(
    private authService: AuthService,
    private searchService: SearchService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.authService.user$.subscribe(user => {
      this.user = user;
      this.isAdmin = user?.role === 'admin';
    });
  }

  toggleSidebar(): void {
    this.sidebarOpen = !this.sidebarOpen;
  }

  toggleUserMenu(): void {
    this.userMenuOpen = !this.userMenuOpen;
  }

  closeUserMenu(): void {
    this.userMenuOpen = false;
  }

  openProfileEdit(): void {
    this.showProfileEditModal = true;
    this.userMenuOpen = false;
  }

  closeProfileEdit(): void {
    this.showProfileEditModal = false;
  }

  onProfileSave(updatedProfile: any): void {
    // Atualizar dados do usuário
    this.authService.updateUserProfile(updatedProfile);
  }

  toggleNotifications(): void {
    this.notificationsOpen = !this.notificationsOpen;
    if (this.notificationsOpen) {
      this.userMenuOpen = false;
    }
  }

  onSearch(query: string): void {
    this.searchQuery = query;
    console.log('🔍 onSearch chamado com:', query);
    
    if (query.trim().length > 0) {
      console.log('📤 Enviando busca para API...');
      this.searchService.search(query).subscribe({
        next: (results: SearchResult[]) => {
          console.log('✅ Resultados recebidos:', results);
          console.log('📊 Total de resultados:', results.length);
          this.searchResults = results;
          this.showSearchResults = results.length > 0;
          console.log('🎯 showSearchResults setado para:', this.showSearchResults);
          console.log('📋 searchResults:', this.searchResults);
        },
        error: (error: any) => {
          console.error('❌ Erro na busca:', error);
          this.searchResults = [];
          this.showSearchResults = false;
        }
      });
    } else {
      console.log('❌ Query vazio');
      this.searchResults = [];
      this.showSearchResults = false;
    }
  }

  onSearchResultClick(result: SearchResult): void {
    console.log('=== CLICOU EM RESULTADO ===');
    console.log('Resultado completo:', result);
    console.log('Type:', result.type);
    console.log('Area:', result.area);
    console.log('DisciplinaId:', result.disciplinaId);
    console.log('Id:', result.id);
    
    switch (result.type) {
      case 'area':
        console.log('Navegando para AREA:', result.id);
        this.router.navigate(['/dashboard', result.id]).then(success => {
          console.log('Navegação para área:', success ? 'sucesso' : 'falhou');
        });
        break;
      case 'disciplina':
        console.log('Navegando para DISCIPLINA');
        if (result.area) {
          this.router.navigate(['/dashboard', result.area, 'disciplina', result.id], {
            state: { 
              disciplinaId: result.id,
              area: result.area 
            }
          }).then(success => {
            console.log('Navegação para disciplina:', success ? 'sucesso' : 'falhou');
          });
        } else {
          console.error('Disciplina sem area!');
        }
        break;
      case 'topico':
        console.log('Navegando para TOPICO');
        if (result.area && result.disciplinaId) {
          console.log(`Rota: /dashboard/${result.area}/disciplina/${result.disciplinaId}/topico/${result.id}`);
          this.router.navigate([
            '/dashboard', 
            result.area, 
            'disciplina', 
            result.disciplinaId, 
            'topico', 
            result.id
          ], {
            state: { 
              topicoId: result.id,
              disciplinaId: result.disciplinaId,
              area: result.area 
            }
          }).then(success => {
            console.log('Navegação para tópico:', success ? 'sucesso' : 'falhou');
          });
        } else {
          console.error('Tópico sem area ou disciplinaId:', result);
        }
        break;
    }
    // Fechar o dropdown após clicar
    this.searchQuery = '';
    this.searchResults = [];
    this.showSearchResults = false;
  }

  closeSearchResults(): void {
    setTimeout(() => {
      this.showSearchResults = false;
    }, 200);
  }

  onSearchEnter(): void {
    console.log('Enter pressionado. Resultados:', this.searchResults);
    
    if (this.searchResults.length > 0) {
      console.log('Navegando para primeiro resultado:', this.searchResults[0]);
      // Se há resultados, navega para o primeiro
      this.onSearchResultClick(this.searchResults[0]);
    } else if (this.searchQuery.trim().length > 0) {
      console.log('Nenhum resultado encontrado para:', this.searchQuery);
      // Mostra modal de não encontrado
      this.noResultsQuery = this.searchQuery;
      this.showNoResultsModal = true;
    }
  }

  closeNoResultsModal(): void {
    this.showNoResultsModal = false;
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
