using System;
using System.Collections.Generic;

namespace SGCP.Models;

public partial class Component
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Type { get; set; }

    public long UnitId { get; set; }

    public decimal UnitCost { get; set; }

    public string? Description { get; set; }

    public DateTime? InsertDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public long? UserInsert { get; set; }

    public long? UserUpdate { get; set; }

    public bool Enable { get; set; }

    public long? CategoryId { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<ProductComponent> ProductComponents { get; set; } = new List<ProductComponent>();

    public virtual Unit Unit { get; set; } = null!;
}
