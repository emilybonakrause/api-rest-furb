using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmilyApiFurb.Banco.Migrations
{
    /// <inheritdoc />
    public partial class Inicial1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "COMANDA",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    USUARIO_ID = table.Column<int>(type: "int", nullable: false),
                    NOME_USUARIO = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TELEFONE_USUARIO = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COMANDA", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PRODUTO",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    COMANDA_FK = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CODIGO = table.Column<int>(type: "int", nullable: false),
                    NOME = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PRECO = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRODUTO", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PRODUTO_COMANDA_COMANDA_FK",
                        column: x => x.COMANDA_FK,
                        principalTable: "COMANDA",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PRODUTO_COMANDA_FK",
                table: "PRODUTO",
                column: "COMANDA_FK");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PRODUTO");

            migrationBuilder.DropTable(
                name: "COMANDA");
        }
    }
}
