import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import { UploadService } from '../../services/upload.service';
import { ToasterService } from '../../services/toaster.service';

interface Content {
  id: number;
  topico: string;
  area: string;
  disciplina: string;
  disciplinaId?: number; // ID da disciplina para navegação
  posicao: number;
  conteudo: string;
  duracao: number; // em minutos
  dataCriacao: Date;
}

@Component({
  selector: 'app-content-manager',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './content-manager.component.html',
  styleUrls: ['./content-manager.component.css']
})
export class ContentManagerComponent implements OnInit {
  contents: Content[] = [];
  showForm = false;
  editingId: number | null = null;
  editingContent: Content | null = null;
  viewingContent: Content | null = null;
  deletingContent: Content | null = null;
  searchFilter: string = '';

  areas = ['Fundamentos', 'Frontend', 'Backend', 'DevOps', 'Certificações', 'Controle de Versão'];
  
  disciplinasByArea: { [key: string]: string[] } = {
    'Fundamentos': ['Lógica de Programação', 'Estruturas de Dados'],
    'Frontend': ['HTML & CSS', 'JavaScript'],
    'Backend': ['C# & .NET', 'C#', '.NET'],
    'DevOps': [],
    'Certificações': ['AZ-900', 'AI-900', 'GH-900', 'GH-300'],
    'Controle de Versão': ['Git', 'GitHub']
  };

  // Mapa de disciplinas para IDs (do seed data da API)
  disciplinaIdMap: { [key: string]: number } = {
    'Lógica de Programação': 1,
    'Estruturas de Dados': 2,
    'HTML & CSS': 3,
    'JavaScript': 4,
    'C# & .NET': 5,
    'C#': 6,
    '.NET': 7,
    'AZ-900': 8,
    'AI-900': 9,
    'GH-900': 10,
    'GH-300': 11,
    'Git': 12,
    'GitHub': 13,
    'Operações': 1  // Fallback para ID 1
  };

  newContent: Content = {
    id: 0,
    topico: '',
    area: '',
    disciplina: '',
    disciplinaId: 1,
    posicao: 1,
    conteudo: '',
    duracao: 30,
    dataCriacao: new Date()
  };

  uploadingImage = false;
  imagePreview: string | null = null;
  expandedEditor = false;
  showImageSizeModal = false;
  pendingImageUrl: string | null = null;
  pendingImageName: string | null = null;
  imageWidth: number = 600;
  isEditingContent = false;

  constructor(
    private router: Router,
    private uploadService: UploadService,
    private toaster: ToasterService,
    private sanitizer: DomSanitizer
  ) {}

  ngOnInit(): void {
    // Carregar conteúdos do localStorage
    const stored = localStorage.getItem('codex_contents');
    if (stored) {
      this.contents = JSON.parse(stored);
    }
  }

  selectArea(area: string): void {
    this.newContent.area = area;
    this.newContent.disciplina = this.disciplinasByArea[area][0];
    // Definir o ID da disciplina
    this.newContent.disciplinaId = this.disciplinaIdMap[this.newContent.disciplina] || 1;
  }

  selectDisciplina(disciplina: string): void {
    this.newContent.disciplina = disciplina;
    // Atualizar o ID da disciplina quando a disciplina muda
    this.newContent.disciplinaId = this.disciplinaIdMap[disciplina] || 1;
  }

  openForm(): void {
    this.showForm = true;
    this.editingId = null;
    this.newContent = {
      id: 0,
      topico: '',
      area: '',
      disciplina: '',
      posicao: 1,
      conteudo: '',
      duracao: 30,
      dataCriacao: new Date()
    };
  }

  editContent(content: Content): void {
    this.newContent = { ...content };
    this.editingId = content.id;
    this.showForm = true;
    this.viewingContent = null;
  }

  viewContent(content: Content): void {
    this.viewingContent = content;
  }

  closeViewModal(): void {
    this.viewingContent = null;
  }

