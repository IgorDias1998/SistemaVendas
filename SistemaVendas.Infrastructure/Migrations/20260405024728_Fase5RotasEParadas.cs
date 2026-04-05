using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaVendas.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Fase5RotasEParadas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rotas",
                columns: table => new
                {
                    RotaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CriadoPeloUsuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssociadoAoEntregadorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AtribuidoEm = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InicioEm = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TerminoEm = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rotas", x => x.RotaId);
                    table.ForeignKey(
                        name: "FK_Rotas_Usuarios_AssociadoAoEntregadorId",
                        column: x => x.AssociadoAoEntregadorId,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Rotas_Usuarios_CriadoPeloUsuarioId",
                        column: x => x.CriadoPeloUsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ParadasRota",
                columns: table => new
                {
                    ParadaRotaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RotaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeliveryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StopOrder = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CompletoEm = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParadasRota", x => x.ParadaRotaId);
                    table.ForeignKey(
                        name: "FK_ParadasRota_Deliveries_DeliveryId",
                        column: x => x.DeliveryId,
                        principalTable: "Deliveries",
                        principalColumn: "DeliveryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ParadasRota_Rotas_RotaId",
                        column: x => x.RotaId,
                        principalTable: "Rotas",
                        principalColumn: "RotaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ParadasRota_DeliveryId",
                table: "ParadasRota",
                column: "DeliveryId");

            migrationBuilder.CreateIndex(
                name: "IX_ParadasRota_RotaId",
                table: "ParadasRota",
                column: "RotaId");

            migrationBuilder.CreateIndex(
                name: "IX_Rotas_AssociadoAoEntregadorId",
                table: "Rotas",
                column: "AssociadoAoEntregadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Rotas_CriadoPeloUsuarioId",
                table: "Rotas",
                column: "CriadoPeloUsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParadasRota");

            migrationBuilder.DropTable(
                name: "Rotas");
        }
    }
}
