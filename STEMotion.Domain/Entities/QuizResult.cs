using System;
using System.Collections.Generic;

namespace STEMotion.Domain.Entities;

public partial class QuizResult
{
    public string QuizResultId { get; set; } = null!;

    public string? StudentId { get; set; }

    public string? QuizId { get; set; }

    public double? Score { get; set; }

    public virtual Quiz? Quiz { get; set; }

    public virtual User? Student { get; set; }
}
