﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace APIProyectoMensajeria.Models
{
    public partial class mensajeriatecnmContext : DbContext
    {
        public mensajeriatecnmContext()
        {
        }

        public mensajeriatecnmContext(DbContextOptions<mensajeriatecnmContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Carrera> Carreras { get; set; } = null!;
        public virtual DbSet<Clase> Clases { get; set; } = null!;
        public virtual DbSet<Dato> Datos { get; set; } = null!;
        public virtual DbSet<DatosClase> DatosClases { get; set; } = null!;
        public virtual DbSet<Grupo> Grupos { get; set; } = null!;
        public virtual DbSet<Mensaje> Mensajes { get; set; } = null!;
        public virtual DbSet<Usuario> Usuarios { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql("server=127.0.0.1;user=root;password=root;database=mensajeriatecnm", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.29-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<Carrera>(entity =>
            {
                entity.ToTable("carreras");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(200)
                    .HasColumnName("nombre");
            });

            modelBuilder.Entity<Clase>(entity =>
            {
                entity.ToTable("clases");

                entity.HasIndex(e => e.IdGrupo, "fk_clase_grupo_idx");

                entity.Property(e => e.Nombre).HasMaxLength(200);

                entity.HasOne(d => d.IdGrupoNavigation)
                    .WithMany(p => p.Clases)
                    .HasForeignKey(d => d.IdGrupo)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_clase_grupo");
            });

            modelBuilder.Entity<Dato>(entity =>
            {
                entity.ToTable("datos");

                entity.Property(e => e.NoControl).HasMaxLength(45);

                entity.Property(e => e.Nombre).HasMaxLength(200);

                entity.Property(e => e.Rol).HasMaxLength(45);
            });

            modelBuilder.Entity<DatosClase>(entity =>
            {
                entity.ToTable("datos_clases");

                entity.HasIndex(e => e.IdClase, "fk_clase_datos_idx");

                entity.HasIndex(e => e.IdEstudiante, "fk_datos_clase_idx");

                entity.HasOne(d => d.IdClaseNavigation)
                    .WithMany(p => p.DatosClases)
                    .HasForeignKey(d => d.IdClase)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_clase_datos");

                entity.HasOne(d => d.IdEstudianteNavigation)
                    .WithMany(p => p.DatosClases)
                    .HasForeignKey(d => d.IdEstudiante)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_datos_clase");
            });

            modelBuilder.Entity<Grupo>(entity =>
            {
                entity.ToTable("grupos");

                entity.HasIndex(e => e.IdCarrera, "fk_grupo_carrera_idx");

                entity.Property(e => e.Clave).HasMaxLength(45);

                entity.HasOne(d => d.IdCarreraNavigation)
                    .WithMany(p => p.Grupos)
                    .HasForeignKey(d => d.IdCarrera)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_grupo_carrera");
            });

            modelBuilder.Entity<Mensaje>(entity =>
            {
                entity.ToTable("mensajes");

                entity.HasIndex(e => e.IdEmisor, "fk_emisor_usuario_idx");

                entity.HasIndex(e => e.IdRemitente, "fk_remitente_usuario_idx");

                entity.Property(e => e.Fecha)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Mensaje1).HasColumnName("Mensaje");

                entity.HasOne(d => d.IdEmisorNavigation)
                    .WithMany(p => p.MensajeIdEmisorNavigations)
                    .HasForeignKey(d => d.IdEmisor)
                    .HasConstraintName("fk_emisor_usuario");

                entity.HasOne(d => d.IdRemitenteNavigation)
                    .WithMany(p => p.MensajeIdRemitenteNavigations)
                    .HasForeignKey(d => d.IdRemitente)
                    .HasConstraintName("fk_remitente_usuario");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("usuarios");

                entity.HasIndex(e => e.IdUsuario, "fk_usuario_datos_idx");

                entity.Property(e => e.Correo).HasMaxLength(100);

                entity.Property(e => e.Password).HasMaxLength(8);

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.IdUsuario)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_usuario_datos");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}