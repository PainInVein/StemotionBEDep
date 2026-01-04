using System;
using System.Collections.Generic;

namespace STEMotion.Domain.Entities;

public partial class Question
{
    public string QuestionId { get; set; } = null!;

    public string? QuizId { get; set; }

    public string? QuestionText { get; set; }

    public string? QuestionData { get; set; }

    public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();

    public virtual Quiz? Quiz { get; set; }
}
