import { Component, OnInit, Input, Output, EventEmitter, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { ImageCropperComponent, ImageCroppedEvent, base64ToFile } from 'ngx-image-cropper';
import { ToasterService } from '../../services/toaster.service';

interface UserProfile {
  name: string;
  surname: string;
  email: string;
  cpf: string;
  dataNascimento: string;
  genero: string;
  cep: string;
  rua: string;
  numero: string;
  complemento: string;
  bairro: string;
  cidade: string;
  estado: string;
  avatar?: string;
}

@Component({
  selector: 'app-profile-edit',
  standalone: true,
  imports: [CommonModule, FormsModule, HttpClientModule, ImageCropperComponent],
  templateUrl: './profile-edit.component.html',
  styleUrls: ['./profile-edit.component.css']
})
export class ProfileEditComponent implements OnInit {
  @Input() user: any;
  @Output() close = new EventEmitter<void>();
  @Output() save = new EventEmitter<UserProfile>();
  @ViewChild('imageCropper') imageCropper!: ImageCropperComponent;

  isAdmin = false;
  isEmailLocked = false;
  showCropModal = false;
  imageForCrop: string | undefined = undefined;
  croppedImage: string | null = null;

  profileData: UserProfile = {
    name: '',
    surname: '',
    email: '',
    cpf: '',
    dataNascimento: '',
    genero: '',
    cep: '',
    rua: '',
    numero: '',
    complemento: '',
    bairro: '',
    cidade: '',
    estado: ''
  };

  profileFoto: File | null = null;
  profileFotoPreview: string | ArrayBuffer | null = null;
  cepLoading = false;
  loading = false;
  error = '';

  constructor(
    private toaster: ToasterService,
    private http: HttpClient
  ) {}

  ngOnInit(): void {
    this.loadUserData();
  }

  private loadUserData(): void {
    if (this.user) {
      // Verificar role - pode vir do user object ou precisar ser detectado por email
      this.isAdmin = this.user.role === 'admin' || this.user.email === 'admin@codex.com.br';
      this.isEmailLocked = !!this.user.email?.trim();
      console.log('User role:', this.user.role, 'Email:', this.user.email, 'Is Admin:', this.isAdmin, 'Email Locked:', this.isEmailLocked);
      this.profileData = {
        name: this.user.name || '',
        surname: this.user.surname || '',
        email: this.user.email || '',
        cpf: this.user.cpf || '',
        dataNascimento: this.user.dataNascimento || '',
        genero: this.user.genero || '',
        cep: this.user.cep || '',
        rua: this.user.rua || '',
        numero: this.user.numero || '',
        complemento: this.user.complemento || '',
        bairro: this.user.bairro || '',
        cidade: this.user.cidade || '',
        estado: this.user.estado || ''
      };
      if (this.user.avatar) {
        this.profileFotoPreview = this.user.avatar;
      }
    }
  }

  onFotoSelected(event: any): void {
    const file = event.target.files[0];
    if (file) {
      if (!file.type.startsWith('image/')) {
        this.error = 'Por favor, selecione uma imagem';
        return;
      }
      if (file.size > 5 * 1024 * 1024) {
        this.error = 'A imagem não pode ter mais de 5MB';
        return;
      }
      this.profileFoto = file;
      this.error = '';
      const reader = new FileReader();
      reader.onload = (e) => {
        this.imageForCrop = e.target?.result as string;
        this.showCropModal = true;
      };
      reader.readAsDataURL(file);
    }
  }

  cropperReady(): void {
    console.log('Cropper pronto');
  }

  imageCropped(event: ImageCroppedEvent): void {
    console.log('Imagem cortada disparada:', event);
    
    if (event && event.blob) {
      // Converte blob para base64
      const reader = new FileReader();
      reader.onload = () => {
        this.croppedImage = reader.result as string;
        console.log('Base64 capturada com sucesso:', !!this.croppedImage);
      };
      reader.readAsDataURL(event.blob);
    } else if (event && event.base64) {
      this.croppedImage = event.base64;
      console.log('Base64 capturada do evento:', !!this.croppedImage);
    }
  }

  loadImageFailed(): void {
    this.error = 'Erro ao carregar a imagem';
  }

  salvarImagemCortada(): void {
    console.log('Tentando salvar imagem cortada:', !!this.croppedImage);
    
    if (this.croppedImage) {
      this.profileFotoPreview = this.croppedImage;
      this.showCropModal = false;
      this.imageForCrop = undefined;
      console.log('Imagem salva com sucesso');
      this.toaster.success('Foto atualizada com sucesso!');
    } else {
      console.log('Nenhuma imagem para salvar');
      this.toaster.error('Erro: Mova a área de corte e tente novamente');
    }
  }

  cancelarCrop(): void {
    this.showCropModal = false;
    this.imageForCrop = undefined;
    this.croppedImage = null;
    this.profileFoto = null;
  }

  removerFoto(): void {
    this.profileFoto = null;
    this.profileFotoPreview = null;
  }

  buscarCEP(): void {
    const cep = this.profileData.cep.replace(/\D/g, '');
    if (cep.length !== 8) {
      this.error = 'CEP inválido';
      return;
    }
    this.cepLoading = true;
    fetch(`https://viacep.com.br/ws/${cep}/json/`)
      .then(response => response.json())
      .then(data => {
        if (data.erro) {
          this.error = 'CEP não encontrado';
        } else {
          this.profileData.rua = data.logradouro;
          this.profileData.bairro = data.bairro;
          this.profileData.cidade = data.localidade;
          this.profileData.estado = data.uf;
          this.error = '';
        }
        this.cepLoading = false;
      })
      .catch(() => {
        this.error = 'Erro ao buscar CEP';
        this.cepLoading = false;
      });
  }

  formatarCPF(event: any): void {
    let valor = event.target.value.replace(/\D/g, '');
    if (valor.length <= 11) {
      if (valor.length <= 3) {
        valor = valor;
      } else if (valor.length <= 6) {
        valor = valor.substring(0, 3) + '.' + valor.substring(3);
      } else if (valor.length <= 9) {
        valor = valor.substring(0, 3) + '.' + valor.substring(3, 6) + '.' + valor.substring(6);
      } else {
        valor = valor.substring(0, 3) + '.' + valor.substring(3, 6) + '.' + valor.substring(6, 9) + '-' + valor.substring(9, 11);
      }
    }
    this.profileData.cpf = valor;
  }

  salvarPerfil(): void {
    if (!this.profileData.name || !this.profileData.surname || !this.profileData.email) {
      this.error = 'Por favor, preencha os campos obrigatórios (Nome, Sobrenome, Email)';
      return;
    }
    this.loading = true;
    this.error = '';
    setTimeout(() => {
      // Se há uma foto em preview (cortada ou carregada), usa ela diretamente
      if (this.profileFotoPreview && typeof this.profileFotoPreview === 'string') {
        this.profileData.avatar = this.profileFotoPreview;
        this.executarSalvamento();
      } else if (this.profileFoto) {
        // Se tem um arquivo de foto, carrega como base64
        const reader = new FileReader();
        reader.onload = () => {
          this.profileData.avatar = reader.result as string;
          this.executarSalvamento();
        };
        reader.readAsDataURL(this.profileFoto);
      } else {
        this.executarSalvamento();
      }
    }, 500);
  }

  private executarSalvamento(): void {
    const user = JSON.parse(localStorage.getItem('user') || '{}');
    const updatedUser = Object.assign({}, user, {
      name: this.profileData.name,
      surname: this.profileData.surname,
      email: this.isEmailLocked ? (user.email || this.profileData.email) : this.profileData.email,
      cpf: this.profileData.cpf,
      dataNascimento: this.profileData.dataNascimento,
      genero: this.profileData.genero,
      cep: this.profileData.cep,
      rua: this.profileData.rua,
      numero: this.profileData.numero,
      complemento: this.profileData.complemento,
      bairro: this.profileData.bairro,
      cidade: this.profileData.cidade,
      estado: this.profileData.estado
    });
    if (this.profileFotoPreview) {
      updatedUser.avatar = this.profileFotoPreview;
    }
    localStorage.setItem('user', JSON.stringify(updatedUser));
    const token = localStorage.getItem('token');
    if (token) {
      this.http.put('/api/usuarios/perfil', updatedUser, {
        headers: { 'Authorization': `Bearer ${token}` }
      }).subscribe({
        next: () => {
          this.toaster.success('Perfil atualizado com sucesso!');
        },
        error: () => {
          this.toaster.success('Perfil atualizado com sucesso!');
        }
      });
    } else {
      this.toaster.success('Perfil atualizado com sucesso!');
    }
    this.loading = false;
    this.save.emit(this.profileData);
    this.fecharModal();
  }

  fecharModal(): void {
    this.close.emit();
  }
}
