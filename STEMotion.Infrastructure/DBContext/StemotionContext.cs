using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace STEMotion.Domain.Entities;

public partial class StemotionContext : DbContext
{
    public StemotionContext()
    {
    }

    public StemotionContext(DbContextOptions<StemotionContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Answer> Answers { get; set; }

    public virtual DbSet<Chapter> Chapters { get; set; }

    public virtual DbSet<Game> Games { get; set; }

    public virtual DbSet<GameResult> GameResults { get; set; }

    public virtual DbSet<Lesson> Lessons { get; set; }

    public virtual DbSet<LessonContent> LessonContents { get; set; }

    public virtual DbSet<LessonProgress> LessonProgresses { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<Quiz> Quizzes { get; set; }

    public virtual DbSet<QuizResult> QuizResults { get; set; }

    public virtual DbSet<Report> Reports { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    public virtual DbSet<User> Users { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Server=LAPTOP-MPVKCBL2\\ERIKDEV;uid=sa;pwd=12345;database=STEMotion;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Answer>(entity =>
        {
            entity.HasKey(e => e.AnswerId).HasName("PK__Answer__33724318AE3B9F11");

            entity.ToTable("Answer");

            entity.Property(e => e.AnswerId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("answer_id");
            entity.Property(e => e.AnswerText).HasColumnName("answer_text");
            entity.Property(e => e.IsCorrect).HasColumnName("is_correct");
            entity.Property(e => e.QuestionId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("question_id");

            entity.HasOne(d => d.Question).WithMany(p => p.Answers)
                .HasForeignKey(d => d.QuestionId)
                .HasConstraintName("FK_Answer.question_id");
        });

        modelBuilder.Entity<Chapter>(entity =>
        {
            entity.HasKey(e => e.ChapterId).HasName("PK__Chapter__745EFE8755C619B4");

            entity.ToTable("Chapter");

            entity.Property(e => e.ChapterId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("chapter_id");
            entity.Property(e => e.GradeLevel).HasColumnName("grade_level");
            entity.Property(e => e.SubjectId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("subject_id");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");

            entity.HasOne(d => d.Subject).WithMany(p => p.Chapters)
                .HasForeignKey(d => d.SubjectId)
                .HasConstraintName("FK_Chapter.subject_id");
        });

        modelBuilder.Entity<Game>(entity =>
        {
            entity.HasKey(e => e.GameId).HasName("PK__Game__FFE11FCF65352AF3");

            entity.ToTable("Game");

            entity.Property(e => e.GameId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("game_id");
            entity.Property(e => e.Config).HasColumnName("config");
            entity.Property(e => e.GameType)
                .HasMaxLength(100)
                .HasColumnName("game_type");
            entity.Property(e => e.LessonId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("lesson_id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");

            entity.HasOne(d => d.Lesson).WithMany(p => p.Games)
                .HasForeignKey(d => d.LessonId)
                .HasConstraintName("FK_Game.lesson_id");
        });

        modelBuilder.Entity<GameResult>(entity =>
        {
            entity.HasKey(e => e.GameResultId).HasName("PK__GameResu__0457811029BEFFBC");

            entity.ToTable("GameResult");

            entity.Property(e => e.GameResultId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("game_result_id");
            entity.Property(e => e.GameId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("game_id");
            entity.Property(e => e.PlayedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("played_at");
            entity.Property(e => e.Score).HasColumnName("score");
            entity.Property(e => e.StudentId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("student_id");

            entity.HasOne(d => d.Game).WithMany(p => p.GameResults)
                .HasForeignKey(d => d.GameId)
                .HasConstraintName("FK_GameResult.game_id");

            entity.HasOne(d => d.Student).WithMany(p => p.GameResults)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK_GameResult.student_id");
        });

        modelBuilder.Entity<Lesson>(entity =>
        {
            entity.HasKey(e => e.LessonId).HasName("PK__Lesson__6421F7BE3858EB58");

            entity.ToTable("Lesson");

            entity.Property(e => e.LessonId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("lesson_id");
            entity.Property(e => e.ChapterId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("chapter_id");
            entity.Property(e => e.EstimatedTime).HasColumnName("estimated_time");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");

            entity.HasOne(d => d.Chapter).WithMany(p => p.Lessons)
                .HasForeignKey(d => d.ChapterId)
                .HasConstraintName("FK_Lesson.chapter_id");
        });

        modelBuilder.Entity<LessonContent>(entity =>
        {
            entity.HasKey(e => e.ContentId).HasName("PK__LessonCo__655FE5100FE93567");

            entity.ToTable("LessonContent");

            entity.Property(e => e.ContentId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("content_id");
            entity.Property(e => e.ContentData).HasColumnName("content_data");
            entity.Property(e => e.ContentType)
                .HasMaxLength(100)
                .HasColumnName("content_type");
            entity.Property(e => e.LessonId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("lesson_id");

            entity.HasOne(d => d.Lesson).WithMany(p => p.LessonContents)
                .HasForeignKey(d => d.LessonId)
                .HasConstraintName("FK_LessonContent.lesson_id");
        });

        modelBuilder.Entity<LessonProgress>(entity =>
        {
            entity.HasKey(e => e.ProgressId).HasName("PK__LessonPr__49B3D8C1E4EDD811");

            entity.ToTable("LessonProgress");

            entity.Property(e => e.ProgressId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("progress_id");
            entity.Property(e => e.CompletedAt)
                .HasColumnType("datetime")
                .HasColumnName("completed_at");
            entity.Property(e => e.LessonId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("lesson_id");
            entity.Property(e => e.Score).HasColumnName("score");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.StudentId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("student_id");

            entity.HasOne(d => d.Lesson).WithMany(p => p.LessonProgresses)
                .HasForeignKey(d => d.LessonId)
                .HasConstraintName("FK_LessonProgress.lesson_id");

            entity.HasOne(d => d.Student).WithMany(p => p.LessonProgresses)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK_LessonProgress.student_id");
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.QuestionId).HasName("PK__Question__2EC21549C217B6D2");

            entity.ToTable("Question");

            entity.Property(e => e.QuestionId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("question_id");
            entity.Property(e => e.QuestionData).HasColumnName("question_data");
            entity.Property(e => e.QuestionText).HasColumnName("question_text");
            entity.Property(e => e.QuizId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("quiz_id");

            entity.HasOne(d => d.Quiz).WithMany(p => p.Questions)
                .HasForeignKey(d => d.QuizId)
                .HasConstraintName("FK_Question.quiz_id");
        });

        modelBuilder.Entity<Quiz>(entity =>
        {
            entity.HasKey(e => e.QuizId).HasName("PK__Quiz__2D7053ECA0DA5F87");

            entity.ToTable("Quiz");

            entity.Property(e => e.QuizId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("quiz_id");
            entity.Property(e => e.LessonId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("lesson_id");
            entity.Property(e => e.QuizType)
                .HasMaxLength(100)
                .HasColumnName("quiz_type");

            entity.HasOne(d => d.Lesson).WithMany(p => p.Quizzes)
                .HasForeignKey(d => d.LessonId)
                .HasConstraintName("FK_Quiz.lesson_id");
        });

        modelBuilder.Entity<QuizResult>(entity =>
        {
            entity.HasKey(e => e.QuizResultId).HasName("PK__QuizResu__FF9391A18F0F1472");

            entity.ToTable("QuizResult");

            entity.Property(e => e.QuizResultId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("quiz_result_id");
            entity.Property(e => e.QuizId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("quiz_id");
            entity.Property(e => e.Score).HasColumnName("score");
            entity.Property(e => e.StudentId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("student_id");

            entity.HasOne(d => d.Quiz).WithMany(p => p.QuizResults)
                .HasForeignKey(d => d.QuizId)
                .HasConstraintName("FK_QuizResult.quiz_id");

            entity.HasOne(d => d.Student).WithMany(p => p.QuizResults)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK_QuizResult.student_id");
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.HasKey(e => e.ReportId).HasName("PK__Report__779B7C5893D5C5B9");

            entity.ToTable("Report");

            entity.Property(e => e.ReportId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("report_id");
            entity.Property(e => e.AvgScore).HasColumnName("avg_score");
            entity.Property(e => e.GeneratedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("generated_at");
            entity.Property(e => e.StudentId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("student_id");
            entity.Property(e => e.TotalTimeSpent).HasColumnName("total_time_spent");

            entity.HasOne(d => d.Student).WithMany(p => p.Reports)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK_Report.student_id");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Role__3213E83F7482CC0F");

            entity.ToTable("Role");

            entity.HasIndex(e => e.Name, "UQ__Role__72E12F1BC35A7CB0").IsUnique();

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Status).HasColumnName("status");
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(e => e.SubjectId).HasName("PK__Subject__5004F6607E824D7C");

            entity.ToTable("Subject");

            entity.Property(e => e.SubjectId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("subject_id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__CB9A1CFF88DFD292");

            entity.ToTable("User");

            entity.HasIndex(e => e.Email, "UQ__User__AB6E61648DC8A8A5").IsUnique();

            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("userId");
            entity.Property(e => e.AvatarUrl).HasColumnName("avatarUrl");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(255)
                .HasColumnName("firstName");
            entity.Property(e => e.GradeLevel).HasColumnName("gradeLevel");
            entity.Property(e => e.LastName)
                .HasMaxLength(255)
                .HasColumnName("lastName");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.RoleId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("roleId");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User.role_id");

            entity.HasMany(d => d.Parents).WithMany(p => p.Students)
                .UsingEntity<Dictionary<string, object>>(
                    "ParentStudent",
                    r => r.HasOne<User>().WithMany()
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_ParentStudent.parent_id"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_ParentStudent.student_id"),
                    j =>
                    {
                        j.HasKey("ParentId", "StudentId").HasName("PK__ParentSt__4005387095555429");
                        j.ToTable("ParentStudent");
                        j.IndexerProperty<string>("ParentId")
                            .HasMaxLength(50)
                            .IsUnicode(false)
                            .HasColumnName("parent_id");
                        j.IndexerProperty<string>("StudentId")
                            .HasMaxLength(50)
                            .IsUnicode(false)
                            .HasColumnName("student_id");
                    });

            entity.HasMany(d => d.Students).WithMany(p => p.Parents)
                .UsingEntity<Dictionary<string, object>>(
                    "ParentStudent",
                    r => r.HasOne<User>().WithMany()
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_ParentStudent.student_id"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_ParentStudent.parent_id"),
                    j =>
                    {
                        j.HasKey("ParentId", "StudentId").HasName("PK__ParentSt__4005387095555429");
                        j.ToTable("ParentStudent");
                        j.IndexerProperty<string>("ParentId")
                            .HasMaxLength(50)
                            .IsUnicode(false)
                            .HasColumnName("parent_id");
                        j.IndexerProperty<string>("StudentId")
                            .HasMaxLength(50)
                            .IsUnicode(false)
                            .HasColumnName("student_id");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
