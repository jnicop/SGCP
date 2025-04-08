using System;
using System.Collections.Generic;

namespace SGCP.Models;

public partial class ProductType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool Enable { get; set; }

    public DateTime InsertDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public string? UserInsert { get; set; }

    public string? UserUpdate { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
