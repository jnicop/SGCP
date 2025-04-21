using System;
using System.Collections.Generic;

namespace SGCP.Models;

public partial class ComponentType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool Enable { get; set; }

    public virtual ICollection<Component> Components { get; set; } = new List<Component>();
}
