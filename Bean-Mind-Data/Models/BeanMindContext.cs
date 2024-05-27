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

    public virtual DbSet<Chapter> Chapters { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Curriculum> Curricula { get; set; }

    public virtual DbSet<Parent> Parents { get; set; }

    public virtual DbSet<School> Schools { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    public virtual DbSet<Teacher> Teachers { get; set; }

    public virtual DbSet<Topic> Topics { get; set; }

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

            entity.HasOne(d => d.School).WithMany(p => p.Accounts).HasConstraintName("FK_Account _School");
        });

        modelBuilder.Entity<Chapter>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DelFlg).HasDefaultValue(false);
            entity.Property(e => e.InsDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.UpdDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Subject).WithMany(p => p.Chapters)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Chapter_Subject");
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DelFlg).HasDefaultValue(false);
            entity.Property(e => e.InsDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.UpdDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Curriculum).WithMany(p => p.Courses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Course_Curriculum");
        });

        modelBuilder.Entity<Curriculum>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DelFlg).HasDefaultValue(false);
            entity.Property(e => e.InsDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.UpdDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.School).WithMany(p => p.Curricula)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Curriculum_School");
        });

        modelBuilder.Entity<Parent>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DelFlg).HasDefaultValue(false);
            entity.Property(e => e.InsDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.UpdDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Account).WithMany(p => p.Parents)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Parent_Account ");
        });

        modelBuilder.Entity<School>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DelFlg).HasDefaultValue(false);
            entity.Property(e => e.InsDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.UpdDate).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DelFlg).HasDefaultValue(false);
            entity.Property(e => e.InsDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.UpdDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Account).WithMany(p => p.Students)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Student_Account");

            entity.HasOne(d => d.Parent).WithMany(p => p.Students)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Student_Parent");

            entity.HasOne(d => d.School).WithMany(p => p.Students)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Student_School");
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Subjects");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DelFlg).HasDefaultValue(false);
            entity.Property(e => e.InsDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.UpdDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Course).WithMany(p => p.Subjects)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Subject_Course");
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DelFlg).HasDefaultValue(false);
            entity.Property(e => e.InsDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.UpdDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Account).WithMany(p => p.Teachers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Teacher_Account");

            entity.HasOne(d => d.School).WithMany(p => p.Teachers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Teacher_School");
        });

        modelBuilder.Entity<Topic>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_NewTable");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.InsDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.UpdDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Chapter).WithMany(p => p.Topics)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Topic_Chapter");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
