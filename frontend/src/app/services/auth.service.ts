import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private isAuthenticatedSubject = new BehaviorSubject<boolean>(this.hasToken());
  public isAuthenticated$ = this.isAuthenticatedSubject.asObservable();

  private userSubject = new BehaviorSubject<any>(null);
  public user$ = this.userSubject.asObservable();

  constructor() {
    const token = localStorage.getItem('auth_token');
    if (token) {
      const user = JSON.parse(localStorage.getItem('user') || '{}');
      this.userSubject.next(user);
    }
  }

  login(email: string, password: string): Observable<any> {
    return new Observable(observer => {
      // Simular autenticação
      setTimeout(() => {
        const user = {
          id: 1,
          email: email,
          name: email.split('@')[0],
          avatar: `https://api.dicebear.com/7.x/avataaars/svg?seed=${email}`
        };
        
        localStorage.setItem('auth_token', 'fake_token_' + Date.now());
        localStorage.setItem('user', JSON.stringify(user));
        
        this.userSubject.next(user);
        this.isAuthenticatedSubject.next(true);
        
        observer.next(user);
        observer.complete();
      }, 1000);
    });
  }

  register(email: string, password: string): Observable<any> {
    return new Observable(observer => {
      // Simular registro
      setTimeout(() => {
        const users = JSON.parse(localStorage.getItem('users') || '[]');
        
        // Verificar se o email já existe
        if (users.some((u: any) => u.email === email)) {
          observer.error({ message: 'Email já cadastrado' });
          return;
        }
        
        // Criar novo usuário
        const user = {
          id: users.length + 1,
          email: email,
          password: password, // Em produção, seria hasheada
          name: email.split('@')[0],
          avatar: `https://api.dicebear.com/7.x/avataaars/svg?seed=${email}`,
          createdAt: new Date()
        };
        
        users.push(user);
        localStorage.setItem('users', JSON.stringify(users));
        
        observer.next({ success: true, message: 'Conta criada com sucesso' });
        observer.complete();
      }, 1000);
    });
  }

  socialLogin(email: string, provider: 'google' | 'outlook'): Observable<any> {
    return new Observable(observer => {
      // Simular login social
      setTimeout(() => {
        const users = JSON.parse(localStorage.getItem('users') || '[]');
        
        // Procurar por usuário existente
        const existingUser = users.find((u: any) => u.email === email);
        
        if (existingUser) {
          // Usuário existe, fazer login
          localStorage.setItem('auth_token', 'fake_token_' + Date.now());
          localStorage.setItem('user', JSON.stringify(existingUser));
          localStorage.setItem('loginProvider', provider);
          
          this.userSubject.next(existingUser);
          this.isAuthenticatedSubject.next(true);
          
          observer.next(existingUser);
          observer.complete();
        } else {
          // Usuário não existe, criar novo
          const newUser = {
            id: users.length + 1,
            email: email,
            name: email.split('@')[0],
            avatar: `https://api.dicebear.com/7.x/avataaars/svg?seed=${email}`,
            provider: provider,
            createdAt: new Date()
          };
          
          users.push(newUser);
          localStorage.setItem('users', JSON.stringify(users));
          localStorage.setItem('auth_token', 'fake_token_' + Date.now());
          localStorage.setItem('user', JSON.stringify(newUser));
          localStorage.setItem('loginProvider', provider);
          
          this.userSubject.next(newUser);
          this.isAuthenticatedSubject.next(true);
          
          observer.next(newUser);
          observer.complete();
        }
      }, 1000);
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

  updateUserProfile(profileData: any): void {
    const user = this.userSubject.value;
    if (user) {
      const updatedUser = {
        ...user,
        ...profileData
      };
      localStorage.setItem('user', JSON.stringify(updatedUser));
      this.userSubject.next(updatedUser);
    }
  }
}
