using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZSports.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTurnosAndInvitaciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "HoraFinMaxima",
                schema: "zsports",
                table: "Establecimiento",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 23, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "HoraInicioMinima",
                schema: "zsports",
                table: "Establecimiento",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 6, 0, 0, 0));

            migrationBuilder.CreateTable(
                name: "Turno",
                schema: "zsports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    HoraInicio = table.Column<TimeSpan>(type: "time", nullable: false),
                    HoraFin = table.Column<TimeSpan>(type: "time", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    FechaConfirmacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaCancelacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MotivoCancelacion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CanchaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsuarioCreadorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Turno", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Turno_AspNetUsers_UsuarioCreadorId",
                        column: x => x.UsuarioCreadorId,
                        principalSchema: "zsports",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Turno_Cancha_CanchaId",
                        column: x => x.CanchaId,
                        principalSchema: "zsports",
                        principalTable: "Cancha",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InvitacionTurno",
                schema: "zsports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    FechaInvitacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    FechaRespuesta = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TurnoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsuarioInvitadoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvitacionTurno", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvitacionTurno_AspNetUsers_UsuarioInvitadoId",
                        column: x => x.UsuarioInvitadoId,
                        principalSchema: "zsports",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvitacionTurno_Turno_TurnoId",
                        column: x => x.TurnoId,
                        principalSchema: "zsports",
                        principalTable: "Turno",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvitacionTurno_UsuarioInvitado",
                schema: "zsports",
                table: "InvitacionTurno",
                column: "UsuarioInvitadoId");

            migrationBuilder.CreateIndex(
                name: "UQ_InvitacionTurno_Turno_Usuario",
                schema: "zsports",
                table: "InvitacionTurno",
                columns: new[] { "TurnoId", "UsuarioInvitadoId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Turno_Cancha_Fecha_Estado",
                schema: "zsports",
                table: "Turno",
                columns: new[] { "CanchaId", "Fecha", "Estado" });

            migrationBuilder.CreateIndex(
                name: "IX_Turno_UsuarioCreador",
                schema: "zsports",
                table: "Turno",
                column: "UsuarioCreadorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvitacionTurno",
                schema: "zsports");

            migrationBuilder.DropTable(
                name: "Turno",
                schema: "zsports");

            migrationBuilder.DropColumn(
                name: "HoraFinMaxima",
                schema: "zsports",
                table: "Establecimiento");

            migrationBuilder.DropColumn(
                name: "HoraInicioMinima",
                schema: "zsports",
                table: "Establecimiento");
        }
    }
}
