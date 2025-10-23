using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZSports.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCanchaAndRelatedEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Deporte",
                schema: "zsports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deporte", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Establecimiento",
                schema: "zsports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    PropietarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Establecimiento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Establecimiento_AspNetUsers_PropietarioId",
                        column: x => x.PropietarioId,
                        principalSchema: "zsports",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cancha",
                schema: "zsports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Numero = table.Column<int>(type: "int", nullable: false),
                    EsIndoor = table.Column<bool>(type: "bit", nullable: false),
                    CapacidadJugadores = table.Column<int>(type: "int", nullable: false),
                    DuracionPartido = table.Column<int>(type: "int", nullable: false),
                    Activa = table.Column<bool>(type: "bit", nullable: false),
                    SuperficieId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeporteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EstablecimientoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cancha", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cancha_Deporte_DeporteId",
                        column: x => x.DeporteId,
                        principalSchema: "zsports",
                        principalTable: "Deporte",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cancha_Establecimiento_EstablecimientoId",
                        column: x => x.EstablecimientoId,
                        principalSchema: "zsports",
                        principalTable: "Establecimiento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cancha_Superficie_SuperficieId",
                        column: x => x.SuperficieId,
                        principalSchema: "zsports",
                        principalTable: "Superficie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cancha_DeporteId",
                schema: "zsports",
                table: "Cancha",
                column: "DeporteId");

            migrationBuilder.CreateIndex(
                name: "IX_Cancha_EstablecimientoId",
                schema: "zsports",
                table: "Cancha",
                column: "EstablecimientoId");

            migrationBuilder.CreateIndex(
                name: "IX_Cancha_SuperficieId",
                schema: "zsports",
                table: "Cancha",
                column: "SuperficieId");

            migrationBuilder.CreateIndex(
                name: "IX_Establecimiento_PropietarioId",
                schema: "zsports",
                table: "Establecimiento",
                column: "PropietarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cancha",
                schema: "zsports");

            migrationBuilder.DropTable(
                name: "Deporte",
                schema: "zsports");

            migrationBuilder.DropTable(
                name: "Establecimiento",
                schema: "zsports");
        }
    }
}
