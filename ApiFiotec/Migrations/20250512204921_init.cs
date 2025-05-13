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
                    id = table.Column<ushort>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    uf = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_estados", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_municipios",
                columns: table => new
                {
                    id = table.Column<uint>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    municipio = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    EstadoId = table.Column<ushort>(type: "INTEGER", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_tbl_municipios_EstadoId",
                table: "tbl_municipios",
                column: "EstadoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_municipios");

            migrationBuilder.DropTable(
                name: "tbl_estados");
        }
    }
}
