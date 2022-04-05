using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Kebele.Data.Migrations
{
    public partial class citi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Citizens",
                columns: table => new
                {
                    SSN = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    First_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Last_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DOB = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CityCode = table.Column<int>(type: "int", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Woreda = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Kebele = table.Column<int>(type: "int", nullable: false),
                    Blood_Type = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Age = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Citizens", x => x.SSN);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Citizens");
        }
    }
}
