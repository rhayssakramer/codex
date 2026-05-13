import { Component, OnInit, ViewChild, ViewContainerRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterOutlet, NavigationEnd } from '@angular/router';
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
  visualizandoTopico: boolean = false;

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
        id: 1,
        nome: 'HTML & CSS',
        descricao: 'Domine a estrutura e estilo de páginas web',
        progresso: 90,
        topicos: [
          { id: 1, nome: 'HTML Semântico', descricao: 'Tags e estrutura', duracao: '20 min', concluido: true },
          { id: 2, nome: 'CSS Básico', descricao: 'Seletores e propriedades', duracao: '25 min', concluido: true },
          { id: 3, nome: 'Flexbox', descricao: 'Layout moderno', duracao: '25 min', concluido: true },
          { id: 4, nome: 'Grid CSS', descricao: 'Layouts avançados', duracao: '30 min', concluido: true }
        ]
      },
      {
        id: 2,
        nome: 'JavaScript Essencial',
        descricao: 'JavaScript puro e boas práticas',
        progresso: 85,
        topicos: [
          { id: 1, nome: 'Variáveis e Tipos', descricao: 'var, let, const', duracao: '20 min', concluido: true },
          { id: 2, nome: 'Funções', descricao: 'Declaração e expressão', duracao: '20 min', concluido: true },
          { id: 3, nome: 'DOM API', descricao: 'Manipulação de elementos', duracao: '25 min', concluido: true },
          { id: 4, nome: 'Async/Await', descricao: 'Operações assíncronas', duracao: '25 min', concluido: false }
        ]
      }
    ],
    backend: [
      {
        id: 1,
        nome: 'Node.js & Express',
        descricao: 'Crie servidores web com JavaScript',
        progresso: 78,
        topicos: [
          { id: 1, nome: 'Node.js Basics', descricao: 'Modules e npm', duracao: '25 min', concluido: true },
          { id: 2, nome: 'Express Setup', descricao: 'Criando servidores', duracao: '20 min', concluido: true },
          { id: 3, nome: 'Rotas e Middleware', descricao: 'Requisições e respostas', duracao: '25 min', concluido: true },
          { id: 4, nome: 'Banco de Dados', descricao: 'Integração com BD', duracao: '30 min', concluido: false }
        ]
      }
    ],
    devops: [
      {
        id: 1,
        nome: 'Docker & Containers',
        descricao: 'Containerização de aplicações',
        progresso: 85,
        topicos: [
          { id: 1, nome: 'Introdução', descricao: 'O que é Docker', duracao: '15 min', concluido: true },
          { id: 2, nome: 'Imagens', descricao: 'Criando Dockerfiles', duracao: '25 min', concluido: true },
          { id: 3, nome: 'Containers', descricao: 'Executando e gerenciando', duracao: '20 min', concluido: true },
          { id: 4, nome: 'Docker Compose', descricao: 'Multi-containers', duracao: '25 min', concluido: false }
        ]
      }
    ],
    certificacoes: [
      {
        id: 1,
        nome: 'JavaScript Certificado',
        descricao: 'Certificação oficial de JavaScript',
        progresso: 88,
        topicos: [
          { id: 1, nome: 'Módulo 1', descricao: 'Fundamentos JS', duracao: '60 min', concluido: true },
          { id: 2, nome: 'Módulo 2', descricao: 'Avançado', duracao: '60 min', concluido: true },
          { id: 3, nome: 'Teste Prático', descricao: 'Exercícios', duracao: '90 min', concluido: false }
        ]
      }
    ]
  };

  constructor(
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.area = this.route.snapshot.data['area'] || 'fundamentos';
      this.disciplinaId = +params['id'];
      this.loadDisciplina();
    });
    
    // Monitorar eventos de navegação para detectar quando há rota filha
    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe(() => {
      // Verificar se há uma rota filha ativa
      if (this.route.firstChild) {
        this.route.firstChild.params.subscribe(p => {
          this.visualizandoTopico = Object.keys(p).length > 0;
        });
      } else {
        this.visualizandoTopico = false;
      }
    });
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
