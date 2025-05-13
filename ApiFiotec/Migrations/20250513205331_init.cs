using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiFiotec.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_estados",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    uf = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_estados", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_solicitantes",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    cpf = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_solicitantes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_municipios",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    municipio = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EstadoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_municipios", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_municipios_tbl_estados_EstadoId",
                        column: x => x.EstadoId,
                        principalTable: "tbl_estados",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_relatorios",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    data = table.Column<DateTime>(type: "datetime2", nullable: false),
                    arbovirose = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    solicitanteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    semana_inicio = table.Column<int>(type: "int", nullable: false),
                    semana_termino = table.Column<int>(type: "int", nullable: false),
                    codigo_ibge = table.Column<int>(type: "int", nullable: false),
                    municipio = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    dados_relatorio = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_relatorios", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_relatorios_tbl_solicitantes_solicitanteId",
                        column: x => x.solicitanteId,
                        principalTable: "tbl_solicitantes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_municipios_EstadoId",
                table: "tbl_municipios",
                column: "EstadoId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_relatorios_solicitanteId",
                table: "tbl_relatorios",
                column: "solicitanteId");

            migrationBuilder.CreateIndex(
                name: "idx_cpf",
                table: "tbl_solicitantes",
                column: "cpf",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_municipios");

            migrationBuilder.DropTable(
                name: "tbl_relatorios");

            migrationBuilder.DropTable(
                name: "tbl_estados");

            migrationBuilder.DropTable(
                name: "tbl_solicitantes");
        }
    }
}
