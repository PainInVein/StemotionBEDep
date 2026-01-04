using System;
using System.Collections.Generic;

namespace STEMotion.Domain.Entities;

public partial class LessonProgress
{
    public string ProgressId { get; set; } = null!;

    public string? StudentId { get; set; }

    public string? LessonId { get; set; }

    public string? Status { get; set; }

    public double? Score { get; set; }

    public DateTime? CompletedAt { get; set; }

    public virtual Lesson? Lesson { get; set; }

    public virtual User? Student { get; set; }
}
