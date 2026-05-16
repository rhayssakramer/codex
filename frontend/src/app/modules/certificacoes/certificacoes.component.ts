import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterOutlet, ActivatedRoute, NavigationEnd } from '@angular/router';
import { filter } from 'rxjs/operators';
import { DisciplinaService, Disciplina } from '../../services/disciplina.service';

@Component({
  selector: 'app-certificacoes',
  standalone: true,
  imports: [CommonModule, RouterOutlet],
  templateUrl: './certificacoes.component.html',
  styleUrls: ['./certificacoes.component.css']
})
export class CertificacoesComponent implements OnInit {
  disciplinas: (Disciplina & { progresso?: number })[] = [];
  visualizandoDisciplina: boolean = false;
  loading = true;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private disciplinaService: DisciplinaService
  ) {}

  ngOnInit(): void {
    this.disciplinaService.getDisciplinasByArea('certificacoes').subscribe(disciplinas => {
      this.disciplinas = disciplinas.map(d => ({ ...d, progresso: 0 }));
      this.loading = false;
    });

    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe(() => {
      if (this.route.firstChild) {
        this.route.firstChild.params.subscribe(params => {
          this.visualizandoDisciplina = Object.keys(params).length > 0;
        });
      } else {
        this.visualizandoDisciplina = false;
      }
    });
  }

  entrarDisciplina(disciplinaId: number): void {
    this.router.navigate(['/dashboard/certificacoes/disciplina', disciplinaId]);
  }
}
