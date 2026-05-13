import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
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
  imports: [CommonModule, FormsModule],
  templateUrl: './profile-edit.component.html',
  styleUrls: ['./profile-edit.component.css']
})
export class ProfileEditComponent implements OnInit {
  @Input() user: any;
  @Output() close = new EventEmitter<void>();
  @Output() save = new EventEmitter<UserProfile>();

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

  constructor(private toaster: ToasterService) {}

  ngOnInit(): void {
    this.loadUserData();
  }

  private loadUserData(): void {
    if (this.user) {
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
      // Validar tipo de arquivo
      if (!file.type.startsWith('image/')) {
        this.error = 'Por favor, selecione uma imagem';
        return;
      }

      // Validar tamanho (máx 5MB)
      if (file.size > 5 * 1024 * 1024) {
        this.error = 'A imagem não pode ter mais de 5MB';
        return;
      }

      this.profileFoto = file;
      this.error = '';

      // Criar preview
      const reader = new FileReader();
      reader.onload = (e) => {
        this.profileFotoPreview = e.target?.result || null;
      };
      reader.readAsDataURL(file);
    }
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
    
    // Usar API pública ViaCEP
    fetch(`https://viacep.com.br/ws/${cep}/json/`)
      .then(response => response.json())
      .then(data => {
        if (data.erro) {
          this.error = 'CEP não encontrado';
        } else {
          this.profileData.rua = data.logradouro;
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

    // Simular salvamento
    setTimeout(() => {
      // Se houver foto nova, salvar como base64
      if (this.profileFoto) {
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
    // Atualizar localStorage com os dados do perfil
    const user = JSON.parse(localStorage.getItem('user') || '{}');
    Object.assign(user, {
      name: this.profileData.name,
      surname: this.profileData.surname,
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
      user.avatar = this.profileFotoPreview;
    }
    localStorage.setItem('user', JSON.stringify(user));

    this.toaster.success('Perfil atualizado com sucesso!');
    this.loading = false;
    this.save.emit(this.profileData);
    this.fecharModal();
  }

  fecharModal(): void {
    this.close.emit();
  }
}
