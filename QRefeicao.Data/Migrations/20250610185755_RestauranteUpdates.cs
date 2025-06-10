using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QRefeicao.Data.Migrations
{
    /// <inheritdoc />
    public partial class RestauranteUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bairro",
                table: "Restaurante");

            migrationBuilder.DropColumn(
                name: "CEP",
                table: "Restaurante");

            migrationBuilder.DropColumn(
                name: "Cidade",
                table: "Restaurante");

            migrationBuilder.DropColumn(
                name: "Complemento",
                table: "Restaurante");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Restaurante");

            migrationBuilder.DropColumn(
                name: "Endereco",
                table: "Restaurante");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Restaurante");

            migrationBuilder.DropColumn(
                name: "Numero",
                table: "Restaurante");

            migrationBuilder.DropColumn(
                name: "Telefone",
                table: "Restaurante");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Bairro",
                table: "Restaurante",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CEP",
                table: "Restaurante",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Cidade",
                table: "Restaurante",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Complemento",
                table: "Restaurante",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Restaurante",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Endereco",
                table: "Restaurante",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "Restaurante",
                type: "character varying(2)",
                maxLength: 2,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "Numero",
                table: "Restaurante",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "Telefone",
                table: "Restaurante",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }
    }
}
