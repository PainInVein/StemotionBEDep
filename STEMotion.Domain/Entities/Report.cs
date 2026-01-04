using System;
using System.Collections.Generic;

namespace STEMotion.Domain.Entities;

public partial class Report
{
    public string ReportId { get; set; } = null!;

    public string? StudentId { get; set; }

    public int? TotalTimeSpent { get; set; }

    public double? AvgScore { get; set; }

    public DateTime? GeneratedAt { get; set; }

    public virtual User? Student { get; set; }
}
