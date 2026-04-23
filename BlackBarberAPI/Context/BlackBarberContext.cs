using System;
using System.Collections.Generic;
using BlackBarberAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BlackBarberAPI.Data;

public partial class BlackBarberContext : DbContext
{
    public BlackBarberContext()
    {
    }

    public BlackBarberContext(DbContextOptions<BlackBarberContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AnadidoServicio> AnadidoServicios { get; set; }

    public virtual DbSet<Barbero> Barberos { get; set; }

    public virtual DbSet<BarberoServicio> BarberoServicios { get; set; }

    public virtual DbSet<Citum> Cita { get; set; }

    public virtual DbSet<DetalleCitum> DetalleCita { get; set; }

    public virtual DbSet<Perfume> Perfumes { get; set; }

    public virtual DbSet<PreferenciasCliente> PreferenciasClientes { get; set; }

    public virtual DbSet<Promocion> Promocions { get; set; }

    public virtual DbSet<Resena> Resenas { get; set; }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<Servicio> Servicios { get; set; }

    public virtual DbSet<ServicioCitum> ServicioCita { get; set; }

    public virtual DbSet<TipoServicio> TipoServicios { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<Consultas> Consultas { get; set; }

    public virtual DbSet<DiasHabiles> DiasHabiles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AnadidoServicio>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AnadidoS__3214EC076A85C40D");

            entity.ToTable("AnadidoServicio");

            entity.Property(e => e.Descripcion).HasMaxLength(150);
            entity.Property(e => e.Nombre).HasMaxLength(80);
            entity.Property(e => e.Precio).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.IdPertenecienteNavigation).WithMany(p => p.AnadidoServicios)
                .HasForeignKey(d => d.IdPerteneciente)
                .HasConstraintName("FK_Anadido_Servicio");
        });

        modelBuilder.Entity<Barbero>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Barbero__3214EC0794A1031E");

            entity.ToTable("Barbero");

            entity.Property(e => e.Estatus).HasDefaultValue(1);
            entity.Property(e => e.Nombre).HasMaxLength(80);

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Barberos)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK_Barbero_Usuario");
        });

        modelBuilder.Entity<BarberoServicio>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BarberoS__3214EC07474205BA");

            entity.ToTable("BarberoServicio");

            entity.HasOne(d => d.IdBarberoNavigation).WithMany(p => p.BarberoServicios)
                .HasForeignKey(d => d.IdBarbero)
                .HasConstraintName("FK_BarberoServicio_Barbero");

            entity.HasOne(d => d.IdServicioNavigation).WithMany(p => p.BarberoServicios)
                .HasForeignKey(d => d.IdServicio)
                .HasConstraintName("FK_BarberoServicio_Servicio");
        });

        modelBuilder.Entity<Citum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Cita__3214EC072EEF337F");

            entity.Property(e => e.Estatus).HasDefaultValue(1);
            entity.Property(e => e.FechaInicio).HasColumnType("datetime");
            entity.Property(e => e.FechaTermino).HasColumnType("datetime");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Cita)
                .HasForeignKey(d => d.IdCliente)
                .HasConstraintName("FK_Cita_Cliente");
        });

        modelBuilder.Entity<DetalleCitum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DetalleC__3214EC0761F3EF01");

            entity.Property(e => e.Precio).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.IdServicioCitaNavigation).WithMany(p => p.DetalleCita)
                .HasForeignKey(d => d.IdServicioCita)
                .HasConstraintName("FK_DetalleCita_ServicioCita");
        });

        modelBuilder.Entity<Perfume>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Perfume__3214EC07804738A8");

            entity.ToTable("Perfume");

            entity.Property(e => e.Descripcion).HasMaxLength(150);
            entity.Property(e => e.Disponible).HasDefaultValue(true);
            entity.Property(e => e.Nombre).HasMaxLength(60);
        });

        modelBuilder.Entity<PreferenciasCliente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Preferen__3214EC072C63A108");

            entity.ToTable("PreferenciasCliente");

            entity.Property(e => e.NumeroNavaja).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.PreferenciasClientes)
                .HasForeignKey(d => d.IdCliente)
                .HasConstraintName("FK_Preferencias_Cliente");

            entity.HasOne(d => d.IdPerfumeNavigation).WithMany(p => p.PreferenciasClientes)
                .HasForeignKey(d => d.IdPerfume)
                .HasConstraintName("FK_Preferencias_Perfume");
        });

        modelBuilder.Entity<Promocion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Promocio__3214EC076FE32361");

            entity.ToTable("Promocion");

            entity.Property(e => e.Descripcion).HasMaxLength(200);
        });

        modelBuilder.Entity<Resena>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Resena__3214EC073E83D4CF");

            entity.ToTable("Resena");

            entity.Property(e => e.Comentario).HasMaxLength(250);

            entity.HasOne(d => d.IdCitaNavigation).WithMany(p => p.Resenas)
                .HasForeignKey(d => d.IdCita)
                .HasConstraintName("FK_Resena_Cita");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Resenas)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK_Resena_Usuario");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Rol__3214EC07A7A31DE3");

            entity.ToTable("Rol");

            entity.HasIndex(e => e.Nombre, "UQ__Rol__75E3EFCF760A267A").IsUnique();

            entity.Property(e => e.Descripcion).HasMaxLength(150);
            entity.Property(e => e.Nombre).HasMaxLength(80);
        });

        modelBuilder.Entity<Servicio>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Servicio__3214EC07F6C3ED08");

            entity.ToTable("Servicio");

            entity.Property(e => e.Descripcion).HasMaxLength(150);
            entity.Property(e => e.Estatus).HasDefaultValue(1);
            entity.Property(e => e.Nombre).HasMaxLength(80);
            entity.Property(e => e.PrecioBase).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.IdTipoNavigation).WithMany(p => p.Servicios)
                .HasForeignKey(d => d.IdTipo)
                .HasConstraintName("FK_Servicio_TipoServicio");
        });

        modelBuilder.Entity<ServicioCitum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Servicio__3214EC07A1BF930E");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Precio).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.IdCitaNavigation).WithMany(p => p.ServicioCita)
                .HasForeignKey(d => d.IdCita)
                .HasConstraintName("FK_ServicioCita_Cita");

            entity.HasOne(d => d.IdServicioNavigation).WithMany(p => p.ServicioCita)
                .HasForeignKey(d => d.IdServicio)
                .HasConstraintName("FK_ServicioCita_Servicio");
        });

        modelBuilder.Entity<TipoServicio>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TipoServ__3214EC07D63AD8F0");

            entity.ToTable("TipoServicio");

            entity.Property(e => e.Descripcion).HasMaxLength(150);
            entity.Property(e => e.NombreCompleto).HasMaxLength(80);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Usuario__3214EC07E560C7CA");

            entity.ToTable("Usuario");

            entity.HasIndex(e => e.Username, "UQ__Usuario__536C85E4D7A7BFCD").IsUnique();

            entity.HasIndex(e => e.Correo, "UQ__Usuario__60695A19CE1E176F").IsUnique();

            entity.Property(e => e.Correo).HasMaxLength(200);
            entity.Property(e => e.Estatus).HasDefaultValue(1);
            entity.Property(e => e.HoraCreacion)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.Username).HasMaxLength(80);

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdRol)
                .HasConstraintName("FK_Usuario_Rol");
        });

        modelBuilder.Entity<Consultas>(entity =>
        {
            entity.Property(e => e.Estatus).HasDefaultValue(1);
            entity.Property(e => e.Nombre).HasMaxLength(80);
            entity.Property(e => e.Correo).HasMaxLength(200);
            entity.Property(e => e.Mensaje).HasMaxLength(450);
        });

        modelBuilder.Entity<DiasHabiles>(entity =>
        {
            entity.Property(e => e.Habil).HasDefaultValue(true);
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.HoraInicio).HasColumnType("time");
            entity.Property(e => e.HoraFin).HasColumnType("time");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
