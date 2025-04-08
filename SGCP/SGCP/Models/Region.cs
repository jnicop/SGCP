using System;
using System.Collections.Generic;

namespace SGCP.Models;

public partial class Region
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime? InsertDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public long? UserInsert { get; set; }

    public long? UserUpdate { get; set; }

    public bool? Enable { get; set; }

    public virtual ICollection<RegionalsPrice> RegionalsPrices { get; set; } = new List<RegionalsPrice>();
}
