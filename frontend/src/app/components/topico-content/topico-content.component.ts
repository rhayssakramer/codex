import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';

interface Content {
  id: number;
  topico: string;
  area: string;
  disciplina: string;
  posicao: number;
  conteudo: string;
  dataCriacao: Date;
}

@Component({
  selector: 'app-topico-content',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './topico-content.component.html',
  styleUrls: ['./topico-content.component.css']
})
export class TopicoContentComponent implements OnInit {
  content: Content | null = null;
  area: string = '';
  disciplina: string = '';
  disciplinaId: number = 0;
  topicoNome: string = '';

  constructor(
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.topicoNome = params['topico'] || '';
      
      // Tentar obter area, disciplina e disciplinaId do state passado pela navegação
      const state = (window as any).history.state || {};
      
      console.log('State recebido:', state);
      console.log('Params:', params);
      
      if (state.area) {
        this.area = state.area;
        this.disciplina = state.disciplina || '';
        this.disciplinaId = state.disciplinaId || 0;
      } else {
        this.area = this.route.snapshot.data['area'] || 'fundamentos';
        this.disciplina = '';
        this.disciplinaId = 0;
      }
      
      console.log('Area definida como:', this.area);
      console.log('Disciplina definida como:', this.disciplina);
      console.log('DisciplinaId definida como:', this.disciplinaId);
      
      this.loadContent();
    });
  }

  loadContent(): void {
    const stored = localStorage.getItem('codex_contents');
    if (stored) {
      const allContents: Content[] = JSON.parse(stored);
      
      console.log('Procurando conteúdo com:');
      console.log('- area:', this.area);
      console.log('- topico:', this.topicoNome);
      console.log('- disciplina:', this.disciplina);
      console.log('Conteúdos disponíveis:', allContents);
      
      this.content = allContents.find(
        c => {
          const areaMatch = (c.area?.toLowerCase().trim() || '') === (this.area?.toLowerCase().trim() || '');
          const topicoMatch = c.topico === this.topicoNome;
          const disciplinaMatch = this.disciplina === '' || c.disciplina === this.disciplina;
          
          console.log(`Verificando: area=${areaMatch}, topico=${topicoMatch}, disciplina=${disciplinaMatch}`, c);
          
          return areaMatch && topicoMatch && disciplinaMatch;
        }
      ) || null;
      
      console.log('Conteúdo encontrado:', this.content);
    }
  }

  voltar(): void {
    // Voltar para a disciplina, não para a área
    if (this.disciplinaId > 0) {
      this.router.navigate([`/dashboard/${this.area}/disciplina/${this.disciplinaId}`]);
    } else {
      // Fallback caso disciplinaId não esteja disponível
      this.router.navigate([`/dashboard/${this.area}`]);
    }
  }

  parseContent(content: string): string {
    // Converter markdown básico para HTML
    let html = content
      .replace(/^# (.*?)$/gm, '<h1>$1</h1>')
      .replace(/^## (.*?)$/gm, '<h2>$1</h2>')
      .replace(/^### (.*?)$/gm, '<h3>$1</h3>')
      .replace(/^#### (.*?)$/gm, '<h4>$1</h4>')
      .replace(/\*\*(.*?)\*\*/g, '<strong>$1</strong>')
      .replace(/__(.*?)__/g, '<strong>$1</strong>')
      .replace(/\*(.*?)\*/g, '<em>$1</em>')
      .replace(/_(.*?)_/g, '<em>$1</em>')
      .replace(/\[([^\]]+)\]\(([^)]+)\)/g, '<a href="$2" target="_blank">$1</a>')
      .replace(/`([^`]+)`/g, '<code>$1</code>')
      .replace(/```(\w+)?\n([\s\S]*?)```/g, '<pre><code class="language-$1">$2</code></pre>')
      .replace(/^- (.*?)$/gm, '<li>$1</li>')
      .replace(/^\\d+\\. (.*?)$/gm, '<li>$1</li>')
      .replace(/(<li>.*?<\/li>)/s, '<ul>$1</ul>')
      .replace(/<\/ul>\n<ul>/g, '')
      .replace(/\n\n+/g, '</p><p>')
      .split('\n')
      .map(line => {
        if (!line.match(/^<[h|ul|pre]/)) {
          return line ? `<p>${line}</p>` : '';
        }
        return line;
      })
      .join('\n');
    
    return html;
  }
}
