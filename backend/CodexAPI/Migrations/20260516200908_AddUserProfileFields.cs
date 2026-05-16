using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodexAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddUserProfileFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Bairro",
                table: "Usuarios",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Cep",
                table: "Usuarios",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Cidade",
                table: "Usuarios",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Complemento",
                table: "Usuarios",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Cpf",
                table: "Usuarios",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataNascimento",
                table: "Usuarios",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "Usuarios",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Genero",
                table: "Usuarios",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Numero",
                table: "Usuarios",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Rua",
                table: "Usuarios",
                type: "TEXT",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 1,
                column: "DataCriacao",
                value: new DateTime(2026, 5, 16, 20, 9, 7, 379, DateTimeKind.Utc).AddTicks(2287));

            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 2,
                column: "DataCriacao",
                value: new DateTime(2026, 5, 16, 20, 9, 7, 379, DateTimeKind.Utc).AddTicks(3137));

            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 3,
                column: "DataCriacao",
                value: new DateTime(2026, 5, 16, 20, 9, 7, 379, DateTimeKind.Utc).AddTicks(3140));

            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 4,
                column: "DataCriacao",
                value: new DateTime(2026, 5, 16, 20, 9, 7, 379, DateTimeKind.Utc).AddTicks(3142));

            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 5,
                column: "DataCriacao",
                value: new DateTime(2026, 5, 16, 20, 9, 7, 379, DateTimeKind.Utc).AddTicks(3144));

            migrationBuilder.UpdateData(
                table: "Disciplinas",
                keyColumn: "Id",
                keyValue: 1,
                column: "DataCriacao",
                value: new DateTime(2026, 5, 16, 20, 9, 7, 379, DateTimeKind.Utc).AddTicks(3894));

            migrationBuilder.UpdateData(
                table: "Disciplinas",
                keyColumn: "Id",
                keyValue: 2,
                column: "DataCriacao",
                value: new DateTime(2026, 5, 16, 20, 9, 7, 379, DateTimeKind.Utc).AddTicks(4622));

            migrationBuilder.UpdateData(
                table: "Disciplinas",
                keyColumn: "Id",
                keyValue: 3,
                column: "DataCriacao",
                value: new DateTime(2026, 5, 16, 20, 9, 7, 379, DateTimeKind.Utc).AddTicks(4625));

            migrationBuilder.UpdateData(
                table: "Disciplinas",
                keyColumn: "Id",
                keyValue: 4,
                column: "DataCriacao",
                value: new DateTime(2026, 5, 16, 20, 9, 7, 379, DateTimeKind.Utc).AddTicks(4628));

            migrationBuilder.UpdateData(
                table: "Disciplinas",
                keyColumn: "Id",
                keyValue: 5,
                column: "DataCriacao",
                value: new DateTime(2026, 5, 16, 20, 9, 7, 379, DateTimeKind.Utc).AddTicks(4630));

            migrationBuilder.UpdateData(
                table: "Topicos",
                keyColumn: "Id",
                keyValue: 1,
                column: "DataCriacao",
                value: new DateTime(2026, 5, 16, 20, 9, 7, 379, DateTimeKind.Utc).AddTicks(5247));

            migrationBuilder.UpdateData(
                table: "Topicos",
                keyColumn: "Id",
                keyValue: 2,
                column: "DataCriacao",
                value: new DateTime(2026, 5, 16, 20, 9, 7, 379, DateTimeKind.Utc).AddTicks(6070));

            migrationBuilder.UpdateData(
                table: "Topicos",
                keyColumn: "Id",
                keyValue: 3,
                column: "DataCriacao",
                value: new DateTime(2026, 5, 16, 20, 9, 7, 379, DateTimeKind.Utc).AddTicks(6073));

            migrationBuilder.UpdateData(
                table: "Topicos",
                keyColumn: "Id",
                keyValue: 4,
                column: "DataCriacao",
                value: new DateTime(2026, 5, 16, 20, 9, 7, 379, DateTimeKind.Utc).AddTicks(6075));

            migrationBuilder.UpdateData(
                table: "Topicos",
                keyColumn: "Id",
                keyValue: 5,
                column: "DataCriacao",
                value: new DateTime(2026, 5, 16, 20, 9, 7, 379, DateTimeKind.Utc).AddTicks(6077));

            migrationBuilder.UpdateData(
                table: "Topicos",
                keyColumn: "Id",
                keyValue: 6,
                column: "DataCriacao",
                value: new DateTime(2026, 5, 16, 20, 9, 7, 379, DateTimeKind.Utc).AddTicks(6081));

            migrationBuilder.UpdateData(
                table: "Topicos",
                keyColumn: "Id",
                keyValue: 7,
                column: "DataCriacao",
                value: new DateTime(2026, 5, 16, 20, 9, 7, 379, DateTimeKind.Utc).AddTicks(6082));

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Bairro", "Cep", "Cidade", "Complemento", "Cpf", "DataAtualizacao", "DataCriacao", "DataNascimento", "Estado", "Genero", "Numero", "Rua", "SenhaHash" },
                values: new object[] { null, null, null, null, null, new DateTime(2026, 5, 16, 20, 9, 7, 378, DateTimeKind.Utc).AddTicks(4627), new DateTime(2026, 5, 13, 16, 22, 57, 366, DateTimeKind.Utc), null, null, null, null, null, "$2a$11$Z9XRi3yU00qWRB5REoBS4OulWR7setX9CC5QmL6VHa1U01K30g1sm" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bairro",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "Cep",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "Cidade",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "Complemento",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "Cpf",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "DataNascimento",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "Genero",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "Numero",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "Rua",
                table: "Usuarios");

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
        }
    }
}
