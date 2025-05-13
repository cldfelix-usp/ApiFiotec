using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiFiotec.Migrations
{
    /// <inheritdoc />
    public partial class add_tabela_solicitante : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_solicitantes",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    nome = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    cpf = table.Column<string>(type: "TEXT", maxLength: 11, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_solicitantes", x => x.id);
                });

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
                name: "tbl_solicitantes");
        }
    }
}
