using System;
using System.Collections.Generic;

namespace SGCP.Models;

public partial class ProductPrice
{
    public long Id { get; set; }

    public long ProductId { get; set; }

    public decimal Cost { get; set; }

    public decimal WholesaleSuggestedPrice { get; set; }

    public decimal? WholesaleRealPrice { get; set; }

    public decimal RetailSuggestedPrice { get; set; }

    public decimal? RetailRealPrice { get; set; }

    public DateTime InsertDate { get; set; }

    public DateTime UpdateDate { get; set; }

    public string? UserInsert { get; set; }

    public string? UserUpdate { get; set; }

    public bool Enable { get; set; }

    public virtual Product Product { get; set; } = null!;
}
