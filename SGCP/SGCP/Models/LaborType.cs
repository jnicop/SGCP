using System;
using System.Collections.Generic;

namespace SGCP.Models;

public partial class LaborType
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal HourlyCost { get; set; }

    public bool Enable { get; set; }

    public DateTime? InsertDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public virtual ICollection<LaborCost> LaborCosts { get; set; } = new List<LaborCost>();
}
