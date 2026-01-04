using System;
using System.Collections.Generic;

namespace STEMotion.Domain.Entities;

public partial class GameResult
{
    public string GameResultId { get; set; } = null!;

    public string? StudentId { get; set; }

    public string? GameId { get; set; }

    public double? Score { get; set; }

    public DateTime? PlayedAt { get; set; }

    public virtual Game? Game { get; set; }

    public virtual User? Student { get; set; }
}
