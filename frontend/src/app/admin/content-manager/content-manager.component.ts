import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

interface Content {
  id: number;
  topico: string;
  area: string;
  disciplina: string;
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

  areas = ['Fundamentos', 'Frontend', 'Backend', 'DevOps', 'Certificações'];
  
  disciplinasByArea: { [key: string]: string[] } = {
    'Fundamentos': ['Lógica de Programação', 'Algoritmos', 'Estruturas de Dados', 'Versionamento com Git', 'Terminal e Command Line'],
    'Frontend': ['HTML & CSS', 'JavaScript Essencial', 'React', 'Angular', 'Vue.js', 'TypeScript'],
    'Backend': ['Node.js & Express', 'Python & Django', 'Java & Spring Boot', 'Bancos de Dados SQL', 'Bancos NoSQL', 'APIs RESTful'],
    'DevOps': ['Docker & Containers', 'Kubernetes', 'CI/CD', 'AWS', 'Terraform', 'Monitoring & Logs'],
    'Certificações': ['JavaScript Certificado', 'React Advanced', 'AWS Solutions Architect', 'Google Cloud Professional', 'Kubernetes Associate']
  };

  newContent: Content = {
    id: 0,
    topico: '',
    area: '',
    disciplina: '',
    posicao: 1,
    conteudo: '',
    duracao: 30,
    dataCriacao: new Date()
  };

  constructor(private router: Router) {}

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
    // Converter markdown básico para HTML
    let html = content
      .replace(/^# (.*?)$/gm, '<h1>$1</h1>')
      .replace(/^## (.*?)$/gm, '<h2>$1</h2>')
      .replace(/^### (.*?)$/gm, '<h3>$1</h3>')
      .replace(/\*\*(.*?)\*\*/g, '<strong>$1</strong>')
      .replace(/\*(.*?)\*/g, '<em>$1</em>')
      .replace(/\n\n/g, '</p><p>')
      .replace(/^- (.*?)$/gm, '<li>$1</li>')
      .replace(/(<li>.*?<\/li>)/s, '<ul>$1</ul>')
      .replace(/<\/ul>\n<ul>/g, '');
    
    return '<p>' + html + '</p>';
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

  saveToLocalStorage(): void {
    localStorage.setItem('codex_contents', JSON.stringify(this.contents));
  }
}
