using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiFiotec.Migrations
{
    /// <inheritdoc />
    public partial class relatorios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_relatorios",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    data = table.Column<DateTime>(type: "TEXT", nullable: false),
                    arbovirose = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    SolicitanteId = table.Column<Guid>(type: "TEXT", nullable: false),
                    semana_inicio = table.Column<int>(type: "INTEGER", nullable: false),
                    semana_termino = table.Column<int>(type: "INTEGER", nullable: false),
                    codigo_ibge = table.Column<int>(type: "INTEGER", nullable: false),
                    municipio = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    dados_relatorio = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_relatorios", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_relatorios_tbl_solicitantes_SolicitanteId",
                        column: x => x.SolicitanteId,
                        principalTable: "tbl_solicitantes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_relatorios_SolicitanteId",
                table: "tbl_relatorios",
                column: "SolicitanteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_relatorios");
        }
    }
}
