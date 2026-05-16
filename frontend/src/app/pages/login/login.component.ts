import { Component, OnInit, ViewChild, ElementRef, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { ToasterService } from '../../services/toaster.service';

interface Particle {
  x: number;
  y: number;
  vx: number;
  vy: number;
  size: number;
  opacity: number;
}

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit, OnDestroy {
  email = '';
  password = '';
  loading = false;
  error = '';
  showPassword = false;
  showRegisterPassword = false;
  showRegisterConfirmPassword = false;
  showRegisterModal = false;
  registerEmail = '';
  registerPassword = '';
  registerConfirmPassword = '';
  registerError = '';
  registerNome = '';
  registerSobrenome = '';
  registerFoto: File | null = null;
  registerFotoPreview: string | ArrayBuffer | null = null;
  socialLoading = false;
  socialError = '';
  
  @ViewChild('particlesCanvas') canvasRef!: ElementRef<HTMLCanvasElement>;

  private particles: Particle[] = [];
  private animationId: number | null = null;

  constructor(
    private authService: AuthService,
    private router: Router,
    private toaster: ToasterService
  ) {}

  ngOnInit(): void {
    if (this.authService.isAuthenticated()) {
      this.router.navigate(['/dashboard']);
    }
    
    setTimeout(() => this.drawParticles(), 100);
  }

  private drawParticles(): void {
    const canvas = this.canvasRef?.nativeElement;
    if (!canvas) return;

    const container = canvas.parentElement;
    if (!container) return;

    canvas.width = container.clientWidth;
    canvas.height = container.clientHeight;

    for (let i = 0; i < 80; i++) {
      this.particles.push({
        x: Math.random() * canvas.width,
        y: Math.random() * canvas.height,
        vx: (Math.random() - 0.5) * 2,
        vy: (Math.random() - 0.5) * 2,
        size: Math.random() * 3 + 1,
        opacity: Math.random() * 0.1 + 0.2
      });
    }

    this.animate(canvas);
  }

  private animate(canvas: HTMLCanvasElement): void {
    const ctx = canvas.getContext('2d');
    if (!ctx) return;

    ctx.clearRect(0, 0, canvas.width, canvas.height);

    for (let i = 0; i < this.particles.length; i++) {
      const p = this.particles[i];
      p.x += p.vx;
      p.y += p.vy;

      if (p.x - p.size < 0 || p.x + p.size > canvas.width) p.vx *= -1;
      if (p.y - p.size < 0 || p.y + p.size > canvas.height) p.vy *= -1;

      p.x = Math.max(p.size, Math.min(canvas.width - p.size, p.x));
      p.y = Math.max(p.size, Math.min(canvas.height - p.size, p.y));

      ctx.fillStyle = `rgba(255, 255, 255, ${p.opacity})`;
      ctx.beginPath();
      ctx.arc(p.x, p.y, p.size, 0, Math.PI * 2);
      ctx.fill();
    }

    for (let i = 0; i < this.particles.length; i++) {
      for (let j = i + 1; j < this.particles.length; j++) {
        const dx = this.particles[i].x - this.particles[j].x;
        const dy = this.particles[i].y - this.particles[j].y;
        const distance = Math.sqrt(dx * dx + dy * dy);

        if (distance < 150) {
          ctx.strokeStyle = `rgba(255, 255, 255, ${0.4 * (1 - distance / 150)})`;
          ctx.lineWidth = 1;
          ctx.beginPath();
          ctx.moveTo(this.particles[i].x, this.particles[i].y);
          ctx.lineTo(this.particles[j].x, this.particles[j].y);
          ctx.stroke();
        }
      }
    }

    this.animationId = requestAnimationFrame(() => this.animate(canvas));
  }

  login(): void {
    if (!this.email || !this.password) {
      this.error = 'Por favor, preencha todos os campos';
      return;
    }

    this.loading = true;
    this.error = '';

    this.authService.login(this.email, this.password).subscribe({
      next: () => {
        this.toaster.success('Login realizado com sucesso!');
        this.router.navigate(['/dashboard']);
        this.loading = false;
      },
      error: (err: any) => {
        this.error = 'Email ou senha incorretos';
        this.toaster.error('Erro ao fazer login. Verifique suas credenciais.');
        this.loading = false;
      }
    });
  }

  handleKeyPress(event: KeyboardEvent): void {
    if (event.key === 'Enter') {
      this.login();
    }
  }

  forgotPassword(): void {
    alert('Funcionalidade de recuperação de senha será implementada em breve!');
  }

  openRegisterModal(): void {
    this.showRegisterModal = true;
  }

  closeRegisterModal(): void {
    this.showRegisterModal = false;
    this.registerEmail = '';
    this.registerPassword = '';
    this.registerConfirmPassword = '';
    this.registerNome = '';
    this.registerSobrenome = '';
    this.registerFoto = null;
    this.registerFotoPreview = null;
    this.registerError = '';
  }

  onFotoSelected(event: any): void {
    const file = event.target.files[0];
    if (file) {
      // Validar tipo de arquivo
      if (!file.type.startsWith('image/')) {
        this.registerError = 'Por favor, selecione uma imagem';
        return;
      }

      // Validar tamanho (máx 5MB)
      if (file.size > 5 * 1024 * 1024) {
        this.registerError = 'A imagem não pode ter mais de 5MB';
        return;
      }

      this.registerFoto = file;
      this.registerError = '';

      // Criar preview
      const reader = new FileReader();
      reader.onload = (e) => {
        this.registerFotoPreview = e.target?.result || null;
      };
      reader.readAsDataURL(file);
    }
  }

  removerFoto(): void {
    this.registerFoto = null;
    this.registerFotoPreview = null;
  }

  register(): void {
    if (!this.registerEmail || !this.registerPassword || !this.registerConfirmPassword ||
        !this.registerNome || !this.registerSobrenome) {
      this.registerError = 'Por favor, preencha todos os campos obrigatórios';
      return;
    }

    if (this.registerPassword !== this.registerConfirmPassword) {
      this.registerError = 'As senhas não conferem';
      return;
    }

    if (this.registerPassword.length < 6) {
      this.registerError = 'A senha deve ter no mínimo 6 caracteres';
      return;
    }

    this.loading = true;
    this.registerError = '';

    const avatarBase64 = this.registerFotoPreview && typeof this.registerFotoPreview === 'string'
      ? this.registerFotoPreview
      : undefined;

    this.authService.register(
      this.registerEmail,
      this.registerPassword,
      this.registerNome,
      this.registerSobrenome,
      avatarBase64
    ).subscribe({
      next: () => {
        this.toaster.success('Conta criada com sucesso! Faça login para continuar.');
        this.closeRegisterModal();
        this.loading = false;
      },
      error: (err: any) => {
        this.registerError = 'Erro ao criar conta. Email pode já estar em uso.';
        this.toaster.error('Erro ao criar conta. Email pode já estar em uso.');
        this.loading = false;
      }
    });
  }

  loginWithGoogle(): void {
    this.socialLoading = true;
    this.socialError = '';
    const email = prompt('Simular login Google - Digite seu email:');
    if (email) {
      this.authService.socialLogin(email, 'google').subscribe({
        next: () => {
          this.toaster.success('Login com Google realizado!');
          this.router.navigate(['/dashboard']);
          this.socialLoading = false;
        },
        error: (err: any) => {
          this.socialError = 'Erro ao fazer login com Google';
          this.toaster.error('Erro ao fazer login com Google');
          this.socialLoading = false;
        }
      });
    } else {
      this.socialLoading = false;
    }
  }

  loginWithOutlook(): void {
    this.socialLoading = true;
    this.socialError = '';
    const email = prompt('Simular login Outlook - Digite seu email:');
    if (email) {
      this.authService.socialLogin(email, 'outlook').subscribe({
        next: () => {
          this.toaster.success('Login com Outlook realizado!');
          this.router.navigate(['/dashboard']);
          this.socialLoading = false;
        },
        error: (err: any) => {
          this.socialError = 'Erro ao fazer login com Outlook';
          this.toaster.error('Erro ao fazer login com Outlook');
          this.socialLoading = false;
        }
      });
    } else {
      this.socialLoading = false;
    }
  }

  ngOnDestroy(): void {
    if (this.animationId) {
      cancelAnimationFrame(this.animationId);
    }
  }
}
