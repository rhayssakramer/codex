namespace CodexAPI.DTOs;

// Auth DTOs
public class LoginRequestDto
{
    public string Email { get; set; } = null!;
    public string Senha { get; set; } = null!;
}

public class RegisterRequestDto
{
    public string Email { get; set; } = null!;
    public string Nome { get; set; } = null!;
    public string Sobrenome { get; set; } = null!;
    public string Senha { get; set; } = null!;
    public string ConfirmarSenha { get; set; } = null!;
    public string? Avatar { get; set; }
}

public class AuthResponseDto
{
    public int Id { get; set; }
    public string Email { get; set; } = null!;
    public string Nome { get; set; } = null!;
    public string Sobrenome { get; set; } = null!;
    public string? Avatar { get; set; }
    public string Token { get; set; } = null!;
    public DateTime TokenExpiresAt { get; set; }
    public string Papel { get; set; } = null!;
    public string? Cpf { get; set; }
    public string? DataNascimento { get; set; }
    public string? Genero { get; set; }
    public string? Cep { get; set; }
    public string? Rua { get; set; }
    public string? Numero { get; set; }
    public string? Complemento { get; set; }
    public string? Bairro { get; set; }
    public string? Cidade { get; set; }
    public string? Estado { get; set; }
}

public class UsuarioDto
{
    public int Id { get; set; }
    public string Email { get; set; } = null!;
    public string Nome { get; set; } = null!;
    public string Sobrenome { get; set; } = null!;
    public string? Avatar { get; set; }
    public bool Ativo { get; set; }
    public DateTime DataCriacao { get; set; }
    public string Papel { get; set; } = null!;
    public string? Cpf { get; set; }
    public string? DataNascimento { get; set; }
    public string? Genero { get; set; }
    public string? Cep { get; set; }
    public string? Rua { get; set; }
    public string? Numero { get; set; }
    public string? Complemento { get; set; }
    public string? Bairro { get; set; }
    public string? Cidade { get; set; }
    public string? Estado { get; set; }
}

public class UpdateProfileDto
{
    public string? Nome { get; set; }
    public string? Sobrenome { get; set; }
    public string? Avatar { get; set; }
    public string? Cpf { get; set; }
    public string? DataNascimento { get; set; }
    public string? Genero { get; set; }
    public string? Cep { get; set; }
    public string? Rua { get; set; }
    public string? Numero { get; set; }
    public string? Complemento { get; set; }
    public string? Bairro { get; set; }
    public string? Cidade { get; set; }
    public string? Estado { get; set; }
}

// Disciplina DTOs
public class DisciplinaDto
{
    public int Id { get; set; }
    public int AreaId { get; set; }
    public string Nome { get; set; } = null!;
    public string Descricao { get; set; } = null!;
    public string? Imagem { get; set; }
    public int Ordem { get; set; }
    public bool Ativo { get; set; }
    public int TotalTopicos { get; set; }
    public int TopicosConcluidos { get; set; }
}

public class CreateDisciplinaDto
{
    public int AreaId { get; set; }
    public string Nome { get; set; } = null!;
    public string Descricao { get; set; } = null!;
    public string? Imagem { get; set; }
    public int Ordem { get; set; }
}

public class UpdateDisciplinaDto
{
    public string? Nome { get; set; }
    public string? Descricao { get; set; }
    public string? Imagem { get; set; }
    public int? Ordem { get; set; }
    public bool? Ativo { get; set; }
}

// Area DTOs
public class AreaDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = null!;
    public string Descricao { get; set; } = null!;
    public string? Icone { get; set; }
    public int Ordem { get; set; }
    public bool Ativo { get; set; }
    public List<DisciplinaDto> Disciplinas { get; set; } = [];
}

public class CreateAreaDto
{
    public string Nome { get; set; } = null!;
    public string Descricao { get; set; } = null!;
    public string? Icone { get; set; }
    public int Ordem { get; set; }
}

// Topico DTOs
public class TopicoDto
{
    public int Id { get; set; }
    public int DisciplinaId { get; set; }
    public string Titulo { get; set; } = null!;
    public string Conteudo { get; set; } = null!;
    public string? CodigoExemplo { get; set; }
    public string? VideoUrl { get; set; }
    public int Ordem { get; set; }
    public int Dificuldade { get; set; }
    public bool Ativo { get; set; }
    public bool Concluido { get; set; }
    public int ProgressoPercentual { get; set; }
}

public class CreateTopicoDto
{
    public int DisciplinaId { get; set; }
    public string Titulo { get; set; } = null!;
    public string Conteudo { get; set; } = null!;
    public string? CodigoExemplo { get; set; }
    public string? VideoUrl { get; set; }
    public int Ordem { get; set; }
    public int Dificuldade { get; set; }
}

public class UpdateTopicoDto
{
    public string? Titulo { get; set; }
    public string? Conteudo { get; set; }
    public string? CodigoExemplo { get; set; }
    public string? VideoUrl { get; set; }
    public int? Ordem { get; set; }
    public int? Dificuldade { get; set; }
    public bool? Ativo { get; set; }
}

// Progresso DTOs
public class ProgressoTopicoDto
{
    public int Id { get; set; }
    public int TopicoId { get; set; }
    public bool Concluido { get; set; }
    public int Progresso { get; set; }
    public DateTime? DataInicio { get; set; }
    public DateTime? DataConclusao { get; set; }
}

public class UpdateProgressoDto
{
    public bool? Concluido { get; set; }
    public int? Progresso { get; set; }
}

// Response DTOs
public class ApiResponse<T>
{
    public bool Sucesso { get; set; }
    public string? Mensagem { get; set; }
    public T? Dados { get; set; }
    public string[]? Erros { get; set; }
}

public class PaginatedResponse<T>
{
    public List<T> Dados { get; set; } = [];
    public int Total { get; set; }
    public int Pagina { get; set; }
    public int TamanhoPagina { get; set; }
    public int TotalPaginas { get; set; }
}

// Search DTOs
public class SearchResultDto
{
    public string Id { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string Type { get; set; } = null!; // "area", "disciplina", "topico"
    public string? Area { get; set; }
    public string? DisciplinaId { get; set; }
    public string Icon { get; set; } = "fa-file";
}
