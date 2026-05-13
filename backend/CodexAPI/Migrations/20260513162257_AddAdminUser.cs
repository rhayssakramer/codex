using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodexAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "Ativo", "Avatar", "DataAtualizacao", "DataCriacao", "Email", "Nome", "Papel", "SenhaHash", "Sobrenome" },
                values: new object[] { 1, true, "https://i.pravatar.cc/150?img=1", new DateTime(2026, 5, 13, 16, 22, 57, 366, DateTimeKind.Utc).AddTicks(1014), new DateTime(2026, 5, 13, 16, 22, 57, 366, DateTimeKind.Utc).AddTicks(1836), "admin@codex.com.br", "Admin", "admin", "$2a$11$Z9XRi3yU00qWRB5REoBS4OulWR7setX9CC5QmL6VHa1U01K30g1sm", "Codex" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 1,
                column: "DataCriacao",
                value: new DateTime(2026, 5, 13, 15, 39, 36, 698, DateTimeKind.Utc).AddTicks(5485));

            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 2,
                column: "DataCriacao",
                value: new DateTime(2026, 5, 13, 15, 39, 36, 698, DateTimeKind.Utc).AddTicks(6104));

            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 3,
                column: "DataCriacao",
                value: new DateTime(2026, 5, 13, 15, 39, 36, 698, DateTimeKind.Utc).AddTicks(6106));

            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 4,
                column: "DataCriacao",
                value: new DateTime(2026, 5, 13, 15, 39, 36, 698, DateTimeKind.Utc).AddTicks(6107));

            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 5,
                column: "DataCriacao",
                value: new DateTime(2026, 5, 13, 15, 39, 36, 698, DateTimeKind.Utc).AddTicks(6108));

            migrationBuilder.UpdateData(
                table: "Disciplinas",
                keyColumn: "Id",
                keyValue: 1,
                column: "DataCriacao",
                value: new DateTime(2026, 5, 13, 15, 39, 36, 699, DateTimeKind.Utc).AddTicks(183));

            migrationBuilder.UpdateData(
                table: "Disciplinas",
                keyColumn: "Id",
                keyValue: 2,
                column: "DataCriacao",
                value: new DateTime(2026, 5, 13, 15, 39, 36, 699, DateTimeKind.Utc).AddTicks(802));

            migrationBuilder.UpdateData(
                table: "Disciplinas",
                keyColumn: "Id",
                keyValue: 3,
                column: "DataCriacao",
                value: new DateTime(2026, 5, 13, 15, 39, 36, 699, DateTimeKind.Utc).AddTicks(804));

            migrationBuilder.UpdateData(
                table: "Disciplinas",
                keyColumn: "Id",
                keyValue: 4,
                column: "DataCriacao",
                value: new DateTime(2026, 5, 13, 15, 39, 36, 699, DateTimeKind.Utc).AddTicks(805));

            migrationBuilder.UpdateData(
                table: "Disciplinas",
                keyColumn: "Id",
                keyValue: 5,
                column: "DataCriacao",
                value: new DateTime(2026, 5, 13, 15, 39, 36, 699, DateTimeKind.Utc).AddTicks(807));

            migrationBuilder.UpdateData(
                table: "Topicos",
                keyColumn: "Id",
                keyValue: 1,
                column: "DataCriacao",
                value: new DateTime(2026, 5, 13, 15, 39, 36, 699, DateTimeKind.Utc).AddTicks(1340));

            migrationBuilder.UpdateData(
                table: "Topicos",
                keyColumn: "Id",
                keyValue: 2,
                column: "DataCriacao",
                value: new DateTime(2026, 5, 13, 15, 39, 36, 699, DateTimeKind.Utc).AddTicks(1916));

            migrationBuilder.UpdateData(
                table: "Topicos",
                keyColumn: "Id",
                keyValue: 3,
                column: "DataCriacao",
                value: new DateTime(2026, 5, 13, 15, 39, 36, 699, DateTimeKind.Utc).AddTicks(1919));

            migrationBuilder.UpdateData(
                table: "Topicos",
                keyColumn: "Id",
                keyValue: 4,
                column: "DataCriacao",
                value: new DateTime(2026, 5, 13, 15, 39, 36, 699, DateTimeKind.Utc).AddTicks(1920));

            migrationBuilder.UpdateData(
                table: "Topicos",
                keyColumn: "Id",
                keyValue: 5,
                column: "DataCriacao",
                value: new DateTime(2026, 5, 13, 15, 39, 36, 699, DateTimeKind.Utc).AddTicks(1922));

            migrationBuilder.UpdateData(
                table: "Topicos",
                keyColumn: "Id",
                keyValue: 6,
                column: "DataCriacao",
                value: new DateTime(2026, 5, 13, 15, 39, 36, 699, DateTimeKind.Utc).AddTicks(1924));

            migrationBuilder.UpdateData(
                table: "Topicos",
                keyColumn: "Id",
                keyValue: 7,
                column: "DataCriacao",
                value: new DateTime(2026, 5, 13, 15, 39, 36, 699, DateTimeKind.Utc).AddTicks(1925));
        }
    }
}
