using System;
using System.Collections.Generic;

namespace SGCP.Models;

public partial class Product
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public long CategoryId { get; set; }

    public decimal? FinalPrice { get; set; }

    public string? Description { get; set; }

    public DateTime? InsertDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public long? UserInsert { get; set; }

    public long? UserUpdate { get; set; }

    public bool Enable { get; set; }

    public string? Presentation { get; set; }

    public decimal? TotalLength { get; set; }

    public long? UnitId { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<LaborCost> LaborCosts { get; set; } = new List<LaborCost>();

    public virtual ICollection<ProductComponent> ProductComponents { get; set; } = new List<ProductComponent>();

    public virtual ICollection<ProductFixedCost> ProductFixedCosts { get; set; } = new List<ProductFixedCost>();

    public virtual ICollection<ProductPackaging> ProductPackagings { get; set; } = new List<ProductPackaging>();

    public virtual ProductPrice? ProductPrice { get; set; }

    public virtual ICollection<RegionalsPrice> RegionalsPrices { get; set; } = new List<RegionalsPrice>();

    public virtual Unit? Unit { get; set; }
}
