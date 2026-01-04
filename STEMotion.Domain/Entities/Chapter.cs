using System;
using System.Collections.Generic;

namespace STEMotion.Domain.Entities;

public partial class Chapter
{
    public string ChapterId { get; set; } = null!;

    public string? SubjectId { get; set; }

    public int? GradeLevel { get; set; }

    public string? Title { get; set; }

    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();

    public virtual Subject? Subject { get; set; }
}
