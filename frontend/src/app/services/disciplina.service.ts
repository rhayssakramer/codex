import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { map, catchError } from 'rxjs/operators';

export interface Disciplina {
  id: number;
  areaId: number;
  nome: string;
  descricao: string;
  imagem?: string;
  ordem: number;
  ativo: boolean;
  totalTopicos: number;
  topicosConcluidos: number;
}

interface ApiResponse<T> {
  sucesso: boolean;
  dados: T;
  mensagem?: string;
}

// Mapeamento de nome da área para ID no banco
const AREA_ID_MAP: { [key: string]: number } = {
  'fundamentos': 1,
  'frontend': 2,
  'backend': 3,
  'devops': 4,
  'certificacoes': 5
};

@Injectable({
  providedIn: 'root'
})
export class DisciplinaService {
  private apiUrl = '/api/disciplinas';

  constructor(private http: HttpClient) {}

  /**
   * Busca disciplinas por nome da área (ex: 'fundamentos', 'frontend', etc.)
   */
  getDisciplinasByArea(areaNome: string): Observable<Disciplina[]> {
    const areaId = AREA_ID_MAP[areaNome.toLowerCase()];
    if (!areaId) {
      console.warn(`Área não encontrada: ${areaNome}`);
      return of([]);
    }
    return this.getDisciplinasByAreaId(areaId);
  }

  /**
   * Busca disciplinas por ID da área
   */
  getDisciplinasByAreaId(areaId: number): Observable<Disciplina[]> {
    return this.http.get<ApiResponse<Disciplina[]>>(`${this.apiUrl}/area/${areaId}`).pipe(
      map(response => {
        if (response.sucesso && response.dados) {
          return response.dados;
        }
        return [];
      }),
      catchError(error => {
        console.error('Erro ao buscar disciplinas:', error);
        return of([]);
      })
    );
  }

  /**
   * Busca disciplina por ID
   */
  getDisciplinaById(id: number): Observable<Disciplina | null> {
    return this.http.get<ApiResponse<Disciplina>>(`${this.apiUrl}/${id}`).pipe(
      map(response => {
        if (response.sucesso && response.dados) {
          return response.dados;
        }
        return null;
      }),
      catchError(() => of(null))
    );
  }
}
