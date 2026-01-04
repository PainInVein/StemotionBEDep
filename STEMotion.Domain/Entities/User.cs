using System;
using System.Collections.Generic;

namespace STEMotion.Domain.Entities;

public partial class User
{
    public string UserId { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string RoleId { get; set; } = null!;

    public int? GradeLevel { get; set; }

    public string? AvatarUrl { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<GameResult> GameResults { get; set; } = new List<GameResult>();

    public virtual ICollection<LessonProgress> LessonProgresses { get; set; } = new List<LessonProgress>();

    public virtual ICollection<QuizResult> QuizResults { get; set; } = new List<QuizResult>();

    public virtual ICollection<Report> Reports { get; set; } = new List<Report>();

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<User> Parents { get; set; } = new List<User>();

    public virtual ICollection<User> Students { get; set; } = new List<User>();
}
