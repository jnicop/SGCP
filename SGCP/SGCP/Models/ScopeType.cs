using System;
using System.Collections.Generic;

namespace SGCP.Models;

public partial class ScopeType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool Enable { get; set; }

    public virtual ICollection<ComponentProcess> ComponentProcesses { get; set; } = new List<ComponentProcess>();
}
