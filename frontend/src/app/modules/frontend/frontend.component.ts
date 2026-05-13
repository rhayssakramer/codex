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
  selector: 'app-frontend',
  standalone: true,
  imports: [CommonModule, RouterOutlet],
  templateUrl: './frontend.component.html',
  styleUrls: ['./frontend.component.css']
})
export class FrontendComponent implements OnInit {
  disciplinas: Disciplina[] = [
    {
      id: 1,
      nome: 'HTML & CSS',
      descricao: 'Domine a estrutura e estilo de páginas web',
      progresso: 0
    },
    {
      id: 2,
      nome: 'JavaScript Essencial',
      descricao: 'JavaScript puro e boas práticas',
      progresso: 0
    },
    {
      id: 3,
      nome: 'React',
      descricao: 'Componentes, hooks e gerenciamento de estado',
      progresso: 0
    },
    {
      id: 4,
      nome: 'Angular',
      descricao: 'Framework completo para aplicações web robustas',
      progresso: 0
    },
    {
      id: 5,
      nome: 'Vue.js',
      descricao: 'Framework progressivo e fácil de aprender',
      progresso: 0
    },
    {
      id: 6,
      nome: 'TypeScript',
      descricao: 'JavaScript com tipagem estática e segurança',
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
        
        if (contentArea === 'frontend' && contentDisciplina === disciplina.nome) {
          topicosUnicos.add(content.topico);
        }
      });

      if (topicosUnicos.size > 0) {
        disciplina.progresso = 0;
      }
    });
  }

  entrarDisciplina(disciplinaId: number): void {
    this.router.navigate(['/dashboard/frontend/disciplina', disciplinaId]);
  }
}
