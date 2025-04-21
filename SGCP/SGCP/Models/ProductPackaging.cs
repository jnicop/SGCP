using System;
using System.Collections.Generic;

namespace SGCP.Models;

public partial class ProductPackaging
{
    public long Id { get; set; }

    public long ProductId { get; set; }

    public long ComponentId { get; set; }

    public decimal Quantity { get; set; }

    public long UnitId { get; set; }

    public decimal Cost { get; set; }

    public DateTime? InsertDate { get; set; }

    public bool? Enable { get; set; }

    public virtual Component Component { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;

    public virtual Unit Unit { get; set; } = null!;
}
