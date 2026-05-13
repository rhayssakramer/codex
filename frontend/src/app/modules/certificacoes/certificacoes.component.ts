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
  selector: 'app-certificacoes',
  standalone: true,
  imports: [CommonModule, RouterOutlet],
  templateUrl: './certificacoes.component.html',
  styleUrls: ['./certificacoes.component.css']
})
export class CertificacoesComponent implements OnInit {
  certificacoes: Disciplina[] = [
    {
      id: 1,
      nome: 'JavaScript Certificado',
      descricao: 'Certificação oficial de JavaScript',
      progresso: 0
    },
    {
      id: 2,
      nome: 'React Advanced',
      descricao: 'Certificação avançada em React',
      progresso: 0
    },
    {
      id: 3,
      nome: 'AWS Solutions Architect',
      descricao: 'Certificação profissional AWS',
      progresso: 0
    },
    {
      id: 4,
      nome: 'Google Cloud Professional',
      descricao: 'Certificação Google Cloud',
      progresso: 0
    },
    {
      id: 5,
      nome: 'Kubernetes Associate',
      descricao: 'Certificação CKA (Certified Kubernetes Administrator)',
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

    this.certificacoes.forEach(disciplina => {
      const topicosUnicos = new Set<string>();

      allContents.forEach((content: any) => {
        const contentArea = content.area?.toLowerCase().trim() || '';
        const contentDisciplina = content.disciplina?.trim() || '';
        
        if (contentArea === 'certificacoes' && contentDisciplina === disciplina.nome) {
          topicosUnicos.add(content.topico);
        }
      });

      if (topicosUnicos.size > 0) {
        disciplina.progresso = 0;
      }
    });
  }

  entrarDisciplina(certificacaoId: number): void {
    this.router.navigate(['/dashboard/certificacoes/disciplina', certificacaoId]);
  }
}
