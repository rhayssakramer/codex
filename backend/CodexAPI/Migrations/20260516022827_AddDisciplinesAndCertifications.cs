using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodexAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddDisciplinesAndCertifications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 1,
                column: "DataCriacao",
                value: new DateTime(2026, 5, 16, 2, 28, 26, 758, DateTimeKind.Utc).AddTicks(800));

            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 2,
                column: "DataCriacao",
                value: new DateTime(2026, 5, 16, 2, 28, 26, 758, DateTimeKind.Utc).AddTicks(1296));

            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 3,
                column: "DataCriacao",
                value: new DateTime(2026, 5, 16, 2, 28, 26, 758, DateTimeKind.Utc).AddTicks(1298));

            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 4,
                column: "DataCriacao",
                value: new DateTime(2026, 5, 16, 2, 28, 26, 758, DateTimeKind.Utc).AddTicks(1299));

            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 5,
                column: "DataCriacao",
                value: new DateTime(2026, 5, 16, 2, 28, 26, 758, DateTimeKind.Utc).AddTicks(1300));

            migrationBuilder.UpdateData(
                table: "Disciplinas",
                keyColumn: "Id",
                keyValue: 1,
                column: "DataCriacao",
                value: new DateTime(2026, 5, 16, 2, 28, 26, 758, DateTimeKind.Utc).AddTicks(1962));

            migrationBuilder.UpdateData(
                table: "Disciplinas",
                keyColumn: "Id",
                keyValue: 2,
                column: "DataCriacao",
                value: new DateTime(2026, 5, 16, 2, 28, 26, 758, DateTimeKind.Utc).AddTicks(2512));

            migrationBuilder.UpdateData(
                table: "Disciplinas",
                keyColumn: "Id",
                keyValue: 3,
                column: "DataCriacao",
                value: new DateTime(2026, 5, 16, 2, 28, 26, 758, DateTimeKind.Utc).AddTicks(2514));

            migrationBuilder.UpdateData(
                table: "Disciplinas",
                keyColumn: "Id",
                keyValue: 4,
                column: "DataCriacao",
                value: new DateTime(2026, 5, 16, 2, 28, 26, 758, DateTimeKind.Utc).AddTicks(2523));

            migrationBuilder.UpdateData(
                table: "Disciplinas",
                keyColumn: "Id",
                keyValue: 5,
                column: "DataCriacao",
                value: new DateTime(2026, 5, 16, 2, 28, 26, 758, DateTimeKind.Utc).AddTicks(2524));

            migrationBuilder.UpdateData(
                table: "Topicos",
                keyColumn: "Id",
                keyValue: 1,
                column: "DataCriacao",
                value: new DateTime(2026, 5, 16, 2, 28, 26, 758, DateTimeKind.Utc).AddTicks(3042));

            migrationBuilder.UpdateData(
                table: "Topicos",
                keyColumn: "Id",
                keyValue: 2,
                column: "DataCriacao",
                value: new DateTime(2026, 5, 16, 2, 28, 26, 758, DateTimeKind.Utc).AddTicks(3616));

            migrationBuilder.UpdateData(
                table: "Topicos",
                keyColumn: "Id",
                keyValue: 3,
                column: "DataCriacao",
                value: new DateTime(2026, 5, 16, 2, 28, 26, 758, DateTimeKind.Utc).AddTicks(3618));

            migrationBuilder.UpdateData(
                table: "Topicos",
                keyColumn: "Id",
                keyValue: 4,
                column: "DataCriacao",
                value: new DateTime(2026, 5, 16, 2, 28, 26, 758, DateTimeKind.Utc).AddTicks(3620));

            migrationBuilder.UpdateData(
                table: "Topicos",
                keyColumn: "Id",
                keyValue: 5,
                column: "DataCriacao",
                value: new DateTime(2026, 5, 16, 2, 28, 26, 758, DateTimeKind.Utc).AddTicks(3621));

            migrationBuilder.UpdateData(
                table: "Topicos",
                keyColumn: "Id",
                keyValue: 6,
                column: "DataCriacao",
                value: new DateTime(2026, 5, 16, 2, 28, 26, 758, DateTimeKind.Utc).AddTicks(3623));

            migrationBuilder.UpdateData(
                table: "Topicos",
                keyColumn: "Id",
                keyValue: 7,
                column: "DataCriacao",
                value: new DateTime(2026, 5, 16, 2, 28, 26, 758, DateTimeKind.Utc).AddTicks(3624));

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DataAtualizacao", "DataCriacao", "SenhaHash" },
                values: new object[] { new DateTime(2026, 5, 16, 2, 28, 26, 757, DateTimeKind.Utc).AddTicks(4843), new DateTime(2026, 5, 16, 2, 28, 26, 757, DateTimeKind.Utc).AddTicks(5749), "$2a$11$uWTRWYpaza1wg3De3/FL8u35Skmgl085aAxL1QnzGtsg/eBy5pG9O" });

            // Insert new Area for Version Control
            migrationBuilder.InsertData(
                table: "Areas",
                columns: new[] { "Id", "Ativo", "DataCriacao", "Descricao", "Icone", "Nome", "Ordem" },
                values: new object[] { 6, true, new DateTime(2026, 5, 16, 2, 28, 26, 758, DateTimeKind.Utc).AddTicks(1301), "Controle de versão e colaboração", null, "Controle de Versão", 6 });

            // Add C# and .NET to Backend area (AreaId = 3)
            migrationBuilder.InsertData(
                table: "Disciplinas",
                columns: new[] { "Id", "AreaId", "Ativo", "DataCriacao", "Descricao", "Imagem", "Nome", "Ordem" },
                values: new object[,]
                {
                    { 6, 3, true, new DateTime(2026, 5, 16, 2, 28, 26, 758, DateTimeKind.Utc).AddTicks(2000), "Linguagem de programação orientada a objetos", null, "C#", 2 },
                    { 7, 3, true, new DateTime(2026, 5, 16, 2, 28, 26, 758, DateTimeKind.Utc).AddTicks(2050), "Framework .NET para desenvolvimento backend", null, ".NET", 3 }
                });

            // Add Certificates to Certificações area (AreaId = 5)
            migrationBuilder.InsertData(
                table: "Disciplinas",
                columns: new[] { "Id", "AreaId", "Ativo", "DataCriacao", "Descricao", "Imagem", "Nome", "Ordem" },
                values: new object[,]
                {
                    { 8, 5, true, new DateTime(2026, 5, 16, 2, 28, 26, 758, DateTimeKind.Utc).AddTicks(2100), "Certificação fundamentals Microsoft Azure", null, "AZ-900", 1 },
                    { 9, 5, true, new DateTime(2026, 5, 16, 2, 28, 26, 758, DateTimeKind.Utc).AddTicks(2150), "Certificação AI fundamentals Microsoft Azure", null, "AI-900", 2 },
                    { 10, 5, true, new DateTime(2026, 5, 16, 2, 28, 26, 758, DateTimeKind.Utc).AddTicks(2200), "Certificação GitHub fundamentals", null, "GH-900", 3 },
                    { 11, 5, true, new DateTime(2026, 5, 16, 2, 28, 26, 758, DateTimeKind.Utc).AddTicks(2250), "Certificação GitHub advanced", null, "GH-300", 4 }
                });

            // Add Git and GitHub to Controle de Versão area (AreaId = 6)
            migrationBuilder.InsertData(
                table: "Disciplinas",
                columns: new[] { "Id", "AreaId", "Ativo", "DataCriacao", "Descricao", "Imagem", "Nome", "Ordem" },
                values: new object[,]
                {
                    { 12, 6, true, new DateTime(2026, 5, 16, 2, 28, 26, 758, DateTimeKind.Utc).AddTicks(2300), "Sistema de controle de versão distribuído", null, "Git", 1 },
                    { 13, 6, true, new DateTime(2026, 5, 16, 2, 28, 26, 758, DateTimeKind.Utc).AddTicks(2350), "Plataforma de colaboração e hospedagem de repositórios", null, "GitHub", 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Delete Disciplinas
            migrationBuilder.DeleteData(
                table: "Disciplinas",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Disciplinas",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Disciplinas",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Disciplinas",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Disciplinas",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Disciplinas",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Disciplinas",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Disciplinas",
                keyColumn: "Id",
                keyValue: 13);

            // Delete Area
            migrationBuilder.DeleteData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 1,
                column: "DataCriacao",
                value: new DateTime(2026, 5, 13, 16, 22, 57, 366, DateTimeKind.Utc).AddTicks(7083));

            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 2,
                column: "DataCriacao",
                value: new DateTime(2026, 5, 13, 16, 22, 57, 366, DateTimeKind.Utc).AddTicks(7550));

            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 3,
                column: "DataCriacao",
                value: new DateTime(2026, 5, 13, 16, 22, 57, 366, DateTimeKind.Utc).AddTicks(7552));

            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 4,
                column: "DataCriacao",
                value: new DateTime(2026, 5, 13, 16, 22, 57, 366, DateTimeKind.Utc).AddTicks(7553));

            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 5,
                column: "DataCriacao",
                value: new DateTime(2026, 5, 13, 16, 22, 57, 366, DateTimeKind.Utc).AddTicks(7554));

            migrationBuilder.UpdateData(
                table: "Disciplinas",
                keyColumn: "Id",
                keyValue: 1,
                column: "DataCriacao",
                value: new DateTime(2026, 5, 13, 16, 22, 57, 366, DateTimeKind.Utc).AddTicks(8116));

            migrationBuilder.UpdateData(
                table: "Disciplinas",
                keyColumn: "Id",
                keyValue: 2,
                column: "DataCriacao",
                value: new DateTime(2026, 5, 13, 16, 22, 57, 366, DateTimeKind.Utc).AddTicks(8546));

            migrationBuilder.UpdateData(
                table: "Disciplinas",
                keyColumn: "Id",
                keyValue: 3,
                column: "DataCriacao",
                value: new DateTime(2026, 5, 13, 16, 22, 57, 366, DateTimeKind.Utc).AddTicks(8548));

            migrationBuilder.UpdateData(
                table: "Disciplinas",
                keyColumn: "Id",
                keyValue: 4,
                column: "DataCriacao",
                value: new DateTime(2026, 5, 13, 16, 22, 57, 366, DateTimeKind.Utc).AddTicks(8556));

            migrationBuilder.UpdateData(
                table: "Disciplinas",
                keyColumn: "Id",
                keyValue: 5,
                column: "DataCriacao",
                value: new DateTime(2026, 5, 13, 16, 22, 57, 366, DateTimeKind.Utc).AddTicks(8557));

            migrationBuilder.UpdateData(
                table: "Topicos",
                keyColumn: "Id",
                keyValue: 1,
                column: "DataCriacao",
                value: new DateTime(2026, 5, 13, 16, 22, 57, 366, DateTimeKind.Utc).AddTicks(9025));

            migrationBuilder.UpdateData(
                table: "Topicos",
                keyColumn: "Id",
                keyValue: 2,
                column: "DataCriacao",
                value: new DateTime(2026, 5, 13, 16, 22, 57, 366, DateTimeKind.Utc).AddTicks(9567));

            migrationBuilder.UpdateData(
                table: "Topicos",
                keyColumn: "Id",
                keyValue: 3,
                column: "DataCriacao",
                value: new DateTime(2026, 5, 13, 16, 22, 57, 366, DateTimeKind.Utc).AddTicks(9569));

            migrationBuilder.UpdateData(
                table: "Topicos",
                keyColumn: "Id",
                keyValue: 4,
                column: "DataCriacao",
                value: new DateTime(2026, 5, 13, 16, 22, 57, 366, DateTimeKind.Utc).AddTicks(9571));

            migrationBuilder.UpdateData(
                table: "Topicos",
                keyColumn: "Id",
                keyValue: 5,
                column: "DataCriacao",
                value: new DateTime(2026, 5, 13, 16, 22, 57, 366, DateTimeKind.Utc).AddTicks(9572));

            migrationBuilder.UpdateData(
                table: "Topicos",
                keyColumn: "Id",
                keyValue: 6,
                column: "DataCriacao",
                value: new DateTime(2026, 5, 13, 16, 22, 57, 366, DateTimeKind.Utc).AddTicks(9574));

            migrationBuilder.UpdateData(
                table: "Topicos",
                keyColumn: "Id",
                keyValue: 7,
                column: "DataCriacao",
                value: new DateTime(2026, 5, 13, 16, 22, 57, 366, DateTimeKind.Utc).AddTicks(9575));

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DataAtualizacao", "DataCriacao", "SenhaHash" },
                values: new object[] { new DateTime(2026, 5, 13, 16, 22, 57, 366, DateTimeKind.Utc).AddTicks(1014), new DateTime(2026, 5, 13, 16, 22, 57, 366, DateTimeKind.Utc).AddTicks(1836), "$2a$11$Z9XRi3yU00qWRB5REoBS4OulWR7setX9CC5QmL6VHa1U01K30g1sm" });
        }
    }
}
