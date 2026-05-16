using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CodexAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Areas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Descricao = table.Column<string>(type: "text", nullable: false),
                    Icone = table.Column<string>(type: "text", nullable: true),
                    Ordem = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Areas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Sobrenome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    SenhaHash = table.Column<string>(type: "text", nullable: false),
                    Avatar = table.Column<string>(type: "text", nullable: true),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Papel = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true, defaultValue: "usuario"),
                    Cpf = table.Column<string>(type: "text", nullable: true),
                    DataNascimento = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Genero = table.Column<string>(type: "text", nullable: true),
                    Cep = table.Column<string>(type: "text", nullable: true),
                    Rua = table.Column<string>(type: "text", nullable: true),
                    Numero = table.Column<string>(type: "text", nullable: true),
                    Complemento = table.Column<string>(type: "text", nullable: true),
                    Bairro = table.Column<string>(type: "text", nullable: true),
                    Cidade = table.Column<string>(type: "text", nullable: true),
                    Estado = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Disciplinas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AreaId = table.Column<int>(type: "integer", nullable: false),
                    Nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Descricao = table.Column<string>(type: "text", nullable: false),
                    Imagem = table.Column<string>(type: "text", nullable: true),
                    Ordem = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Disciplinas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Disciplinas_Areas_AreaId",
                        column: x => x.AreaId,
                        principalTable: "Areas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UsuarioId = table.Column<int>(type: "integer", nullable: true),
                    Acao = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Entidade = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Detalhes = table.Column<string>(type: "text", nullable: true),
                    Data = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IpAddress = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditLogs_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Topicos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DisciplinaId = table.Column<int>(type: "integer", nullable: false),
                    Titulo = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Conteudo = table.Column<string>(type: "text", nullable: false),
                    CodigoExemplo = table.Column<string>(type: "text", nullable: true),
                    VideoUrl = table.Column<string>(type: "text", nullable: true),
                    Ordem = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Dificuldade = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Topicos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Topicos_Disciplinas_DisciplinaId",
                        column: x => x.DisciplinaId,
                        principalTable: "Disciplinas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProgressosTopicos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UsuarioId = table.Column<int>(type: "integer", nullable: false),
                    TopicoId = table.Column<int>(type: "integer", nullable: false),
                    Concluido = table.Column<bool>(type: "boolean", nullable: false),
                    Progresso = table.Column<int>(type: "integer", nullable: true, defaultValue: 0),
                    DataInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataConclusao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataAtualizacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgressosTopicos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProgressosTopicos_Topicos_TopicoId",
                        column: x => x.TopicoId,
                        principalTable: "Topicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProgressosTopicos_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Areas",
                columns: new[] { "Id", "Ativo", "DataCriacao", "Descricao", "Icone", "Nome", "Ordem" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2026, 5, 16, 21, 14, 0, 642, DateTimeKind.Utc).AddTicks(7951), "Conceitos básicos de programação", null, "Fundamentos", 1 },
                    { 2, true, new DateTime(2026, 5, 16, 21, 14, 0, 642, DateTimeKind.Utc).AddTicks(8704), "Desenvolvimento web frontend", null, "Frontend", 2 },
                    { 3, true, new DateTime(2026, 5, 16, 21, 14, 0, 642, DateTimeKind.Utc).AddTicks(8707), "Desenvolvimento web backend", null, "Backend", 3 },
                    { 4, true, new DateTime(2026, 5, 16, 21, 14, 0, 642, DateTimeKind.Utc).AddTicks(8709), "DevOps e infraestrutura", null, "DevOps", 4 },
                    { 5, true, new DateTime(2026, 5, 16, 21, 14, 0, 642, DateTimeKind.Utc).AddTicks(8710), "Preparação para certificações", null, "Certificações", 5 }
                });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "Ativo", "Avatar", "Bairro", "Cep", "Cidade", "Complemento", "Cpf", "DataAtualizacao", "DataCriacao", "DataNascimento", "Email", "Estado", "Genero", "Nome", "Numero", "Papel", "Rua", "SenhaHash", "Sobrenome" },
                values: new object[] { 1, true, "https://i.pravatar.cc/150?img=1", null, null, null, null, null, new DateTime(2026, 5, 16, 21, 14, 0, 642, DateTimeKind.Utc).AddTicks(870), new DateTime(2026, 5, 13, 16, 22, 57, 366, DateTimeKind.Utc), null, "admin@codex.com.br", null, null, "Admin", null, "admin", null, "$2a$11$Z9XRi3yU00qWRB5REoBS4OulWR7setX9CC5QmL6VHa1U01K30g1sm", "Codex" });

            migrationBuilder.InsertData(
                table: "Disciplinas",
                columns: new[] { "Id", "AreaId", "Ativo", "DataCriacao", "Descricao", "Imagem", "Nome", "Ordem" },
                values: new object[,]
                {
                    { 1, 1, true, new DateTime(2026, 5, 16, 21, 14, 0, 642, DateTimeKind.Utc).AddTicks(9500), "Princípios fundamentais", null, "Lógica de Programação", 1 },
                    { 2, 1, true, new DateTime(2026, 5, 16, 21, 14, 0, 643, DateTimeKind.Utc).AddTicks(284), "Arrays, Listas, Pilhas", null, "Estruturas de Dados", 2 },
                    { 3, 2, true, new DateTime(2026, 5, 16, 21, 14, 0, 643, DateTimeKind.Utc).AddTicks(288), "Markup e estilos", null, "HTML & CSS", 1 },
                    { 4, 2, true, new DateTime(2026, 5, 16, 21, 14, 0, 643, DateTimeKind.Utc).AddTicks(290), "Linguagem do navegador", null, "JavaScript", 2 },
                    { 5, 3, true, new DateTime(2026, 5, 16, 21, 14, 0, 643, DateTimeKind.Utc).AddTicks(292), "Linguagem de programação C#", null, "C#", 1 },
                    { 6, 3, true, new DateTime(2026, 5, 16, 21, 14, 0, 643, DateTimeKind.Utc).AddTicks(295), "Framework .NET para aplicações web e desktop", null, ".NET", 2 },
                    { 7, 3, true, new DateTime(2026, 5, 16, 21, 14, 0, 643, DateTimeKind.Utc).AddTicks(297), "JavaScript no backend com Node.js", null, "JavaScript", 3 },
                    { 8, 5, true, new DateTime(2026, 5, 16, 21, 14, 0, 643, DateTimeKind.Utc).AddTicks(299), "Certificação fundamentals Microsoft Azure", null, "AZ-900", 1 },
                    { 9, 5, true, new DateTime(2026, 5, 16, 21, 14, 0, 643, DateTimeKind.Utc).AddTicks(300), "Certificação AI fundamentals Microsoft Azure", null, "AI-900", 2 },
                    { 10, 5, true, new DateTime(2026, 5, 16, 21, 14, 0, 643, DateTimeKind.Utc).AddTicks(303), "Certificação GitHub fundamentals", null, "GH-900", 3 },
                    { 11, 5, true, new DateTime(2026, 5, 16, 21, 14, 0, 643, DateTimeKind.Utc).AddTicks(304), "Certificação GitHub advanced", null, "GH-300", 4 },
                    { 12, 4, true, new DateTime(2026, 5, 16, 21, 14, 0, 643, DateTimeKind.Utc).AddTicks(306), "Sistema de controle de versão distribuído", null, "Git", 1 },
                    { 13, 4, true, new DateTime(2026, 5, 16, 21, 14, 0, 643, DateTimeKind.Utc).AddTicks(307), "Plataforma de colaboração e hospedagem de repositórios", null, "GitHub", 2 }
                });

            migrationBuilder.InsertData(
                table: "Topicos",
                columns: new[] { "Id", "Ativo", "CodigoExemplo", "Conteudo", "DataCriacao", "Dificuldade", "DisciplinaId", "Ordem", "Titulo", "VideoUrl" },
                values: new object[,]
                {
                    { 1, true, null, "Aprenda sobre variáveis...", new DateTime(2026, 5, 16, 21, 14, 0, 643, DateTimeKind.Utc).AddTicks(988), 1, 1, 1, "Variáveis e Tipos", null },
                    { 2, true, null, "Operadores aritméticos, lógicos...", new DateTime(2026, 5, 16, 21, 14, 0, 643, DateTimeKind.Utc).AddTicks(1916), 1, 1, 2, "Operadores", null },
                    { 3, true, null, "If, else, switch...", new DateTime(2026, 5, 16, 21, 14, 0, 643, DateTimeKind.Utc).AddTicks(1919), 2, 1, 3, "Condicionais", null },
                    { 4, true, null, "For, while, do-while...", new DateTime(2026, 5, 16, 21, 14, 0, 643, DateTimeKind.Utc).AddTicks(1922), 2, 1, 4, "Laços de Repetição", null },
                    { 5, true, null, "Trabalho com arrays...", new DateTime(2026, 5, 16, 21, 14, 0, 643, DateTimeKind.Utc).AddTicks(1924), 2, 2, 1, "Arrays", null },
                    { 6, true, null, "Tags, atributos, semântica...", new DateTime(2026, 5, 16, 21, 14, 0, 643, DateTimeKind.Utc).AddTicks(1927), 1, 3, 1, "Introdução ao HTML", null },
                    { 7, true, null, "Variáveis, funções básicas...", new DateTime(2026, 5, 16, 21, 14, 0, 643, DateTimeKind.Utc).AddTicks(1928), 1, 4, 1, "Primeiros Passos com JavaScript", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_UsuarioId",
                table: "AuditLogs",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Disciplinas_AreaId",
                table: "Disciplinas",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_ProgressosTopicos_TopicoId",
                table: "ProgressosTopicos",
                column: "TopicoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProgressosTopicos_UsuarioId_TopicoId",
                table: "ProgressosTopicos",
                columns: new[] { "UsuarioId", "TopicoId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Topicos_DisciplinaId",
                table: "Topicos",
                column: "DisciplinaId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "ProgressosTopicos");

            migrationBuilder.DropTable(
                name: "Topicos");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Disciplinas");

            migrationBuilder.DropTable(
                name: "Areas");
        }
    }
}
