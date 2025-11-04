using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoFinal_LP3__Daylin_.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dentistas",
                columns: table => new
                {
                    IDDENTISTA = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NOMBREDENTISTA = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TELEFONODENTISTA = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    CORREODENTISTA = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dentistas", x => x.IDDENTISTA);
                });

            migrationBuilder.CreateTable(
                name: "Motivos",
                columns: table => new
                {
                    IDMOTIVO = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DESCRIPCIONMOTIVO = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Motivos", x => x.IDMOTIVO);
                });

            migrationBuilder.CreateTable(
                name: "Pacientes",
                columns: table => new
                {
                    IdPaciente = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NOMBREPACIENTE = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CEDULA = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    TELEFONOPACIENTE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pacientes", x => x.IdPaciente);
                });

            migrationBuilder.CreateTable(
                name: "Citas",
                columns: table => new
                {
                    IDCITA = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDPACIENTE = table.Column<int>(type: "int", nullable: false),
                    FECHA = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HORA = table.Column<TimeSpan>(type: "time", nullable: false),
                    DURACION = table.Column<int>(type: "int", nullable: false),
                    IDDENTISTA = table.Column<int>(type: "int", nullable: false),
                    IDMOTIVO = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Citas", x => x.IDCITA);
                    table.ForeignKey(
                        name: "FK_Citas_Dentistas_IDDENTISTA",
                        column: x => x.IDDENTISTA,
                        principalTable: "Dentistas",
                        principalColumn: "IDDENTISTA",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Citas_Motivos_IDMOTIVO",
                        column: x => x.IDMOTIVO,
                        principalTable: "Motivos",
                        principalColumn: "IDMOTIVO",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Citas_Pacientes_IDPACIENTE",
                        column: x => x.IDPACIENTE,
                        principalTable: "Pacientes",
                        principalColumn: "IdPaciente",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Citas_IDDENTISTA",
                table: "Citas",
                column: "IDDENTISTA");

            migrationBuilder.CreateIndex(
                name: "IX_Citas_IDMOTIVO",
                table: "Citas",
                column: "IDMOTIVO");

            migrationBuilder.CreateIndex(
                name: "IX_Citas_IDPACIENTE",
                table: "Citas",
                column: "IDPACIENTE");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Citas");

            migrationBuilder.DropTable(
                name: "Dentistas");

            migrationBuilder.DropTable(
                name: "Motivos");

            migrationBuilder.DropTable(
                name: "Pacientes");
        }
    }
}
