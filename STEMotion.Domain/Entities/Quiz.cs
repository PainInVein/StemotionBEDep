using System;
using System.Collections.Generic;

namespace STEMotion.Domain.Entities;

public partial class Quiz
{
    public string QuizId { get; set; } = null!;

    public string? LessonId { get; set; }

    public string? QuizType { get; set; }

    public virtual Lesson? Lesson { get; set; }

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();

    public virtual ICollection<QuizResult> QuizResults { get; set; } = new List<QuizResult>();
}
