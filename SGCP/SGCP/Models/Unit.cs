using System;
using System.Collections.Generic;

namespace SGCP.Models;

public partial class Unit
{
    public long Id { get; set; }

    public string? Name { get; set; }

    public string? Symbol { get; set; }

    public decimal? ConversionBase { get; set; }

    public DateTime? InsertDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public long? UserInsert { get; set; }

    public long? UserUpdate { get; set; }

    public bool? Enable { get; set; }

    public virtual ICollection<ComponentPresentation> ComponentPresentations { get; set; } = new List<ComponentPresentation>();

    public virtual ICollection<Component> Components { get; set; } = new List<Component>();

    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();

    public virtual ICollection<LaborCost> LaborCosts { get; set; } = new List<LaborCost>();

    public virtual ICollection<LaborType> LaborTypes { get; set; } = new List<LaborType>();

    public virtual ICollection<ProductComponent> ProductComponents { get; set; } = new List<ProductComponent>();

    public virtual ICollection<ProductPackaging> ProductPackagings { get; set; } = new List<ProductPackaging>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
