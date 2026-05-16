import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { map, catchError } from 'rxjs/operators';

interface AuthResponse {
  sucesso: boolean;
  mensagem?: string;
  dados?: {
    id: number;
    email: string;
    nome: string;
    sobrenome: string;
    avatar?: string;
    token: string;
    tokenExpiresAt: string;
    papel: string;
  };
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private isAuthenticatedSubject = new BehaviorSubject<boolean>(this.hasToken());
  public isAuthenticated$ = this.isAuthenticatedSubject.asObservable();

  private userSubject = new BehaviorSubject<any>(null);
  public user$ = this.userSubject.asObservable();

  constructor(private http: HttpClient) {
    const token = localStorage.getItem('auth_token');
    if (token) {
      const user = JSON.parse(localStorage.getItem('user') || '{}');
      if (user.email === 'admin@codex.com.br') {
        user.role = 'admin';
      }
      this.userSubject.next(user);

      // Refresh profile from server to ensure data is up-to-date
      this.http.get<any>('/api/auth/me', {
        headers: { 'Authorization': `Bearer ${token}` }
      }).subscribe({
        next: (response) => {
          if (response.sucesso && response.dados) {
            const d = response.dados;
            const fullUser: any = {
              id: d.id,
              email: d.email,
              name: d.nome,
              surname: d.sobrenome,
              avatar: d.avatar || 'assets/user.png',
              role: d.papel,
              cpf: d.cpf || '',
              dataNascimento: d.dataNascimento || '',
              genero: d.genero || '',
              cep: d.cep || '',
              rua: d.rua || '',
              numero: d.numero || '',
              complemento: d.complemento || '',
              bairro: d.bairro || '',
              cidade: d.cidade || '',
              estado: d.estado || ''
            };
            localStorage.setItem('user', JSON.stringify(fullUser));
            this.userSubject.next(fullUser);
          }
        },
        error: () => {}
      });
    }
  }

  login(email: string, password: string): Observable<any> {
    return this.http.post<any>('/api/auth/login', { email, senha: password }).pipe(
      map(response => {
        if (response.sucesso && response.dados) {
          const data = response.dados;
          const user: any = {
            id: data.id,
            email: data.email,
            name: data.nome,
            surname: data.sobrenome,
            avatar: data.avatar || 'assets/user.png',
            role: data.papel,
            cpf: data.cpf || '',
            dataNascimento: data.dataNascimento || '',
            genero: data.genero || '',
            cep: data.cep || '',
            rua: data.rua || '',
            numero: data.numero || '',
            complemento: data.complemento || '',
            bairro: data.bairro || '',
            cidade: data.cidade || '',
            estado: data.estado || ''
          };

          localStorage.setItem('auth_token', data.token);
          localStorage.setItem('user', JSON.stringify(user));

          this.userSubject.next(user);
          this.isAuthenticatedSubject.next(true);

          return user;
        }
        throw new Error(response.mensagem || 'Erro ao fazer login');
      }),
      catchError(error => {
        const msg = error?.error?.mensagem || error?.message || 'Email ou senha inválidos';
        throw new Error(msg);
      })
    );
  }

  register(email: string, password: string, nome?: string, sobrenome?: string): Observable<any> {
    return this.http.post<AuthResponse>('/api/auth/register', {
      email,
      senha: password,
      confirmarSenha: password,
      nome: nome || email.split('@')[0],
      sobrenome: sobrenome || ''
    }).pipe(
      map(response => {
        if (response.sucesso) {
          return { success: true, message: 'Conta criada com sucesso' };
        }
        throw new Error(response.mensagem || 'Erro ao registrar');
      }),
      catchError(error => {
        const msg = error?.error?.mensagem || error?.message || 'Erro ao registrar';
        throw new Error(msg);
      })
    );
  }

  socialLogin(email: string, provider: 'google' | 'outlook'): Observable<any> {
    return new Observable(observer => {
      setTimeout(() => {
        const user: any = {
          id: Date.now(),
          email: email,
          name: email.split('@')[0],
          avatar: 'assets/user.png',
          provider: provider
        };

        localStorage.setItem('auth_token', 'social_token_' + Date.now());
        localStorage.setItem('user', JSON.stringify(user));

        this.userSubject.next(user);
        this.isAuthenticatedSubject.next(true);

        observer.next(user);
        observer.complete();
      }, 500);
    });
  }

  logout(): void {
    localStorage.removeItem('auth_token');
    localStorage.removeItem('user');
    this.userSubject.next(null);
    this.isAuthenticatedSubject.next(false);
  }

  private hasToken(): boolean {
    return !!localStorage.getItem('auth_token');
  }

  isAuthenticated(): boolean {
    return this.hasToken();
  }

  getUser(): any {
    return this.userSubject.value;
  }

  getToken(): string | null {
    return localStorage.getItem('auth_token');
  }

  updateUserProfile(profileData: any): void {
    const token = this.getToken();
    const user = this.userSubject.value;

    if (!user) return;

    const updatedUser = { ...user, ...profileData };
    localStorage.setItem('user', JSON.stringify(updatedUser));
    this.userSubject.next(updatedUser);

    // Persist to backend
    if (token && !token.startsWith('social_token_')) {
      this.http.put<any>('/api/auth/perfil', {
        nome: profileData.name || profileData.nome,
        sobrenome: profileData.surname || profileData.sobrenome,
        avatar: profileData.avatar,
        cpf: profileData.cpf,
        dataNascimento: profileData.dataNascimento,
        genero: profileData.genero,
        cep: profileData.cep,
        rua: profileData.rua,
        numero: profileData.numero,
        complemento: profileData.complemento,
        bairro: profileData.bairro,
        cidade: profileData.cidade,
        estado: profileData.estado
      }, {
        headers: { 'Authorization': `Bearer ${token}` }
      }).subscribe({
        next: (response) => {
          if (response?.dados) {
            const d = response.dados;
            const serverUser: any = {
              ...updatedUser,
              name: d.nome,
              surname: d.sobrenome,
              avatar: d.avatar || 'assets/user.png',
              cpf: d.cpf || '',
              dataNascimento: d.dataNascimento || '',
              genero: d.genero || '',
              cep: d.cep || '',
              rua: d.rua || '',
              numero: d.numero || '',
              complemento: d.complemento || '',
              bairro: d.bairro || '',
              cidade: d.cidade || '',
              estado: d.estado || ''
            };
            localStorage.setItem('user', JSON.stringify(serverUser));
            this.userSubject.next(serverUser);
          }
          console.log('Perfil salvo no servidor');
        },
        error: () => console.warn('Falha ao salvar perfil no servidor (dados salvos localmente)')
      });
    }
  }
}
