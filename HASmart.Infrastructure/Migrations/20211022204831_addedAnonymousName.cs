using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HASmart.Infrastructure.Migrations
{
    public partial class addedAnonymousName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TipoContato",
                table: "Relatorios",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "AnonimoNome",
                table: "Cidadaos",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TipoContato",
                table: "Relatorios");

            migrationBuilder.DropColumn(
                name: "AnonimoNome",
                table: "Cidadaos");
        }
    }
}
