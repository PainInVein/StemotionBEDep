using System;
using System.Collections.Generic;

namespace STEMotion.Domain.Entities;

public partial class Game
{
    public string GameId { get; set; } = null!;

    public string? LessonId { get; set; }

    public string? Name { get; set; }

    public string? GameType { get; set; }

    public string? Config { get; set; }

    public virtual ICollection<GameResult> GameResults { get; set; } = new List<GameResult>();

    public virtual Lesson? Lesson { get; set; }
}
