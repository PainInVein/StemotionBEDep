using System;
using System.Collections.Generic;

namespace STEMotion.Domain.Entities;

public partial class Subject
{
    public string SubjectId { get; set; } = null!;

    public string? Name { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Chapter> Chapters { get; set; } = new List<Chapter>();
}
