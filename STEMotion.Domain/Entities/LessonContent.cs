using System;
using System.Collections.Generic;

namespace STEMotion.Domain.Entities;

public partial class LessonContent
{
    public string ContentId { get; set; } = null!;

    public string? LessonId { get; set; }

    public string? ContentType { get; set; }

    public string? ContentData { get; set; }

    public virtual Lesson? Lesson { get; set; }
}
