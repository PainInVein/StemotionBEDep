using System;
using System.Collections.Generic;

namespace STEMotion.Domain.Entities;

public partial class Role
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
