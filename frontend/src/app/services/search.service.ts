import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { debounceTime, distinctUntilChanged, switchMap, catchError, map } from 'rxjs/operators';

export interface SearchResult {
  id: string;
  title: string;
  description?: string;
  type: 'topico' | 'disciplina' | 'area';
  area?: string;
  disciplinaId?: string;
  icon: string;
}

interface ApiResponse<T> {
  sucesso: boolean;
  dados: T;
  mensagem?: string;
}

interface ApiSearchResult {
  id: string;
  title: string;
  description?: string;
  type: string;
  area?: string;
  disciplinaId?: string;
  icon: string;
}

@Injectable({
  providedIn: 'root'
})
export class SearchService {
  private apiUrl = '/api/search'; // Usar proxy do Angular
  
  // Mock data para fallback quando API falhar
  private mockResults: SearchResult[] = [
    {
      id: '1',
      title: 'Variáveis e Tipos',
      description: 'Aprenda sobre variáveis, var, let, const...',
      type: 'topico',
      area: '1',
      disciplinaId: '1',
      icon: 'fa-cube'
    }
  ];

  constructor(private http: HttpClient) {}

  /**
   * Busca por palavras-chave em tópicos, disciplinas e áreas
   * @param query - Termo de busca
   * @returns Observable com resultados de busca
   */
  search(query: string): Observable<SearchResult[]> {
    if (!query.trim()) {
      return of([]);
    }

    console.log('Buscando:', query);

    // Buscar da API
    return this.http.get<ApiResponse<ApiSearchResult[]>>(this.apiUrl, {
      params: { q: query }
    }).pipe(
      map(response => {
        console.log('Resposta da API:', response);
        let resultados: SearchResult[] = [];
        
        if (response.sucesso && response.dados) {
          const mapeados = response.dados.map(item => ({
            id: item.id,
            title: item.title,
            description: item.description,
            type: item.type as 'topico' | 'disciplina' | 'area',
            area: item.area,
            disciplinaId: item.disciplinaId,
            icon: item.icon
          }));
          console.log('Dados mapeados:', mapeados);
          resultados.push(...mapeados);
        }
        
        // Também buscar nos dados do localStorage (conteúdo criado no admin)
        const localStorageResults = this.searchLocalStorage(query);
        console.log('Resultados do localStorage:', localStorageResults);
        resultados.push(...localStorageResults);
        
        // Remover duplicatas
        resultados = this.removeDuplicates(resultados);
        
        console.log('Total de resultados:', resultados);
        return resultados;
      }),
      catchError((error) => {
        console.warn('⚠️ API indisponível, buscando apenas no localStorage:', error);
        // Fallback: buscar apenas no localStorage
        const localResults = this.searchLocalStorage(query);
        console.log('🎲 Resultados do localStorage (fallback):', localResults);
        return of(localResults);
      })
    );
  }

  private searchLocalStorage(query: string): SearchResult[] {
    const results: SearchResult[] = [];
    const queryLower = query.toLowerCase();
    
    // Buscar nos conteúdos criados no admin
    const stored = localStorage.getItem('codex_contents');
    if (stored) {
      const contents = JSON.parse(stored);
      contents.forEach((content: any) => {
        // Verificar se a busca encontra em topico, disciplina ou area
        if (
          content.topico?.toLowerCase().includes(queryLower) ||
          content.disciplina?.toLowerCase().includes(queryLower) ||
          content.area?.toLowerCase().includes(queryLower) ||
          content.conteudo?.toLowerCase().includes(queryLower)
        ) {
          // Mapear a área para o slug correto
          const areaSlug = this.normalizeAreaName(content.area);
          
          results.push({
            id: content.id.toString(),
            title: content.topico,
            description: content.conteudo?.substring(0, 100) + '...',
            type: 'topico',
            area: areaSlug,
            disciplinaId: (content.disciplinaId || 1).toString(),
            icon: 'fa-file'
          });
        }
      });
    }
    
    return results;
  }

  private normalizeAreaName(areaName: string): string {
    if (!areaName) return '';
    return areaName.toLowerCase()
      .replace('ã', 'a')
      .replace('ç', 'c')
      .replace(/\s+/g, '')
      .trim();
  }

  private removeDuplicates(results: SearchResult[]): SearchResult[] {
    const seen = new Set<string>();
    return results.filter(item => {
      const key = `${item.type}-${item.title}`;
      if (seen.has(key)) {
        return false;
      }
      seen.add(key);
      return true;
    });
  }

  /**
   * Busca com debounce automático (útil para input em tempo real)
   * @param query$ - Observable do termo de busca
   * @returns Observable com resultados debounced
   */
  searchWithDebounce(query$: Observable<string>): Observable<SearchResult[]> {
    return query$.pipe(
      debounceTime(300),
      distinctUntilChanged(),
      switchMap(query => this.search(query))
    );
  }
}
