import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterOutlet, ActivatedRoute, NavigationEnd } from '@angular/router';
import { filter } from 'rxjs/operators';

interface Disciplina {
  id: number;
  nome: string;
  descricao: string;
  progresso: number;
}

@Component({
  selector: 'app-devops',
  standalone: true,
  imports: [CommonModule, RouterOutlet],
  templateUrl: './devops.component.html',
  styleUrls: ['./devops.component.css']
})
export class DevopsComponent implements OnInit {
  disciplinas: Disciplina[] = [
    {
      id: 1,
      nome: 'Docker & Containers',
      descricao: 'Containerização de aplicações',
      progresso: 0
    },
    {
      id: 2,
      nome: 'Kubernetes',
      descricao: 'Orquestração e gerenciamento de containers',
      progresso: 0
    },
    {
      id: 3,
      nome: 'CI/CD',
      descricao: 'Integração e entrega contínua',
      progresso: 0
    },
    {
      id: 4,
      nome: 'AWS',
      descricao: 'Serviços Amazon Web Services',
      progresso: 0
    },
    {
      id: 5,
      nome: 'Terraform',
      descricao: 'Infrastructure as Code',
      progresso: 0
    },
    {
      id: 6,
      nome: 'Monitoring & Logs',
      descricao: 'Monitoramento e análise de logs',
      progresso: 0
    }
  ];

  visualizandoDisciplina: boolean = false;

  constructor(private router: Router, private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.atualizarProgressoDisciplinas();
    
    // Monitorar eventos de navegação para detectar quando uma disciplina está sendo visualizada
    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe(() => {
      // Verificar se há rotas filhas
      if (this.route.firstChild) {
        this.route.firstChild.params.subscribe(params => {
          this.visualizandoDisciplina = Object.keys(params).length > 0;
        });
      } else {
        this.visualizandoDisciplina = false;
      }
    });
  }

  atualizarProgressoDisciplinas(): void {
    const stored = localStorage.getItem('codex_contents');
    if (!stored) return;

    const allContents = JSON.parse(stored);

    this.disciplinas.forEach(disciplina => {
      const topicosUnicos = new Set<string>();

      allContents.forEach((content: any) => {
        const contentArea = content.area?.toLowerCase().trim() || '';
        const contentDisciplina = content.disciplina?.trim() || '';
        
        if (contentArea === 'devops' && contentDisciplina === disciplina.nome) {
          topicosUnicos.add(content.topico);
        }
      });

      if (topicosUnicos.size > 0) {
        disciplina.progresso = 0;
      }
    });
  }

  entrarDisciplina(disciplinaId: number): void {
    this.router.navigate(['/dashboard/devops/disciplina', disciplinaId]);
  }
}
