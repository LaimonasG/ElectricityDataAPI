using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Girteka_task.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Objects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Network = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Object_Type = table.Column<int>(type: "int", nullable: false),
                    Object_GV_Type = table.Column<int>(type: "int", nullable: false),
                    Object_Number = table.Column<int>(type: "int", nullable: false),
                    Pplus = table.Column<double>(type: "float", nullable: true),
                    PL_T = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Pminus = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Objects", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Objects");
        }
    }
}
