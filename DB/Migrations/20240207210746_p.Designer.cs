﻿// <auto-generated />
using System;
using DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DB.Migrations
{
    [DbContext(typeof(GestorLinkSocialDbContext))]
    [Migration("20240207210746_p")]
    partial class p
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("DB.Modelo.Comentarios", b =>
                {
                    b.Property<int>("idComentario")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("idComentario"));

                    b.Property<string>("contenidoComentario")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("fchComentario")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("idPublicacion")
                        .HasColumnType("integer");

                    b.Property<int>("idUsuario")
                        .HasColumnType("integer");

                    b.HasKey("idComentario");

                    b.HasIndex("idPublicacion");

                    b.HasIndex("idUsuario");

                    b.ToTable("Comentarios");
                });

            modelBuilder.Entity("DB.Modelo.Publicaciones", b =>
                {
                    b.Property<int>("idPublicacion")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("idPublicacion"));

                    b.Property<string>("contenidoPublicacion")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("fchPublicacion")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("idUsuario")
                        .HasColumnType("integer");

                    b.HasKey("idPublicacion");

                    b.HasIndex("idUsuario");

                    b.ToTable("Publicaciones");
                });

            modelBuilder.Entity("DB.Modelo.Seguidores", b =>
                {
                    b.Property<int>("idSeguidores")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("idSeguidores"));

                    b.Property<DateTime>("fchSeguimiento")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("idSeguidorSeguido")
                        .HasColumnType("integer");

                    b.Property<int>("idSeguidorSolicitud")
                        .HasColumnType("integer");

                    b.Property<bool>("siguiendo")
                        .HasColumnType("boolean");

                    b.HasKey("idSeguidores");

                    b.HasIndex("idSeguidorSeguido");

                    b.HasIndex("idSeguidorSolicitud");

                    b.ToTable("Seguidores");
                });

            modelBuilder.Entity("DB.Modelo.Usuarios", b =>
                {
                    b.Property<int>("idUsuario")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("idUsuario"));

                    b.Property<string>("contraseña")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("correoElectronico")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("dniUsuario")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("fchNacimiento")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("fchRegistro")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("fchVencimientoToken")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("movilUsuario")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("nombreCompleto")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("rolAcceso")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("rutaImagen")
                        .HasColumnType("text");

                    b.Property<string>("tokenRecuperacion")
                        .HasColumnType("text");

                    b.HasKey("idUsuario");

                    b.ToTable("Usuarios");
                });

            modelBuilder.Entity("DB.Modelo.Comentarios", b =>
                {
                    b.HasOne("DB.Modelo.Publicaciones", "publicaciones")
                        .WithMany()
                        .HasForeignKey("idPublicacion")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DB.Modelo.Usuarios", "usuarios")
                        .WithMany()
                        .HasForeignKey("idUsuario")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("publicaciones");

                    b.Navigation("usuarios");
                });

            modelBuilder.Entity("DB.Modelo.Publicaciones", b =>
                {
                    b.HasOne("DB.Modelo.Usuarios", "usuarios")
                        .WithMany()
                        .HasForeignKey("idUsuario")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("usuarios");
                });

            modelBuilder.Entity("DB.Modelo.Seguidores", b =>
                {
                    b.HasOne("DB.Modelo.Usuarios", "usuarioSeguido")
                        .WithMany()
                        .HasForeignKey("idSeguidorSeguido")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DB.Modelo.Usuarios", "usuarioSolicitud")
                        .WithMany()
                        .HasForeignKey("idSeguidorSolicitud")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("usuarioSeguido");

                    b.Navigation("usuarioSolicitud");
                });
#pragma warning restore 612, 618
        }
    }
}
