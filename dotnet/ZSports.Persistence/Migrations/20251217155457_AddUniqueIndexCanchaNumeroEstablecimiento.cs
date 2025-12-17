using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZSports.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueIndexCanchaNumeroEstablecimiento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Activa",
                schema: "zsports",
                table: "Cancha",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.CreateIndex(
                name: "UQ_Cancha_Numero_Establecimiento",
                schema: "zsports",
                table: "Cancha",
                columns: new[] { "Numero", "EstablecimientoId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UQ_Cancha_Numero_Establecimiento",
                schema: "zsports",
                table: "Cancha");

            migrationBuilder.AlterColumn<bool>(
                name: "Activa",
                schema: "zsports",
                table: "Cancha",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);
        }
    }
}
