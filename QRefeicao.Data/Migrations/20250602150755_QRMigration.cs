using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QRefeicao.Data.Migrations
{
    /// <inheritdoc />
    public partial class QRMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Restaurante",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "text", nullable: false),
                    LogoURL = table.Column<string>(type: "text", nullable: true),
                    LogoBytes = table.Column<byte[]>(type: "bytea", nullable: true),
                    CorPrincipal = table.Column<string>(type: "text", nullable: true),
                    CorSecundaria = table.Column<string>(type: "text", nullable: true),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    DataInclusao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UsuarioInclusao = table.Column<string>(type: "text", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UsuarioAlteracao = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Restaurante", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TabelaGeral",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "text", nullable: false),
                    Descricao = table.Column<string>(type: "text", nullable: false),
                    DataInclusao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UsuarioInclusao = table.Column<string>(type: "text", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UsuarioAlteracao = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TabelaGeral", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cardapio",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "text", nullable: false),
                    RestauranteId = table.Column<Guid>(type: "uuid", nullable: false),
                    Descrição = table.Column<string>(type: "text", nullable: true),
                    DataInclusao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UsuarioInclusao = table.Column<string>(type: "text", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UsuarioAlteracao = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cardapio", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cardapio_Restaurante_RestauranteId",
                        column: x => x.RestauranteId,
                        principalTable: "Restaurante",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Categoria",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RestauranteId = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "text", nullable: false),
                    OrdemExibicao = table.Column<int>(type: "integer", nullable: false),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    DataInclusao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UsuarioInclusao = table.Column<string>(type: "text", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UsuarioAlteracao = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categoria", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categoria_Restaurante_RestauranteId",
                        column: x => x.RestauranteId,
                        principalTable: "Restaurante",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TabelaGeralItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TabelaGeralId = table.Column<Guid>(type: "uuid", nullable: false),
                    Sigla = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    Descricao = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    DataInclusao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UsuarioInclusao = table.Column<string>(type: "text", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UsuarioAlteracao = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TabelaGeralItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TabelaGeralItem_TabelaGeral_TabelaGeralId",
                        column: x => x.TabelaGeralId,
                        principalTable: "TabelaGeral",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CardapioItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CardapioId = table.Column<Guid>(type: "uuid", nullable: false),
                    CategoriaId = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "text", nullable: false),
                    Descricao = table.Column<string>(type: "text", nullable: true),
                    Preco = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false),
                    ImagemURL = table.Column<string>(type: "text", nullable: true),
                    ImagemBytes = table.Column<byte[]>(type: "bytea", nullable: true),
                    DataInclusao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UsuarioInclusao = table.Column<string>(type: "text", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UsuarioAlteracao = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardapioItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardapioItem_Cardapio_CardapioId",
                        column: x => x.CardapioId,
                        principalTable: "Cardapio",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CardapioItem_Categoria_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categoria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Assinatura",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    IdTGTipoAssinatura = table.Column<Guid>(type: "uuid", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataFim = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IdTGStatusAssinatura = table.Column<Guid>(type: "uuid", nullable: false),
                    DataInclusao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assinatura", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assinatura_TabelaGeralItem_IdTGStatusAssinatura",
                        column: x => x.IdTGStatusAssinatura,
                        principalTable: "TabelaGeralItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Assinatura_TabelaGeralItem_IdTGTipoAssinatura",
                        column: x => x.IdTGTipoAssinatura,
                        principalTable: "TabelaGeralItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assinatura_IdTGStatusAssinatura",
                table: "Assinatura",
                column: "IdTGStatusAssinatura");

            migrationBuilder.CreateIndex(
                name: "IX_Assinatura_IdTGTipoAssinatura",
                table: "Assinatura",
                column: "IdTGTipoAssinatura");

            migrationBuilder.CreateIndex(
                name: "IX_Cardapio_RestauranteId",
                table: "Cardapio",
                column: "RestauranteId");

            migrationBuilder.CreateIndex(
                name: "IX_CardapioItem_CardapioId",
                table: "CardapioItem",
                column: "CardapioId");

            migrationBuilder.CreateIndex(
                name: "IX_CardapioItem_CategoriaId",
                table: "CardapioItem",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Categoria_RestauranteId",
                table: "Categoria",
                column: "RestauranteId");

            migrationBuilder.CreateIndex(
                name: "IX_TabelaGeralItem_TabelaGeralId",
                table: "TabelaGeralItem",
                column: "TabelaGeralId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Assinatura");

            migrationBuilder.DropTable(
                name: "CardapioItem");

            migrationBuilder.DropTable(
                name: "TabelaGeralItem");

            migrationBuilder.DropTable(
                name: "Cardapio");

            migrationBuilder.DropTable(
                name: "Categoria");

            migrationBuilder.DropTable(
                name: "TabelaGeral");

            migrationBuilder.DropTable(
                name: "Restaurante");
        }
    }
}
