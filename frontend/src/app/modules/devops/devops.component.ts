import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterOutlet, ActivatedRoute, NavigationEnd } from '@angular/router';
import { filter } from 'rxjs/operators';
import { DisciplinaService, Disciplina } from '../../services/disciplina.service';

@Component({
  selector: 'app-devops',
  standalone: true,
  imports: [CommonModule, RouterOutlet],
  templateUrl: './devops.component.html',
  styleUrls: ['./devops.component.css']
})
export class DevopsComponent implements OnInit {
  disciplinas: (Disciplina & { progresso?: number })[] = [];
  visualizandoDisciplina: boolean = false;
  loading = true;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private disciplinaService: DisciplinaService
  ) {}

  ngOnInit(): void {
    this.disciplinaService.getDisciplinasByArea('devops').subscribe(disciplinas => {
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
    this.router.navigate(['/dashboard/devops/disciplina', disciplinaId]);
  }
}
