import { Component, OnInit, ViewChild, ViewContainerRef, ChangeDetectionStrategy, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterOutlet, NavigationEnd, ActivationEnd } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { filter } from 'rxjs/operators';

interface Topico {
  id: number;
  nome: string;
  descricao: string;
  duracao: string;
  durationMinutos?: number;
  concluido: boolean;
}

interface Disciplina {
  id: number;
  nome: string;
  descricao: string;
  progresso: number;
  topicos: Topico[];
}

@Component({
  selector: 'app-disciplina-detail',
  standalone: true,
  imports: [CommonModule, RouterOutlet, FormsModule],
  templateUrl: './disciplina-detail.component.html',
  styleUrls: ['./disciplina-detail.component.css']
})
export class DisciplinaDetailComponent implements OnInit {
  disciplina: Disciplina | null = null;
  area: string = '';
  disciplinaId: number = 0;
  searchTopico: string = '';
  
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  get visualizandoTopico(): boolean {
    // Verificar direto na URL se há "/topico/"
    return this.router.url.includes('/topico/');
  }

  // Base de dados de disciplinas por área
  disciplinasData: { [key: string]: Disciplina[] } = {
    fundamentos: [
      {
        id: 1,
        nome: 'Lógica de Programação',
        descricao: 'Aprenda os conceitos fundamentais de lógica',
        progresso: 65,
        topicos: [
          { id: 1, nome: 'Sequência', descricao: 'Instruções em ordem', duracao: '15 min', concluido: true },
          { id: 2, nome: 'Decisão', descricao: 'If, else e operadores lógicos', duracao: '20 min', concluido: true },
          { id: 3, nome: 'Repetição', descricao: 'Loops e iterações', duracao: '25 min', concluido: false },
          { id: 4, nome: 'Funções', descricao: 'Modularização e reuso', duracao: '20 min', concluido: false }
        ]
      },
      {
        id: 2,
        nome: 'Algoritmos',
        descricao: 'Estruturação e otimização de algoritmos',
        progresso: 40,
        topicos: [
          { id: 1, nome: 'Introdução', descricao: 'O que são algoritmos', duracao: '10 min', concluido: true },
          { id: 2, nome: 'Análise de Complexidade', descricao: 'Big O Notation', duracao: '30 min', concluido: false },
          { id: 3, nome: 'Busca e Ordenação', descricao: 'Algoritmos clássicos', duracao: '35 min', concluido: false }
        ]
      },
      {
        id: 3,
        nome: 'Estruturas de Dados',
        descricao: 'Arrays, listas, árvores e grafos',
        progresso: 55,
        topicos: [
          { id: 1, nome: 'Arrays e Listas', descricao: 'Estruturas lineares', duracao: '25 min', concluido: true },
          { id: 2, nome: 'Stacks e Queues', descricao: 'LIFO e FIFO', duracao: '20 min', concluido: true },
          { id: 3, nome: 'Árvores', descricao: 'Estruturas hierárquicas', duracao: '30 min', concluido: false },
          { id: 4, nome: 'Grafos', descricao: 'Redes e conexões', duracao: '35 min', concluido: false }
        ]
      },
      {
        id: 4,
        nome: 'Versionamento com Git',
        descricao: 'Controle de versão profissional',
        progresso: 80,
        topicos: [
          { id: 1, nome: 'Git Basics', descricao: 'Clone, commit, push', duracao: '20 min', concluido: true },
          { id: 2, nome: 'Branching', descricao: 'Trabalhar com branches', duracao: '20 min', concluido: true },
          { id: 3, nome: 'Merge e Rebase', descricao: 'Combinando commits', duracao: '25 min', concluido: true },
          { id: 4, nome: 'GitHub Workflow', descricao: 'Pull requests e issues', duracao: '20 min', concluido: false }
        ]
      },
      {
        id: 5,
        nome: 'Terminal e Command Line',
        descricao: 'Domine a linha de comando',
        progresso: 75,
        topicos: [
          { id: 1, nome: 'Navegação', descricao: 'pwd, cd, ls', duracao: '15 min', concluido: true },
          { id: 2, nome: 'Manipulação de Arquivos', descricao: 'cp, mv, rm, touch', duracao: '20 min', concluido: true },
          { id: 3, nome: 'Permissões', descricao: 'chmod, chown', duracao: '20 min', concluido: true },
          { id: 4, nome: 'Processamento de Texto', descricao: 'grep, sed, awk', duracao: '25 min', concluido: false }
        ]
      }
    ],
    frontend: [
      {
        id: 3,
        nome: 'HTML',
        descricao: 'Linguagem de marcação para estruturar páginas web',
        progresso: 0,
        topicos: []
      },
      {
        id: 4,
        nome: 'CSS',
        descricao: 'Estilização e layout de páginas web',
        progresso: 0,
        topicos: []
      },
      {
        id: 14,
        nome: 'JavaScript',
        descricao: 'Linguagem do navegador',
        progresso: 0,
        topicos: []
      }
    ],
    backend: [
      {
        id: 5,
        nome: 'C#',
        descricao: 'Linguagem de programação C#',
        progresso: 0,
        topicos: []
      },
      {
        id: 6,
        nome: '.NET',
        descricao: 'Framework .NET para aplicações web e desktop',
        progresso: 0,
        topicos: []
      },
      {
        id: 7,
        nome: 'JavaScript',
        descricao: 'JavaScript no backend com Node.js',
        progresso: 0,
        topicos: []
      }
    ],
    devops: [
      {
        id: 12,
        nome: 'Git',
        descricao: 'Controle de versão com Git',
        progresso: 0,
        topicos: []
      },
      {
        id: 13,
        nome: 'GitHub',
        descricao: 'Plataforma de colaboração e hospedagem de código',
        progresso: 0,
        topicos: []
      }
    ],
    certificacoes: [
      {
        id: 8,
        nome: 'AZ-900',
        descricao: 'Certificação fundamentals Microsoft Azure',
        progresso: 0,
        topicos: []
      },
      {
        id: 9,
        nome: 'AI-900',
        descricao: 'Certificação AI fundamentals Microsoft Azure',
        progresso: 0,
        topicos: []
      },
      {
        id: 10,
        nome: 'GH-900',
        descricao: 'Certificação GitHub fundamentals',
        progresso: 0,
        topicos: []
      },
      {
        id: 11,
        nome: 'GH-300',
        descricao: 'Certificação GitHub advanced',
        progresso: 0,
        topicos: []
      }
    ]
  };

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.area = this.route.snapshot.data['area'] || 'fundamentos';
      this.disciplinaId = +params['id'];
      this.loadDisciplina();
    });
  }

  hasActiveChild(): boolean {
    // Verificar se há uma rota filha ativa (significa que está visualizando um tópico)
    const hasChild = !!this.route.firstChild;
    console.log('hasActiveChild:', hasChild);
    return hasChild;
  }

  loadDisciplina(): void {
    const disciplinas = this.disciplinasData[this.area] || [];
    this.disciplina = disciplinas.find(d => d.id === this.disciplinaId) || null;
    
    if (this.disciplina) {
      // Carregar tópicos do localStorage (criados no gerenciar conteúdo)
      this.loadTopicosFromStorage();
    }
  }

  loadTopicosFromStorage(): void {
    if (!this.disciplina) return;
    
    const stored = localStorage.getItem('codex_contents');
    if (stored) {
      const allContents = JSON.parse(stored);
      
      // Extrair tópicos únicos para esta área e disciplina
      const topicos = new Map<string, Topico>();
      
      allContents.forEach((content: any) => {
        // Comparar case-insensitive e normalizando espaços
        const contentArea = content.area?.toLowerCase().trim() || '';
        const currentArea = this.area?.toLowerCase().trim() || '';
        const contentDisciplina = content.disciplina?.trim() || '';
        const currentDisciplina = this.disciplina?.nome?.trim() || '';
        
        if (contentArea === currentArea && contentDisciplina === currentDisciplina) {
          if (!topicos.has(content.topico)) {
            topicos.set(content.topico, {
              id: topicos.size + 1,
              nome: content.topico,
              descricao: '',
              duracao: `${content.duracao} min`,
              durationMinutos: content.duracao,
              concluido: false
            });
          }
        }
      });
      
      // Substituir tópicos hardcoded pelos do localStorage
      this.disciplina.topicos = Array.from(topicos.values());
    } else {
      // Se não houver conteúdo no localStorage, deixar array vazio
      this.disciplina.topicos = [];
    }
    
    // Calcular progresso baseado nos tópicos carregados
    this.atualizarProgresso();
  }

  formatarDuracao(minutos?: number): string {
    if (!minutos || minutos <= 0) return '--';
    
    const horas = Math.floor(minutos / 60);
    const mins = minutos % 60;
    
    if (horas === 0) {
      return `${mins}min`;
    } else if (mins === 0) {
      return `${horas}h`;
    } else {
      return `${horas}h ${mins}min`;
    }
  }

  atualizarProgresso(): void {
    if (!this.disciplina || this.disciplina.topicos.length === 0) {
      this.disciplina!.progresso = 0;
      return;
    }

    const totalTopicos = this.disciplina.topicos.length;
    const concluidosCount = this.disciplina.topicos.filter(t => t.concluido).length;
    this.disciplina.progresso = Math.round((concluidosCount / totalTopicos) * 100);
  }

  toggleTopico(topico: Topico): void {
    if (topico && this.disciplina) {
      topico.concluido = !topico.concluido;
      // Atualizar progresso
      this.atualizarProgresso();
    }
  }

  abrirTopico(topicoNome: string): void {
    if (this.disciplina) {
      this.router.navigate([`/dashboard/${this.area}/disciplina/${this.disciplinaId}/topico`, topicoNome], {
        state: { area: this.area, disciplina: this.disciplina.nome, disciplinaId: this.disciplinaId }
      });
    }
  }

  voltar(): void {
    this.router.navigate([`/dashboard/${this.area}`]);
  }

  voltarArea(): void {
    this.router.navigate([`/dashboard/${this.area}`]);
  }

  todosConcluidoS(): boolean {
    if (!this.disciplina) return false;
    return this.disciplina.topicos.every((t) => t.concluido);
  }

  getFilteredTopicos(): Topico[] {
    if (!this.disciplina) return [];
    if (!this.searchTopico.trim()) return this.disciplina.topicos;
    
    return this.disciplina.topicos.filter(t => 
      t.nome.toLowerCase().includes(this.searchTopico.toLowerCase())
    );
  }
}
