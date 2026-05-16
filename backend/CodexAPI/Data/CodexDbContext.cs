using CodexAPI.Models;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace CodexAPI.Data;

public class CodexDbContext : DbContext
{
    public CodexDbContext(DbContextOptions<CodexDbContext> options) : base(options)
    {
    }

    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Area> Areas => Set<Area>();
    public DbSet<Disciplina> Disciplinas => Set<Disciplina>();
    public DbSet<Topico> Topicos => Set<Topico>();
    public DbSet<ProgressoTopico> ProgressosTopicos => Set<ProgressoTopico>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Usuario
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Sobrenome).IsRequired().HasMaxLength(100);
            entity.Property(e => e.SenhaHash).IsRequired();
            entity.Property(e => e.Papel).HasMaxLength(50).HasDefaultValue("usuario");
            entity.HasIndex(e => e.Email).IsUnique();

            entity.HasMany(e => e.ProgressosTopicos)
                .WithOne(p => p.Usuario)
                .HasForeignKey(p => p.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Area
        modelBuilder.Entity<Area>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Descricao).IsRequired();
            entity.Property(e => e.Ordem).HasDefaultValue(0);

            entity.HasMany(e => e.Disciplinas)
                .WithOne(d => d.Area)
                .HasForeignKey(d => d.AreaId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Disciplina
        modelBuilder.Entity<Disciplina>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Descricao).IsRequired();
            entity.Property(e => e.Ordem).HasDefaultValue(0);

            entity.HasOne(e => e.Area)
                .WithMany(a => a.Disciplinas)
                .HasForeignKey(e => e.AreaId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.Topicos)
                .WithOne(t => t.Disciplina)
                .HasForeignKey(t => t.DisciplinaId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Topico
        modelBuilder.Entity<Topico>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Titulo).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Conteudo).IsRequired();
            entity.Property(e => e.Dificuldade).HasDefaultValue(1);
            entity.Property(e => e.Ordem).HasDefaultValue(0);

            entity.HasOne(e => e.Disciplina)
                .WithMany(d => d.Topicos)
                .HasForeignKey(e => e.DisciplinaId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.ProgressosTopicos)
                .WithOne(p => p.Topico)
                .HasForeignKey(p => p.TopicoId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ProgressoTopico
        modelBuilder.Entity<ProgressoTopico>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Progresso).HasDefaultValue(0);

            entity.HasOne(e => e.Usuario)
                .WithMany(u => u.ProgressosTopicos)
                .HasForeignKey(e => e.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Topico)
                .WithMany(t => t.ProgressosTopicos)
                .HasForeignKey(e => e.TopicoId)
                .OnDelete(DeleteBehavior.Cascade);

            // Garantir que um usuário só tenha um progresso por tópico
            entity.HasIndex(e => new { e.UsuarioId, e.TopicoId }).IsUnique();
        });

        // AuditLog
        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Acao).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Entidade).IsRequired().HasMaxLength(100);
            entity.Property(e => e.IpAddress).HasMaxLength(45);

            entity.HasOne(e => e.Usuario)
                .WithMany()
                .HasForeignKey(e => e.UsuarioId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // Dados iniciais
        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        // Usuário Admin - hash fixo para "Admin@123" (evita regeneração a cada migration)
        const string adminPasswordHash = "$2a$11$Z9XRi3yU00qWRB5REoBS4OulWR7setX9CC5QmL6VHa1U01K30g1sm";
        var usuarios = new List<Usuario>
        {
            new Usuario 
            { 
                Id = 1, 
                Email = "admin@codex.com.br", 
                Nome = "Admin", 
                Sobrenome = "Codex", 
                SenhaHash = adminPasswordHash,
                Papel = "admin",
                Avatar = "https://i.pravatar.cc/150?img=1",
                Ativo = true,
                DataCriacao = new DateTime(2026, 5, 13, 16, 22, 57, 366, DateTimeKind.Utc)
            }
        };
        modelBuilder.Entity<Usuario>().HasData(usuarios);

        // Áreas
        var areas = new List<Area>
        {
            new Area { Id = 1, Nome = "Fundamentos", Descricao = "Conceitos básicos de programação", Ordem = 1, Ativo = true },
            new Area { Id = 2, Nome = "Frontend", Descricao = "Desenvolvimento web frontend", Ordem = 2, Ativo = true },
            new Area { Id = 3, Nome = "Backend", Descricao = "Desenvolvimento web backend", Ordem = 3, Ativo = true },
            new Area { Id = 4, Nome = "DevOps", Descricao = "DevOps e infraestrutura", Ordem = 4, Ativo = true },
            new Area { Id = 5, Nome = "Certificações", Descricao = "Preparação para certificações", Ordem = 5, Ativo = true }
        };
        modelBuilder.Entity<Area>().HasData(areas);

        // Disciplinas
        var disciplinas = new List<Disciplina>
        {
            new Disciplina { Id = 1, AreaId = 1, Nome = "Lógica de Programação", Descricao = "Princípios fundamentais", Ordem = 1, Ativo = true },
            new Disciplina { Id = 2, AreaId = 1, Nome = "Estruturas de Dados", Descricao = "Arrays, Listas, Pilhas", Ordem = 2, Ativo = true },
            new Disciplina { Id = 3, AreaId = 2, Nome = "HTML", Descricao = "Linguagem de marcação para estruturar páginas web", Ordem = 1, Ativo = true },
            new Disciplina { Id = 4, AreaId = 2, Nome = "CSS", Descricao = "Estilização e layout de páginas web", Ordem = 2, Ativo = true },
            new Disciplina { Id = 14, AreaId = 2, Nome = "JavaScript", Descricao = "Linguagem do navegador", Ordem = 3, Ativo = true },
            new Disciplina { Id = 5, AreaId = 3, Nome = "C#", Descricao = "Linguagem de programação C#", Ordem = 1, Ativo = true },
            new Disciplina { Id = 6, AreaId = 3, Nome = ".NET", Descricao = "Framework .NET para aplicações web e desktop", Ordem = 2, Ativo = true },
            new Disciplina { Id = 7, AreaId = 3, Nome = "JavaScript", Descricao = "JavaScript no backend com Node.js", Ordem = 3, Ativo = true },
            new Disciplina { Id = 8, AreaId = 5, Nome = "AZ-900", Descricao = "Certificação fundamentals Microsoft Azure", Ordem = 1, Ativo = true },
            new Disciplina { Id = 9, AreaId = 5, Nome = "AI-900", Descricao = "Certificação AI fundamentals Microsoft Azure", Ordem = 2, Ativo = true },
            new Disciplina { Id = 10, AreaId = 5, Nome = "GH-900", Descricao = "Certificação GitHub fundamentals", Ordem = 3, Ativo = true },
            new Disciplina { Id = 11, AreaId = 5, Nome = "GH-300", Descricao = "Certificação GitHub advanced", Ordem = 4, Ativo = true },
            new Disciplina { Id = 12, AreaId = 4, Nome = "Git", Descricao = "Sistema de controle de versão distribuído", Ordem = 1, Ativo = true },
            new Disciplina { Id = 13, AreaId = 4, Nome = "GitHub", Descricao = "Plataforma de colaboração e hospedagem de repositórios", Ordem = 2, Ativo = true }
        };
        modelBuilder.Entity<Disciplina>().HasData(disciplinas);

        // Tópicos para Lógica de Programação
        var topicos = new List<Topico>
        {
            new Topico { Id = 1, DisciplinaId = 1, Titulo = "Variáveis e Tipos", Conteudo = "Aprenda sobre variáveis...", Ordem = 1, Dificuldade = 1, Ativo = true },
            new Topico { Id = 2, DisciplinaId = 1, Titulo = "Operadores", Conteudo = "Operadores aritméticos, lógicos...", Ordem = 2, Dificuldade = 1, Ativo = true },
            new Topico { Id = 3, DisciplinaId = 1, Titulo = "Condicionais", Conteudo = "If, else, switch...", Ordem = 3, Dificuldade = 2, Ativo = true },
            new Topico { Id = 4, DisciplinaId = 1, Titulo = "Laços de Repetição", Conteudo = "For, while, do-while...", Ordem = 4, Dificuldade = 2, Ativo = true },
            new Topico { Id = 5, DisciplinaId = 2, Titulo = "Arrays", Conteudo = "Trabalho com arrays...", Ordem = 1, Dificuldade = 2, Ativo = true },
            new Topico { Id = 6, DisciplinaId = 3, Titulo = "Introdução ao HTML", Conteudo = "Tags, atributos, semântica...", Ordem = 1, Dificuldade = 1, Ativo = true },
            new Topico { Id = 7, DisciplinaId = 14, Titulo = "Primeiros Passos com JavaScript", Conteudo = "Variáveis, funções básicas...", Ordem = 1, Dificuldade = 1, Ativo = true }
        };
        modelBuilder.Entity<Topico>().HasData(topicos);
    }
}
