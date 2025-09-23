using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZSports.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSuperficie : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "zsports");

            migrationBuilder.CreateTable(
                name: "Superficie",
                schema: "zsports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false, collation: "Latin1_General_100_CI_AI_SC")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Superficie", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "UQ_Superficie_Nombre",
                schema: "zsports",
                table: "Superficie",
                column: "Nombre",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Superficie",
                schema: "zsports");
        }
    }
}
