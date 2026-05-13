using System;
using Microsoft.EntityFrameworkCore.Migrations;

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
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Descricao = table.Column<string>(type: "TEXT", nullable: false),
                    Icone = table.Column<string>(type: "TEXT", nullable: true),
                    Ordem = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    Ativo = table.Column<bool>(type: "INTEGER", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Areas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Sobrenome = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    SenhaHash = table.Column<string>(type: "TEXT", nullable: false),
                    Avatar = table.Column<string>(type: "TEXT", nullable: true),
                    Ativo = table.Column<bool>(type: "INTEGER", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Papel = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true, defaultValue: "usuario")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Disciplinas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AreaId = table.Column<int>(type: "INTEGER", nullable: false),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Descricao = table.Column<string>(type: "TEXT", nullable: false),
                    Imagem = table.Column<string>(type: "TEXT", nullable: true),
                    Ordem = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    Ativo = table.Column<bool>(type: "INTEGER", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "TEXT", nullable: false)
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
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UsuarioId = table.Column<int>(type: "INTEGER", nullable: true),
                    Acao = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Entidade = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Detalhes = table.Column<string>(type: "TEXT", nullable: true),
                    Data = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IpAddress = table.Column<string>(type: "TEXT", maxLength: 45, nullable: false)
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
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DisciplinaId = table.Column<int>(type: "INTEGER", nullable: false),
                    Titulo = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Conteudo = table.Column<string>(type: "TEXT", nullable: false),
                    CodigoExemplo = table.Column<string>(type: "TEXT", nullable: true),
                    VideoUrl = table.Column<string>(type: "TEXT", nullable: true),
                    Ordem = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    Dificuldade = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 1),
                    Ativo = table.Column<bool>(type: "INTEGER", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "TEXT", nullable: false)
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
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UsuarioId = table.Column<int>(type: "INTEGER", nullable: false),
                    TopicoId = table.Column<int>(type: "INTEGER", nullable: false),
                    Concluido = table.Column<bool>(type: "INTEGER", nullable: false),
                    Progresso = table.Column<int>(type: "INTEGER", nullable: true, defaultValue: 0),
                    DataInicio = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DataConclusao = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DataAtualizacao = table.Column<DateTime>(type: "TEXT", nullable: false)
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
                    { 1, true, new DateTime(2026, 5, 13, 15, 39, 36, 698, DateTimeKind.Utc).AddTicks(5485), "Conceitos básicos de programação", null, "Fundamentos", 1 },
                    { 2, true, new DateTime(2026, 5, 13, 15, 39, 36, 698, DateTimeKind.Utc).AddTicks(6104), "Desenvolvimento web frontend", null, "Frontend", 2 },
                    { 3, true, new DateTime(2026, 5, 13, 15, 39, 36, 698, DateTimeKind.Utc).AddTicks(6106), "Desenvolvimento web backend", null, "Backend", 3 },
                    { 4, true, new DateTime(2026, 5, 13, 15, 39, 36, 698, DateTimeKind.Utc).AddTicks(6107), "DevOps e infraestrutura", null, "DevOps", 4 },
                    { 5, true, new DateTime(2026, 5, 13, 15, 39, 36, 698, DateTimeKind.Utc).AddTicks(6108), "Preparação para certificações", null, "Certificações", 5 }
                });

            migrationBuilder.InsertData(
                table: "Disciplinas",
                columns: new[] { "Id", "AreaId", "Ativo", "DataCriacao", "Descricao", "Imagem", "Nome", "Ordem" },
                values: new object[,]
                {
                    { 1, 1, true, new DateTime(2026, 5, 13, 15, 39, 36, 699, DateTimeKind.Utc).AddTicks(183), "Princípios fundamentais", null, "Lógica de Programação", 1 },
                    { 2, 1, true, new DateTime(2026, 5, 13, 15, 39, 36, 699, DateTimeKind.Utc).AddTicks(802), "Arrays, Listas, Pilhas", null, "Estruturas de Dados", 2 },
                    { 3, 2, true, new DateTime(2026, 5, 13, 15, 39, 36, 699, DateTimeKind.Utc).AddTicks(804), "Markup e estilos", null, "HTML & CSS", 1 },
                    { 4, 2, true, new DateTime(2026, 5, 13, 15, 39, 36, 699, DateTimeKind.Utc).AddTicks(805), "Linguagem do navegador", null, "JavaScript", 2 },
                    { 5, 3, true, new DateTime(2026, 5, 13, 15, 39, 36, 699, DateTimeKind.Utc).AddTicks(807), "Backend com C#", null, "C# & .NET", 1 }
                });

            migrationBuilder.InsertData(
                table: "Topicos",
                columns: new[] { "Id", "Ativo", "CodigoExemplo", "Conteudo", "DataCriacao", "Dificuldade", "DisciplinaId", "Ordem", "Titulo", "VideoUrl" },
                values: new object[,]
                {
                    { 1, true, null, "Aprenda sobre variáveis...", new DateTime(2026, 5, 13, 15, 39, 36, 699, DateTimeKind.Utc).AddTicks(1340), 1, 1, 1, "Variáveis e Tipos", null },
                    { 2, true, null, "Operadores aritméticos, lógicos...", new DateTime(2026, 5, 13, 15, 39, 36, 699, DateTimeKind.Utc).AddTicks(1916), 1, 1, 2, "Operadores", null },
                    { 3, true, null, "If, else, switch...", new DateTime(2026, 5, 13, 15, 39, 36, 699, DateTimeKind.Utc).AddTicks(1919), 2, 1, 3, "Condicionais", null },
                    { 4, true, null, "For, while, do-while...", new DateTime(2026, 5, 13, 15, 39, 36, 699, DateTimeKind.Utc).AddTicks(1920), 2, 1, 4, "Laços de Repetição", null },
                    { 5, true, null, "Trabalho com arrays...", new DateTime(2026, 5, 13, 15, 39, 36, 699, DateTimeKind.Utc).AddTicks(1922), 2, 2, 1, "Arrays", null },
                    { 6, true, null, "Tags, atributos, semântica...", new DateTime(2026, 5, 13, 15, 39, 36, 699, DateTimeKind.Utc).AddTicks(1924), 1, 3, 1, "Introdução ao HTML", null },
                    { 7, true, null, "Variáveis, funções básicas...", new DateTime(2026, 5, 13, 15, 39, 36, 699, DateTimeKind.Utc).AddTicks(1925), 1, 4, 1, "Primeiros Passos com JavaScript", null }
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
