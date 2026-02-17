using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaVendas.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CriacaoInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Produtos",
                columns: table => new
                {
                    ProdutoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TituloProduto = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    DescricaoProduto = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PrecoProduto = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    EstoqueProduto = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CodigoProduto = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produtos", x => x.ProdutoId);
                    table.CheckConstraint("CK_Produtos_Estoque_NonNegative", "EstoqueProduto >= 0");
                    table.CheckConstraint("CK_Produtos_Preco_NonNegative", "PrecoProduto >= 0");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_CodigoProduto",
                table: "Produtos",
                column: "CodigoProduto",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Produtos");
        }
    }
}
