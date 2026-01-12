using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using STEMotion.Domain.Entities;
using System;
using System.Collections.Generic;

namespace STEMotion.Infrastructure.DBContext;

public partial class StemotionContext : DbContext
{
    public StemotionContext()
    {
    }

    public StemotionContext(DbContextOptions<StemotionContext> options)
        : base(options)
    {
    }

    // DbSets
    public DbSet<Role> Roles { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<ParentStudent> ParentStudents { get; set; }
    public DbSet<Grade> Grades { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<Chapter> Chapters { get; set; }
    public DbSet<Lesson> Lessons { get; set; }
    public static string GetConnectionString(string connectionStringName)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

        string connectionString = config.GetConnectionString(connectionStringName);
        return connectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(GetConnectionString("DefaultConnection"))
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("NEWID()");

            entity.Property(e => e.Name)
                .HasColumnName("name")
                .IsRequired()
                .HasMaxLength(255);

            entity.HasIndex(e => e.Name).IsUnique();

            entity.Property(e => e.Description)
                .HasColumnName("description");

            entity.Property(e => e.Status)
                .HasColumnName("status");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");
            entity.HasKey(e => e.UserId);

            entity.Property(e => e.UserId)
                .HasColumnName("userId")
                .HasDefaultValueSql("NEWID()");

            entity.Property(e => e.Email)
                .HasColumnName("email")
                .IsRequired()
                .HasMaxLength(255);

            entity.HasIndex(e => e.Email).IsUnique();

            entity.Property(e => e.Password)
                .HasColumnName("password")
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(e => e.Phone)
                .HasColumnName("phone")
                .HasMaxLength(20);

            entity.Property(e => e.FirstName)
                .HasColumnName("firstName")
                .HasMaxLength(255);

            entity.Property(e => e.LastName)
                .HasColumnName("lastName")
                .HasMaxLength(255);

            entity.Property(e => e.RoleId)
                .HasColumnName("roleId")
                .IsRequired();

            entity.Property(e => e.GradeLevel)
                .HasColumnName("gradeLevel");

            entity.Property(e => e.AvatarUrl)
                .HasColumnName("avatarUrl")
                .IsRequired(false);

            entity.Property(e => e.Status)
                .HasColumnName("status")
                .HasMaxLength(50);

            entity.Property(e => e.CreatedAt)
                .HasColumnName("createdAt")
                .HasDefaultValueSql("GETDATE()");

            // Foreign Key
            entity.HasOne(u => u.Role)
                .WithMany()
                .HasForeignKey(u => u.RoleId)
                .HasConstraintName("FK_User.role_id")
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ParentStudent>(entity =>
        {
            entity.ToTable("ParentStudent");
            entity.HasKey(ps => new { ps.ParentId, ps.StudentId });

            entity.Property(e => e.ParentId)
                .HasColumnName("parent_id");

            entity.Property(e => e.StudentId)
                .HasColumnName("student_id");

            entity.HasOne(ps => ps.Parent)
                .WithMany(u => u.StudentRelations)
                .HasForeignKey(ps => ps.ParentId)
                .HasConstraintName("FK_ParentStudent.parent_id")
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(ps => ps.Student)
                .WithMany(u => u.ParentRelations)
                .HasForeignKey(ps => ps.StudentId)
                .HasConstraintName("FK_ParentStudent.student_id")
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Grade>(entity =>
        {
            entity.ToTable("Grade");
            entity.HasKey(e => e.GradeId);

            entity.Property(e => e.GradeId)
                .HasColumnName("grade_id")
                .HasDefaultValueSql("NEWID()");

            entity.Property(e => e.GradeLevel)
                .HasColumnName("grade_level")
                .IsRequired();

            entity.HasIndex(e => e.GradeLevel).IsUnique();

            entity.Property(e => e.Name)
                .HasColumnName("name")
                .HasMaxLength(255);

            entity.Property(e => e.Description)
                .HasColumnName("description");
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.ToTable("Subject");
            entity.HasKey(e => e.SubjectId);

            entity.Property(e => e.SubjectId)
                .HasColumnName("subject_id")
                .HasDefaultValueSql("NEWID()");

            entity.Property(e => e.GradeId)
                .HasColumnName("grade_id")
                .IsRequired();

            entity.Property(e => e.Name)
                .HasColumnName("name")
                .HasMaxLength(255);

            entity.Property(e => e.Description)
                .HasColumnName("description");

            entity.HasOne(s => s.Grade)
                .WithMany()
                .HasForeignKey(s => s.GradeId)
                .HasConstraintName("FK_Subject.grade_id")
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Chapter>(entity =>
        {
            entity.ToTable("Chapter");
            entity.HasKey(e => e.ChapterId);

            entity.Property(e => e.ChapterId)
                .HasColumnName("chapter_id")
                .HasDefaultValueSql("NEWID()");

            entity.Property(e => e.SubjectId)
                .HasColumnName("subject_id")
                .IsRequired();

            entity.Property(e => e.Title)
                .HasColumnName("title")
                .HasMaxLength(255);

            entity.HasOne(c => c.Subject)
                .WithMany(s => s.Chapters)
                .HasForeignKey(c => c.SubjectId)
                .HasConstraintName("FK_Chapter.subject_id")
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Lesson>(entity =>
        {
            entity.ToTable("Lesson");
            entity.HasKey(e => e.LessonId);

            entity.Property(e => e.LessonId)
                .HasColumnName("lesson_id")
                .HasDefaultValueSql("NEWID()");

            entity.Property(e => e.ChapterId)
                .HasColumnName("chapter_id")
                .IsRequired();

            entity.Property(e => e.Title)
                .HasColumnName("title")
                .HasMaxLength(255);

            entity.Property(e => e.EstimatedTime)
                .HasColumnName("estimated_time");

            entity.HasOne(l => l.Chapter)
                .WithMany(c => c.Lessons)
                .HasForeignKey(l => l.ChapterId)
                .HasConstraintName("FK_Lesson.chapter_id")
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