  saveContent(): void {
    if (!this.newContent.area || !this.newContent.disciplina || !this.newContent.topico || !this.newContent.conteudo) {
      alert('Preencha todos os campos obrigatórios!');
      return;
    }

    if (this.editingId) {
      const index = this.contents.findIndex(c => c.id === this.editingId);
      if (index !== -1) {
        this.contents[index] = { ...this.newContent, id: this.editingId };
      }
    } else {
      this.newContent.id = Date.now();
      this.contents.push({ ...this.newContent });
    }

    localStorage.setItem('codex_contents', JSON.stringify(this.contents));
    this.showForm = false;
    this.resetForm();
  }

  deleteContent(id: number): void {
    if (confirm('Deseja deletar este conteúdo?')) {
      this.contents = this.contents.filter(c => c.id !== id);
      localStorage.setItem('codex_contents', JSON.stringify(this.contents));
    }
  }

  resetForm(): void {
    this.newContent = {
      id: 0,
      topico: '',
      area: '',
      disciplina: '',
      posicao: 1,
      conteudo: '',
      duracao: 30,
      dataCriacao: new Date()
    };
  }

  voltar(): void {
    this.router.navigate(['/dashboard/fundamentos']);
  }

  parseContent(content: string): string {
    console.log('Parsing content:', content);
    
    // Converter markdown básico para HTML
    let html = content
      .replace(/^# (.*?)$/gm, '<h1>$1</h1>')
      .replace(/^## (.*?)$/gm, '<h2>$1</h2>')
      .replace(/^### (.*?)$/gm, '<h3>$1</h3>')
      .replace(/\*\*(.*?)\*\*/g, '<strong>$1</strong>')
      .replace(/\*(.*?)\*/g, '<em>$1</em>');
    
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
      .replace(/\n\n/g, '</p><p>')
      .replace(/^- (.*?)$/gm, '<li>$1</li>')
      .replace(/(<li>.*?<\/li>)/s, '<ul>$1</ul>')
      .replace(/<\/ul>\n<ul>/g, '');
    
    return '<p>' + html + '</p>';
  }

  getSafeHtml(content: string): SafeHtml {
    return this.sanitizer.bypassSecurityTrustHtml(this.parseContent(content));
  }

  getFilteredContents(): Content[] {
    if (!this.searchFilter.trim()) {
      return this.contents;
    }

    const filter = this.searchFilter.toLowerCase().trim();
    return this.contents.filter(content =>
      content.area.toLowerCase().includes(filter) ||
      content.disciplina.toLowerCase().includes(filter) ||
      content.topico.toLowerCase().includes(filter)
    );
  }

  openCreateModal(): void {
    this.editingContent = null;
    this.showForm = true;
  }

  openEditModal(content: Content): void {
    this.editingContent = { ...content };
  }

  closeEditModal(): void {
    this.editingContent = null;
  }

  saveEditedContent(): void {
    if (this.editingContent) {
      const index = this.contents.findIndex(c => c.id === this.editingContent!.id);
      if (index > -1) {
        this.contents[index] = this.editingContent;
        this.saveToLocalStorage();
        this.closeEditModal();
      }
    }
  }

  openDeleteModal(content: Content): void {
    this.deletingContent = content;
  }

  closeDeleteModal(): void {
    this.deletingContent = null;
  }

  confirmDelete(): void {
    if (this.deletingContent) {
      this.deleteContent(this.deletingContent.id);
      this.closeDeleteModal();
    }
  }

  closeForm(): void {
    this.showForm = false;
    this.newContent = {
      id: 0,
      topico: '',
      area: '',
      disciplina: '',
      posicao: 1,
      conteudo: '',
      duracao: 30,
      dataCriacao: new Date()
    };
  }

  saveToLocalStorage(): void {
    localStorage.setItem('codex_contents', JSON.stringify(this.contents));
  }

  onImageSelected(event: any): void {
    const file = event.target.files[0];
    if (!file) return;

    // Validar tipo de arquivo
    if (!file.type.startsWith('image/')) {
      this.toaster.error('Selecione um arquivo de imagem válido (JPG, PNG, GIF, WebP)');
      return;
    }

    // Validar tamanho (5MB)
    if (file.size > 5 * 1024 * 1024) {
      this.toaster.error('A imagem não pode exceder 5MB');
      return;
    }

    // Mostrar preview
    const reader = new FileReader();
    reader.onload = (e: any) => {
      this.imagePreview = e.target.result;
    };
    reader.readAsDataURL(file);

    // Fazer upload
    this.uploadingImage = true;
    this.toaster.info('Enviando imagem...');
    this.isEditingContent = false;
    
    this.uploadService.uploadImage(file).subscribe({
      next: (response: any) => {
        if (response.success) {
          this.uploadingImage = false;
          // Converter URL relativa para URL completa do backend
          const imageUrl = response.url.startsWith('http') ? response.url : `http://localhost:5000${response.url}`;
          this.pendingImageUrl = imageUrl;
          this.pendingImageName = file.name;
          this.imageWidth = 600;
          this.showImageSizeModal = true;
          this.toaster.success('✓ Imagem enviada! Escolha o tamanho.');
          
          // Resetar input
          const input = document.querySelector('input[type="file"]') as HTMLInputElement;
          if (input) input.value = '';
          
          // Limpar preview após 2 segundos
          setTimeout(() => {
            this.imagePreview = null;
          }, 2000);
        }
      },
      error: (err) => {
        this.uploadingImage = false;
        const errorMsg = err.error?.message || err.message || 'Erro desconhecido ao fazer upload';
        this.toaster.error(`✗ Erro ao fazer upload: ${errorMsg}`);
        console.error('Erro de upload:', err);
      }
    });
  }

  insertImageWithSize(): void {
    if (!this.pendingImageUrl || !this.pendingImageName) return;
    
    // Calcular altura mantendo proporção (4:3)
    const height = Math.round((this.imageWidth / 4) * 3);
    const imageMarkdown = `![${this.pendingImageName}|${this.imageWidth}x${height}](${this.pendingImageUrl})`;
    
    if (this.isEditingContent && this.editingContent) {
      this.editingContent.conteudo += `\n${imageMarkdown}\n`;
    } else if (this.newContent) {
      this.newContent.conteudo += `\n${imageMarkdown}\n`;
    }
    
    this.showImageSizeModal = false;
    this.pendingImageUrl = null;
    this.pendingImageName = null;
  }

  closeImageSizeModal(): void {
    this.showImageSizeModal = false;
    this.pendingImageUrl = null;
    this.pendingImageName = null;
  }

  onImageSelectedEdit(event: any): void {
    const file = event.target.files[0];
    if (!file) return;

    // Validações
    if (!file.type.startsWith('image/')) {
      this.toaster.error('Selecione um arquivo de imagem válido (JPG, PNG, GIF, WebP)');
      return;
    }

    if (file.size > 5 * 1024 * 1024) {
      this.toaster.error('A imagem não pode exceder 5MB');
      return;
    }

    // Preview
    const reader = new FileReader();
    reader.onload = (e: any) => {
      this.imagePreview = e.target.result;
    };
    reader.readAsDataURL(file);

    // Fazer upload
    this.uploadingImage = true;
    this.toaster.info('Enviando imagem...');
    this.isEditingContent = true;
    
    this.uploadService.uploadImage(file).subscribe({
      next: (response: any) => {
        if (response.success) {
          this.uploadingImage = false;
          // Converter URL relativa para URL completa do backend
          const imageUrl = response.url.startsWith('http') ? response.url : `http://localhost:5000${response.url}`;
          this.pendingImageUrl = imageUrl;
          this.pendingImageName = file.name;
          this.imageWidth = 600;
          this.showImageSizeModal = true;
          this.toaster.success('✓ Imagem enviada! Escolha o tamanho.');
          
          // Resetar input
          const input = document.querySelector('input[type="file"]') as HTMLInputElement;
          if (input) input.value = '';
          
          // Limpar preview após 2 segundos
          setTimeout(() => {
            this.imagePreview = null;
          }, 2000);
        }
      },
      error: (err) => {
        this.uploadingImage = false;
        const errorMsg = err.error?.message || err.message || 'Erro desconhecido ao fazer upload';
        this.toaster.error(`✗ Erro ao fazer upload: ${errorMsg}`);
        console.error('Erro de upload:', err);
      }
    });
  }

  toggleExpandedEditor(): void {
    this.expandedEditor = !this.expandedEditor;
  }

  closeExpandedEditor(): void {
    this.expandedEditor = false;
  }
}
