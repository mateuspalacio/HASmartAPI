using Microsoft.EntityFrameworkCore.Migrations;

namespace HASmart.Infrastructure.Migrations
{
    public partial class addedRelatorNomeCol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RelatorNome",
                table: "Relatorios",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RelatorNome",
                table: "Relatorios");
        }
    }
}
