import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

export interface Topico {
  id: number;
  titulo: string;
  conteudo: string;
  disciplinaId: number;
  codigoExemplo?: string;
  videoUrl?: string;
  ordem: number;
  dificuldade: number;
}

interface ApiResponse<T> {
  sucesso: boolean;
  dados: T;
  mensagem?: string;
}

@Injectable({
  providedIn: 'root'
})
export class TopicoService {
  private apiUrl = '/api/topicos';

  constructor(private http: HttpClient) {}

  /**
   * Busca um tópico pelo ID
   */
  getTopicoById(id: number): Observable<Topico | null> {
    console.log(`🔍 TopicoService: Buscando tópico com ID ${id} em ${this.apiUrl}/${id}`);
    return this.http.get<ApiResponse<Topico>>(`${this.apiUrl}/${id}`).pipe(
      map(response => {
        console.log(`✅ TopicoService: Resposta da API:`, response);
        if (response.sucesso && response.dados) {
          console.log(`✅ TopicoService: Tópico encontrado:`, response.dados);
          return response.dados;
        }
        console.warn(`⚠️ TopicoService: Resposta vazia ou erro:`, response);
        return null;
      }),
      catchError((error) => {
        console.error(`❌ TopicoService: Erro ao buscar tópico:`, error);
        return of(null);
      })
    );
  }

  /**
   * Busca tópicos de uma disciplina
   */
  getTopicosByDisciplina(disciplinaId: number): Observable<Topico[]> {
    return this.http.get<ApiResponse<Topico[]>>(`${this.apiUrl}/disciplina/${disciplinaId}`).pipe(
      map(response => {
        if (response.sucesso && response.dados) {
          return response.dados;
        }
        return [];
      }),
      catchError(() => {
        console.error('Erro ao buscar tópicos');
        return of([]);
      })
    );
  }
}
