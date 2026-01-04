using System;
using System.Collections.Generic;

namespace STEMotion.Domain.Entities;

public partial class Answer
{
    public string AnswerId { get; set; } = null!;

    public string? QuestionId { get; set; }

    public string? AnswerText { get; set; }

    public bool? IsCorrect { get; set; }

    public virtual Question? Question { get; set; }
}
