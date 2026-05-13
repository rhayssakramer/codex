import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

interface Area {
  name: string;
  route: string;
  color: string;
  description: string;
}

@Component({
  selector: 'app-dashboard-home',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dashboard-home.component.html',
  styleUrls: ['./dashboard-home.component.css']
})
export class DashboardHomeComponent implements OnInit {
  user: any;
  areas: Area[] = [
    {
      name: 'Fundamentos',
      route: '/dashboard/fundamentos',
      color: '#732BF5',
      description: 'Conceitos básicos de programação'
    },
    {
      name: 'Frontend',
      route: '/dashboard/frontend',
      color: '#FF6B6B',
      description: 'Desenvolvimento de interfaces'
    },
    {
      name: 'Backend',
      route: '/dashboard/backend',
      color: '#4ECDC4',
      description: 'Desenvolvimento de servidores'
    },
    {
      name: 'DevOps',
      route: '/dashboard/devops',
      color: '#FFE66D',
      description: 'Deploy e infraestrutura'
    },
    {
      name: 'Certificações',
      route: '/dashboard/certificacoes',
      color: '#A8E6CF',
      description: 'Programas de certificação'
    }
  ];

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.user = this.authService.getUser();
  }

  navigateTo(route: string): void {
    this.router.navigate([route]);
  }
}
