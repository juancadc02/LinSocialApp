using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DB.Migrations
{
    /// <inheritdoc />
    public partial class p : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    idUsuario = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombreCompleto = table.Column<string>(type: "text", nullable: false),
                    correoElectronico = table.Column<string>(type: "text", nullable: false),
                    dniUsuario = table.Column<string>(type: "text", nullable: false),
                    movilUsuario = table.Column<string>(type: "text", nullable: false),
                    contraseña = table.Column<string>(type: "text", nullable: false),
                    fchRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    fchNacimiento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    rolAcceso = table.Column<string>(type: "text", nullable: false),
                    tokenRecuperacion = table.Column<string>(type: "text", nullable: true),
                    fchVencimientoToken = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.idUsuario);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
