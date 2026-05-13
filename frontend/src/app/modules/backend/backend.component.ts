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
  selector: 'app-backend',
  standalone: true,
  imports: [CommonModule, RouterOutlet],
  templateUrl: './backend.component.html',
  styleUrls: ['./backend.component.css']
})
export class BackendComponent implements OnInit {
  disciplinas: Disciplina[] = [
    {
      id: 1,
      nome: 'Node.js & Express',
      descricao: 'Crie servidores web com JavaScript',
      progresso: 0
    },
    {
      id: 2,
      nome: 'Python & Django',
      descricao: 'Framework web poderoso para Python',
      progresso: 0
    },
    {
      id: 3,
      nome: 'Java & Spring Boot',
      descricao: 'Desenvolvimento enterprise com Java',
      progresso: 0
    },
    {
      id: 4,
      nome: 'Bancos de Dados SQL',
      descricao: 'PostgreSQL, MySQL e SQL Server',
      progresso: 0
    },
    {
      id: 5,
      nome: 'Bancos NoSQL',
      descricao: 'MongoDB, Firebase e alternativas',
      progresso: 0
    },
    {
      id: 6,
      nome: 'APIs RESTful',
      descricao: 'Projete e implemente APIs profissionais',
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
        
        if (contentArea === 'backend' && contentDisciplina === disciplina.nome) {
          topicosUnicos.add(content.topico);
        }
      });

      if (topicosUnicos.size > 0) {
        disciplina.progresso = 0;
      }
    });
  }

  entrarDisciplina(disciplinaId: number): void {
    this.router.navigate(['/dashboard/backend/disciplina', disciplinaId]);
  }
}
