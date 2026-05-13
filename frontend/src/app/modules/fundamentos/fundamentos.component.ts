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
  selector: 'app-fundamentos',
  standalone: true,
  imports: [CommonModule, RouterOutlet],
  templateUrl: './fundamentos.component.html',
  styleUrls: ['./fundamentos.component.css']
})
export class FundamentosComponent implements OnInit {
  disciplinas: Disciplina[] = [
    {
      id: 1,
      nome: 'Lógica de Programação',
      descricao: 'Aprenda os conceitos básicos de lógica e estrutura de programação',
      progresso: 0
    },
    {
      id: 2,
      nome: 'Algoritmos',
      descricao: 'Entenda algoritmos e como criar soluções eficientes',
      progresso: 0
    },
    {
      id: 3,
      nome: 'Estruturas de Dados',
      descricao: 'Aprenda sobre arrays, listas, stacks, queues e mais',
      progresso: 0
    },
    {
      id: 4,
      nome: 'Versionamento com Git',
      descricao: 'Domine Git e controle de versão',
      progresso: 0
    },
    {
      id: 5,
      nome: 'Terminal e Command Line',
      descricao: 'Use o terminal como um profissional',
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
      // Encontrar tópicos desta disciplina
      const topicosUnicos = new Set<string>();
      const topicosCompletos = new Set<string>();

      allContents.forEach((content: any) => {
        const contentArea = content.area?.toLowerCase().trim() || '';
        const contentDisciplina = content.disciplina?.trim() || '';
        
        if (contentArea === 'fundamentos' && contentDisciplina === disciplina.nome) {
          topicosUnicos.add(content.topico);
        }
      });

      // Como não temos persistência do estado "concluído", calcular baseado no count
      // Se houver tópicos criados, progresso começa em 0
      if (topicosUnicos.size > 0) {
        disciplina.progresso = 0;
      }
    });
  }

  entrarDisciplina(disciplinaId: number): void {
    this.router.navigate(['/dashboard/fundamentos/disciplina', disciplinaId]);
  }
}
