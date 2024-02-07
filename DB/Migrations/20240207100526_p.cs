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
                    fchVencimientoToken = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    rutaImagen = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.idUsuario);
                });

            migrationBuilder.CreateTable(
                name: "Publicaciones",
                columns: table => new
                {
                    idPublicacion = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    idUsuario = table.Column<int>(type: "integer", nullable: false),
                    fchPublicacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    contenidoPublicacion = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Publicaciones", x => x.idPublicacion);
                    table.ForeignKey(
                        name: "FK_Publicaciones_Usuarios_idUsuario",
                        column: x => x.idUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "idUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Seguidores",
                columns: table => new
                {
                    idSeguidores = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    idSeguidorSolicitud = table.Column<int>(type: "integer", nullable: false),
                    idSeguidorSeguido = table.Column<int>(type: "integer", nullable: false),
                    fchSeguimiento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seguidores", x => x.idSeguidores);
                    table.ForeignKey(
                        name: "FK_Seguidores_Usuarios_idSeguidorSeguido",
                        column: x => x.idSeguidorSeguido,
                        principalTable: "Usuarios",
                        principalColumn: "idUsuario",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Seguidores_Usuarios_idSeguidorSolicitud",
                        column: x => x.idSeguidorSolicitud,
                        principalTable: "Usuarios",
                        principalColumn: "idUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comentarios",
                columns: table => new
                {
                    idComentario = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    idUsuario = table.Column<int>(type: "integer", nullable: false),
                    idPublicacion = table.Column<int>(type: "integer", nullable: false),
                    contenidoComentario = table.Column<string>(type: "text", nullable: false),
                    fchComentario = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comentarios", x => x.idComentario);
                    table.ForeignKey(
                        name: "FK_Comentarios_Publicaciones_idPublicacion",
                        column: x => x.idPublicacion,
                        principalTable: "Publicaciones",
                        principalColumn: "idPublicacion",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comentarios_Usuarios_idUsuario",
                        column: x => x.idUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "idUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comentarios_idPublicacion",
                table: "Comentarios",
                column: "idPublicacion");

            migrationBuilder.CreateIndex(
                name: "IX_Comentarios_idUsuario",
                table: "Comentarios",
                column: "idUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Publicaciones_idUsuario",
                table: "Publicaciones",
                column: "idUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Seguidores_idSeguidorSeguido",
                table: "Seguidores",
                column: "idSeguidorSeguido");

            migrationBuilder.CreateIndex(
                name: "IX_Seguidores_idSeguidorSolicitud",
                table: "Seguidores",
                column: "idSeguidorSolicitud");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comentarios");

            migrationBuilder.DropTable(
                name: "Seguidores");

            migrationBuilder.DropTable(
                name: "Publicaciones");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
