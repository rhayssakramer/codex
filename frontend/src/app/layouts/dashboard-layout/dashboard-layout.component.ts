import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { ProfileEditComponent } from '../../components/profile-edit/profile-edit.component';

@Component({
  selector: 'app-dashboard-layout',
  standalone: true,
  imports: [CommonModule, RouterModule, ProfileEditComponent],
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

  areas = [
    { id: 'fundamentos', name: 'Fundamentos' },
    { id: 'frontend', name: 'Frontend'},
    { id: 'backend', name: 'Backend'},
    { id: 'devops', name: 'DevOps' },
    { id: 'certificacoes', name: 'Certificações' }
  ];

  constructor(
    private authService: AuthService,
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

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
