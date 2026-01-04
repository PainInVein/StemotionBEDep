using System;
using System.Collections.Generic;

namespace STEMotion.Domain.Entities;

public partial class Lesson
{
    public string LessonId { get; set; } = null!;

    public string? ChapterId { get; set; }

    public string? Title { get; set; }

    public int? EstimatedTime { get; set; }

    public virtual Chapter? Chapter { get; set; }

    public virtual ICollection<Game> Games { get; set; } = new List<Game>();

    public virtual ICollection<LessonContent> LessonContents { get; set; } = new List<LessonContent>();

    public virtual ICollection<LessonProgress> LessonProgresses { get; set; } = new List<LessonProgress>();

    public virtual ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
}
