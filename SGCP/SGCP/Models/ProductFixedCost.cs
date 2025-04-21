using System;
using System.Collections.Generic;

namespace SGCP.Models;

public partial class ProductFixedCost
{
    public long Id { get; set; }

    public long ProductId { get; set; }

    public string Name { get; set; } = null!;

    public decimal Amount { get; set; }

    public DateTime? InsertDate { get; set; }

    public bool? Enable { get; set; }

    public virtual Product Product { get; set; } = null!;
}
