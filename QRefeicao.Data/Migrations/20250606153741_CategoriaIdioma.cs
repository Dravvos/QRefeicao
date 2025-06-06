using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QRefeicao.Data.Migrations
{
    /// <inheritdoc />
    public partial class CategoriaIdioma : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "IdTGIdioma",
                table: "Categoria",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Categoria_IdTGIdioma",
                table: "Categoria",
                column: "IdTGIdioma");

            migrationBuilder.AddForeignKey(
                name: "FK_Categoria_TabelaGeralItem_IdTGIdioma",
                table: "Categoria",
                column: "IdTGIdioma",
                principalTable: "TabelaGeralItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categoria_TabelaGeralItem_IdTGIdioma",
                table: "Categoria");

            migrationBuilder.DropIndex(
                name: "IX_Categoria_IdTGIdioma",
                table: "Categoria");

            migrationBuilder.DropColumn(
                name: "IdTGIdioma",
                table: "Categoria");
        }
    }
}
