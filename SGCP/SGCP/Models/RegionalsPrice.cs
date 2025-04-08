using System;
using System.Collections.Generic;

namespace SGCP.Models;

public partial class RegionalsPrice
{
    public long Id { get; set; }

    public long ProductId { get; set; }

    public long RegionId { get; set; }

    public long CurrencyId { get; set; }

    public decimal Price { get; set; }

    public DateTime? InsertDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public long? UserInsert { get; set; }

    public long? UserUpdate { get; set; }

    public bool? Enable { get; set; }

    public virtual Currency Currency { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;

    public virtual Region Region { get; set; } = null!;
}
