using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Bean_Mind_Data.Models;

public partial class BeanMindContext : DbContext
{
    public BeanMindContext()
    {
    }

    public BeanMindContext(DbContextOptions<BeanMindContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Parent> Parents { get; set; }

    public virtual DbSet<School> Schools { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Teacher> Teachers { get; set; }

    public virtual DbSet<Video> Videos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=120.72.85.82,9033;Database=BeanMind;User Id=sa;Password=f0^wyhMfl*25;Encrypt=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DelFlg).HasDefaultValue(false);
            entity.Property(e => e.InsDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.UpdDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.School).WithMany(p => p.Accounts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Account _School");
        });

        modelBuilder.Entity<Parent>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DelFlg).HasDefaultValue(false);
            entity.Property(e => e.InsDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.UpdDate).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<School>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.School).WithMany(p => p.Students)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Student _School");
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DelFlg).HasDefaultValue(false);
            entity.Property(e => e.InsDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.UpdDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.School).WithMany(p => p.Teachers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("School_Id");
        });

        modelBuilder.Entity<Video>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.UploadDate).HasDefaultValueSql("(getdate())");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
