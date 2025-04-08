using System;
using System.Collections.Generic;

namespace SGCP.Models;

public partial class Category
{
    public long Id { get; set; }

    public string? Name { get; set; }

    public string Type { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? InsertDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public long? UserInsert { get; set; }

    public long? UserUpdate { get; set; }

    public bool? Enable { get; set; }

    public virtual ICollection<Component> Components { get; set; } = new List<Component>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
