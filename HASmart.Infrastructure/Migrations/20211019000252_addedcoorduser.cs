using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HASmart.Infrastructure.Migrations
{
    public partial class addedcoorduser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.InsertData(
                table: "Medicos",
                columns: new[] { "Id", "Crm", "Nome", "Senha" },
                values: new object[] { new Guid("9e5c300a-4aad-4644-98fd-b6c6439dd133"), null, "coordhasmart", "coord123" });

            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
