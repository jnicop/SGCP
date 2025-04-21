using System;
using System.Collections.Generic;

namespace SGCP.Models;

public partial class ComponentProcess
{
    public int Id { get; set; }

    public long ComponentId { get; set; }

    public int ProcessTypeId { get; set; }

    public int ScopeTypeId { get; set; }

    public decimal QuantityApplied { get; set; }

    public decimal CostPerUnit { get; set; }

    public decimal? TotalCost { get; set; }

    public DateTime Date { get; set; }

    public string? Notes { get; set; }

    public virtual Component Component { get; set; } = null!;

    public virtual ProcessType ProcessType { get; set; } = null!;

    public virtual ScopeType ScopeType { get; set; } = null!;
}
