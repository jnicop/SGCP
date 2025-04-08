using System;
using System.Collections.Generic;

namespace SGCP.Models;

public partial class LaborCost
{
    public long Id { get; set; }

    public long ProductId { get; set; }

    public long LaborTypeId { get; set; }

    public DateTime? InsertDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public string? UserInsert { get; set; }

    public string? UserUpdate { get; set; }

    public bool Enable { get; set; }

    public long UnitId { get; set; }

    public decimal Quantity { get; set; }

    public virtual LaborType LaborType { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;

    public virtual Unit Unit { get; set; } = null!;
}
