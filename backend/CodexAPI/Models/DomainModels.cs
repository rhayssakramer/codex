namespace CodexAPI.Models;

public class Usuario
{
    public int Id { get; set; }
    public string Email { get; set; } = null!;
    public string Nome { get; set; } = null!;
    public string Sobrenome { get; set; } = null!;
    public string SenhaHash { get; set; } = null!;
    public string? Avatar { get; set; }
    public bool Ativo { get; set; } = true;
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    public DateTime DataAtualizacao { get; set; } = DateTime.UtcNow;
    public string? Papel { get; set; } = "usuario"; // usuario, admin

    // Dados pessoais
    public string? Cpf { get; set; }
    public DateTime? DataNascimento { get; set; }
    public string? Genero { get; set; }

    // Endereço
    public string? Cep { get; set; }
    public string? Rua { get; set; }
    public string? Numero { get; set; }
    public string? Complemento { get; set; }
    public string? Bairro { get; set; }
    public string? Cidade { get; set; }
    public string? Estado { get; set; }

    // Navegação
    public ICollection<ProgressoTopico>? ProgressosTopicos { get; set; }
}

public class Area
{
    public int Id { get; set; }
    public string Nome { get; set; } = null!;
    public string Descricao { get; set; } = null!;
    public string? Icone { get; set; }
    public int Ordem { get; set; }
    public bool Ativo { get; set; } = true;
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

    // Navegação
    public ICollection<Disciplina>? Disciplinas { get; set; }
}

public class Disciplina
{
    public int Id { get; set; }
    public int AreaId { get; set; }
    public string Nome { get; set; } = null!;
    public string Descricao { get; set; } = null!;
    public string? Imagem { get; set; }
    public int Ordem { get; set; }
    public bool Ativo { get; set; } = true;
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

    // Navegação
    public Area? Area { get; set; }
    public ICollection<Topico>? Topicos { get; set; }
}

public class Topico
{
    public int Id { get; set; }
    public int DisciplinaId { get; set; }
    public string Titulo { get; set; } = null!;
    public string Conteudo { get; set; } = null!;
    public string? CodigoExemplo { get; set; }
    public string? VideoUrl { get; set; }
    public int Ordem { get; set; }
    public int Dificuldade { get; set; } = 1; // 1=Iniciante, 2=Intermediário, 3=Avançado
    public bool Ativo { get; set; } = true;
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

    // Navegação
    public Disciplina? Disciplina { get; set; }
    public ICollection<ProgressoTopico>? ProgressosTopicos { get; set; }
}

public class ProgressoTopico
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public int TopicoId { get; set; }
    public bool Concluido { get; set; } = false;
    public int? Progresso { get; set; } = 0; // Percentual 0-100
    public DateTime? DataInicio { get; set; }
    public DateTime? DataConclusao { get; set; }
    public DateTime DataAtualizacao { get; set; } = DateTime.UtcNow;

    // Navegação
    public Usuario? Usuario { get; set; }
    public Topico? Topico { get; set; }
}

public class AuditLog
{
    public int Id { get; set; }
    public int? UsuarioId { get; set; }
    public string Acao { get; set; } = null!;
    public string Entidade { get; set; } = null!;
    public string? Detalhes { get; set; }
    public DateTime Data { get; set; } = DateTime.UtcNow;
    public string IpAddress { get; set; } = null!;

    // Navegação
    public Usuario? Usuario { get; set; }
}
