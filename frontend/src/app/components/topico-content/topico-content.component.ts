import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import { TopicoService, Topico } from '../../services/topico.service';
import html2canvas from 'html2canvas';
import jsPDF from 'jspdf';

interface Subtopic {
  id: number;
  titulo: string;
  descricao: string;
  progresso: number;
  icone?: string;
}

interface Content {
  id: number;
  topico: string;
  area: string;
  disciplina: string;
  posicao: number;
  conteudo: string;
  dataCriacao: Date;
  subtopicos?: Subtopic[];
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
  topicoId: number = 0;
  topicoData: Topico | null = null;
  showDownloadMenu: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private topicoService: TopicoService,
    private sanitizer: DomSanitizer
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.topicoNome = params['topico'] || '';
      this.topicoId = parseInt(params['topico']) || 0;
      
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
      console.log('TopicoId definido como:', this.topicoId);
      
      // Se temos ID de tópico válido (e maior que ano 2020 = timestamp do localStorage)
      // então é um tópico criado no admin (com Date.now() como ID)
      if (this.topicoId > 0) {
        // Verificar se é timestamp (> 1.5 bilhões = 2017+)
        if (this.topicoId > 1500000000) {
          console.log('🔍 Tópico do localStorage (timestamp):', this.topicoId);
          this.loadContentByLocalStorageId();
        } else {
          console.log('📡 Tópico da API (ID normal):', this.topicoId);
          this.loadTopicoFromAPI();
        }
      } else {
        this.loadContent();
      }
    });
  }

  loadTopicoFromAPI(): void {
    console.log(`📡 TopicoContentComponent: Chamando API para buscar tópico ID ${this.topicoId}`);
    this.topicoService.getTopicoById(this.topicoId).subscribe({
      next: (topico: Topico | null) => {
        if (topico) {
          console.log('✅ TopicoContentComponent: Tópico carregado da API:', topico);
          this.topicoData = topico;
          this.displayTopico(topico);
        } else {
          console.error('❌ TopicoContentComponent: Tópico não encontrado (null)');
          this.loadContent();
        }
      },
      error: (err) => {
        console.error('❌ TopicoContentComponent: Erro ao carregar tópico:', err);
        this.loadContent();
      }
    });
  }

  displayTopico(topico: Topico): void {
    console.log('🎨 TopicoContentComponent: Exibindo tópico:', topico);
    // Converter dados da API para o formato esperado
    this.content = {
      id: topico.id,
      topico: topico.titulo.toLowerCase().replace(/\s+/g, '-'),
      area: this.area,
      disciplina: this.disciplina,
      posicao: topico.ordem,
      conteudo: topico.conteudo,
      dataCriacao: new Date(),
      subtopicos: []
    };
    console.log('✅ TopicoContentComponent: Content setado:', this.content);
  }

  loadContent(): void {
    // Apenas carregar conteúdo do localStorage (nenhum mock data automático)
    const stored = localStorage.getItem('codex_contents');
    const allContents: Content[] = stored ? JSON.parse(stored) : [];
    
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

  loadContentByLocalStorageId(): void {
    // Carregar tópico do localStorage pelo ID (timestamp)
    const stored = localStorage.getItem('codex_contents');
    const allContents: Content[] = stored ? JSON.parse(stored) : [];
    
    console.log('🔍 Procurando tópico no localStorage com ID:', this.topicoId);
    console.log('📦 Conteúdos disponíveis:', allContents);
    
    this.content = allContents.find(c => c.id === this.topicoId) || null;
    
    if (this.content) {
      console.log('✅ Tópico encontrado no localStorage:', this.content);
      console.log('📝 Conteúdo markdown:', this.content.conteudo?.substring(0, 100) + '...');
    } else {
      console.error('❌ Tópico não encontrado no localStorage com ID:', this.topicoId);
      console.error('📋 IDs disponíveis no localStorage:', allContents.map(c => c.id));
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
    console.log('Parsing content:', content);
    
    // Converter markdown básico para HTML
    let html = content
      .replace(/^# (.*?)$/gm, '<h1>$1</h1>')
      .replace(/^## (.*?)$/gm, '<h2>$1</h2>')
      .replace(/^### (.*?)$/gm, '<h3>$1</h3>')
      .replace(/^#### (.*?)$/gm, '<h4>$1</h4>')
      .replace(/\*\*(.*?)\*\*/g, '<strong>$1</strong>')
      .replace(/__(.*)__/g, '<strong>$1</strong>')
      .replace(/\*(.*?)\*/g, '<em>$1</em>')
      .replace(/_(.*?)_/g, '<em>$1</em>');
    
    // Imagens com tamanho: ![alt|WxH](url) - ANTES de imagens normais
    html = html.replace(/!\[([^\]|]+)\|(\d+)x(\d+)\]\(([^)]+)\)/g, (match, alt, width, height, url) => {
      console.log('Found image with size:', {alt, width, height, url});
      return `<img src="${url}" alt="${alt}" style="width: ${width}px; height: ${height}px; object-fit: contain; border-radius: 8px; margin: 15px 0;">`;
    });
    
    // Imagens normais: ![alt](url)
    html = html.replace(/!\[([^\]]*)\]\(([^)]+)\)/g, (match, alt, url) => {
      console.log('Found image:', {alt, url});
      return `<img src="${url}" alt="${alt}" style="max-width: 100%; height: auto; border-radius: 8px; margin: 15px 0;">`;
    });
    
    html = html
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
        if (!line.match(/^<[h|ul|pre|img]/)) {
          return line ? `<p>${line}</p>` : '';
        }
        return line;
      })
      .join('\n');
    
    return html;
  }

  getSafeHtml(content: string): SafeHtml {
    return this.sanitizer.bypassSecurityTrustHtml(this.parseContent(content));
  }

  toggleDownloadMenu(): void {
    this.showDownloadMenu = !this.showDownloadMenu;
    console.log('Menu aberto:', this.showDownloadMenu);
  }

  downloadMarkdown(): void {
    this.showDownloadMenu = false;
    
    if (!this.content) return;

    // Criar o conteúdo do arquivo
    const filename = `${this.content.topico}.md`;
    const content = this.content.conteudo;
    
    // Criar um Blob com o conteúdo
    const blob = new Blob([content], { type: 'text/markdown;charset=utf-8;' });
    
    // Criar um link temporário e fazer o download
    const link = document.createElement('a');
    const url = URL.createObjectURL(blob);
    
    link.setAttribute('href', url);
    link.setAttribute('download', filename);
    link.style.visibility = 'hidden';
    
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
    
    console.log(`✅ Arquivo "${filename}" baixado com sucesso!`);
  }

  downloadPDF(): void {
    this.showDownloadMenu = false;
    
    if (!this.content) return;

    const element = document.querySelector('.markdown-content') as HTMLElement;
    
    if (!element) {
      console.error('Elemento de conteúdo não encontrado');
      return;
    }

    console.log('Iniciando geração de PDF...');

    // Clone o elemento
    const clonedElement = element.cloneNode(true) as HTMLElement;
    
    // Aplicar estilos ao elemento clonado
    clonedElement.style.padding = '15px';
    clonedElement.style.fontFamily = 'Arial, sans-serif';
    clonedElement.style.lineHeight = '1.6';
    clonedElement.style.color = '#333';
    clonedElement.style.fontSize = '12px';
    clonedElement.style.backgroundColor = '#ffffff';
    clonedElement.style.width = '210mm';
    
    // Melhorar estilos de headings
    clonedElement.querySelectorAll('h1').forEach(heading => {
      (heading as HTMLElement).style.marginTop = '15px';
      (heading as HTMLElement).style.marginBottom = '8px';
      (heading as HTMLElement).style.fontWeight = 'bold';
      (heading as HTMLElement).style.fontSize = '18px';
      (heading as HTMLElement).style.color = '#1a1a1a';
    });

    clonedElement.querySelectorAll('h2, h3, h4, h5, h6').forEach(heading => {
      (heading as HTMLElement).style.marginTop = '12px';
      (heading as HTMLElement).style.marginBottom = '6px';
      (heading as HTMLElement).style.fontWeight = 'bold';
      (heading as HTMLElement).style.color = '#1a1a1a';
    });

    // Melhorar estilos de parágrafos
    clonedElement.querySelectorAll('p').forEach(p => {
      (p as HTMLElement).style.marginBottom = '10px';
      (p as HTMLElement).style.lineHeight = '1.6';
      (p as HTMLElement).style.fontSize = '12px';
    });

    // Melhorar estilos de imagens
    clonedElement.querySelectorAll('img').forEach(img => {
      (img as HTMLElement).style.maxWidth = '100%';
      (img as HTMLElement).style.height = 'auto';
      (img as HTMLElement).style.marginTop = '10px';
      (img as HTMLElement).style.marginBottom = '10px';
      (img as HTMLElement).style.display = 'block';
      (img as HTMLElement).style.borderRadius = '4px';
    });

    // Melhorar listas
    clonedElement.querySelectorAll('ul, ol').forEach(list => {
      (list as HTMLElement).style.marginLeft = '15px';
      (list as HTMLElement).style.marginBottom = '10px';
      (list as HTMLElement).style.fontSize = '12px';
    });

    clonedElement.querySelectorAll('li').forEach(item => {
      (item as HTMLElement).style.marginBottom = '4px';
      (item as HTMLElement).style.fontSize = '12px';
    });

    // Melhorar código
    clonedElement.querySelectorAll('code').forEach(code => {
      (code as HTMLElement).style.backgroundColor = '#f5f5f5';
      (code as HTMLElement).style.padding = '2px 4px';
      (code as HTMLElement).style.borderRadius = '3px';
      (code as HTMLElement).style.fontFamily = 'monospace';
      (code as HTMLElement).style.fontSize = '11px';
      (code as HTMLElement).style.color = '#d63384';
    });

    clonedElement.querySelectorAll('pre').forEach(pre => {
      (pre as HTMLElement).style.padding = '10px';
      (pre as HTMLElement).style.marginBottom = '10px';
      (pre as HTMLElement).style.backgroundColor = '#f5f5f5';
      (pre as HTMLElement).style.borderRadius = '4px';
      (pre as HTMLElement).style.fontSize = '11px';
      (pre as HTMLElement).style.overflowX = 'auto';
    });

    // Converter imagens para base64
    const images = Array.from(clonedElement.querySelectorAll('img')) as HTMLImageElement[];
    let loadedImages = 0;
    let totalImages = images.length;

    console.log('Total de imagens encontradas:', totalImages);

    const processImages = () => {
      if (totalImages === 0) {
        this.generatePDFFromElement(clonedElement);
        return;
      }

      images.forEach((img, index) => {
        const src = img.src;
        console.log(`Processando imagem ${index + 1}/${totalImages}: ${src}`);
        
        // Usar Canvas para carregar e converter a imagem
        const canvas = document.createElement('canvas');
        const ctx = canvas.getContext('2d');
        const tempImg = new Image();
        tempImg.crossOrigin = 'anonymous';
        
        tempImg.onload = () => {
          try {
            canvas.width = tempImg.width;
            canvas.height = tempImg.height;
            ctx?.drawImage(tempImg, 0, 0);
            const base64 = canvas.toDataURL('image/jpeg', 0.95);
            img.src = base64;
            loadedImages++;
            console.log(`Imagem ${index + 1} convertida (${loadedImages}/${totalImages})`);
            if (loadedImages === totalImages) {
              this.generatePDFFromElement(clonedElement);
            }
          } catch (error) {
            console.warn(`Erro ao converter imagem ${index + 1}:`, error);
            loadedImages++;
            if (loadedImages === totalImages) {
              this.generatePDFFromElement(clonedElement);
            }
          }
        };
        
        tempImg.onerror = () => {
          console.warn(`Erro ao carregar imagem ${index + 1}: ${src}`);
          loadedImages++;
          if (loadedImages === totalImages) {
            this.generatePDFFromElement(clonedElement);
          }
        };
        
        tempImg.src = src;
      });
    };

    processImages();
  }

  private generatePDFFromElement(clonedElement: HTMLElement): void {
    console.log('Convertendo elemento para PDF...');
    
    // Criar container temporário
    const container = document.createElement('div');
    container.style.position = 'fixed';
    container.style.left = '-9999px';
    container.style.top = '-9999px';
    container.style.backgroundColor = '#ffffff';
    container.style.width = '210mm';
    container.appendChild(clonedElement);
    document.body.appendChild(container);

    // Usar html2canvas para capturar o elemento como imagem
    html2canvas(clonedElement, {
      scale: 1.5,
      allowTaint: true,
      useCORS: true,
      backgroundColor: '#ffffff',
      logging: false,
      windowHeight: clonedElement.scrollHeight
    }).then(canvas => {
      // Remover container temporário
      document.body.removeChild(container);

      const imgWidth = 210; // A4 width em mm
      const imgHeight = (canvas.height * imgWidth) / canvas.width;
      let heightLeft = imgHeight;
      let position = 0;

      const imgData = canvas.toDataURL('image/jpeg', 0.95);
      const pdf = new jsPDF('p', 'mm', 'a4');
      const pageHeight = pdf.internal.pageSize.getHeight();
      
      // Primeira página
      pdf.addImage(imgData, 'JPEG', 0, position, imgWidth, imgHeight);
      heightLeft -= pageHeight;

      // Próximas páginas se necessário
      while (heightLeft > 0) {
        position = heightLeft - imgHeight;
        pdf.addPage();
        pdf.addImage(imgData, 'JPEG', 0, position, imgWidth, imgHeight);
        heightLeft -= pageHeight;
      }

      pdf.save(`${this.content?.topico || 'documento'}.pdf`);
      console.log(`✅ PDF gerado com sucesso!`);
    }).catch(error => {
      // Remover container em caso de erro
      if (container.parentNode) {
        document.body.removeChild(container);
      }
      console.error('Erro ao gerar PDF:', error);
    });
  }
}
