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

    public virtual DbSet<Activity> Activities { get; set; }

    public virtual DbSet<Chapter> Chapters { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Curriculum> Curricula { get; set; }

    public virtual DbSet<Document> Documents { get; set; }

    public virtual DbSet<Parent> Parents { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<QuestionAnswer> QuestionAnswers { get; set; }

    public virtual DbSet<QuestionLevel> QuestionLevels { get; set; }

    public virtual DbSet<School> Schools { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    public virtual DbSet<Teacher> Teachers { get; set; }

    public virtual DbSet<Topic> Topics { get; set; }

    public virtual DbSet<Video> Videos { get; set; }

    public virtual DbSet<WorkSheet> WorkSheets { get; set; }

    public virtual DbSet<WorksheetQuestion> WorksheetQuestions { get; set; }

    public virtual DbSet<WorksheetTemplate> WorksheetTemplates { get; set; }

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

        modelBuilder.Entity<Activity>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DelFlg).HasDefaultValue(false);
            entity.Property(e => e.InsDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.UpdDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Topic).WithMany(p => p.Activities)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Activity_Topic");
        });

        modelBuilder.Entity<Chapter>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DelFlg).HasDefaultValue(false);
            entity.Property(e => e.InsDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.UpdDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Subject).WithMany(p => p.Chapters).HasConstraintName("FK_Chapter_Subject");
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DelFlg).HasDefaultValue(false);
            entity.Property(e => e.InsDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.UpdDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Curriculum).WithMany(p => p.Courses).HasConstraintName("FK_Course_Curriculum");
        });

        modelBuilder.Entity<Curriculum>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DelFlg).HasDefaultValue(false);
            entity.Property(e => e.InsDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.UpdDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.School).WithMany(p => p.Curricula).HasConstraintName("FK_Curriculum_School");
        });

        modelBuilder.Entity<Document>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DelFlg).HasDefaultValue(false);
            entity.Property(e => e.UpdDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Activity).WithMany(p => p.Documents).HasConstraintName("FK_Document_Activity");
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

            entity.HasOne(d => d.School).WithMany(p => p.Parents)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Parent_School");
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DelFlg).HasDefaultValue(false);
            entity.Property(e => e.InsDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.UpdDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.QuestionLevel).WithMany(p => p.Questions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Question_QuestionLevel");
        });

        modelBuilder.Entity<QuestionAnswer>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DelFlg).HasDefaultValue(false);
            entity.Property(e => e.IndDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.UpdDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Question).WithMany(p => p.QuestionAnswers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QuestionAnswer_Question");
        });

        modelBuilder.Entity<QuestionLevel>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DelFlg).HasDefaultValue(false);
            entity.Property(e => e.InsDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.UpdDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.School).WithMany(p => p.QuestionLevels)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QuestionLevel_School");
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

            entity.HasOne(d => d.Course).WithMany(p => p.Subjects).HasConstraintName("FK_Subject_Course");
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

            entity.HasOne(d => d.Chapter).WithMany(p => p.Topics).HasConstraintName("FK_Topic_Chapter");
        });

        modelBuilder.Entity<Video>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DelFlg).HasDefaultValue(false);
            entity.Property(e => e.InsDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.UpdDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Activity).WithMany(p => p.Videos).HasConstraintName("FK_Video_Activity");
        });

        modelBuilder.Entity<WorkSheet>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DelFlg).HasDefaultValue(false);
            entity.Property(e => e.InsDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.UpdDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Activity).WithMany(p => p.WorkSheets).HasConstraintName("FK_WorkSheet_Activity");

            entity.HasOne(d => d.WorksheetTemplate).WithMany(p => p.WorkSheets)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WorkSheet_WorksheetTemplate");
        });

        modelBuilder.Entity<WorksheetQuestion>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DelFlg).HasDefaultValue(false);
            entity.Property(e => e.InsDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.UpdDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Question).WithMany(p => p.WorksheetQuestions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WorksheetQuestion_Question");

            entity.HasOne(d => d.Worksheet).WithMany(p => p.WorksheetQuestions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WorksheetQuestion_WorkSheet");
        });

        modelBuilder.Entity<WorksheetTemplate>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DelFlg).HasDefaultValue(false);
            entity.Property(e => e.InsDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.UpdDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Subject).WithMany(p => p.WorksheetTemplates).HasConstraintName("FK_WorksheetTemplate_Subject");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
