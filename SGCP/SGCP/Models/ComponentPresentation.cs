using System;
using System.Collections.Generic;

namespace SGCP.Models;

public partial class ComponentPresentation
{
    public long Id { get; set; }

    public long ComponentId { get; set; }

    public string? Code { get; set; }

    public string? Description { get; set; }

    public string? Measure { get; set; }

    public long UnitId { get; set; }

    public decimal? QuantityPerBase { get; set; }

    public decimal Price { get; set; }

    public DateTime? InsertDate { get; set; }

    public bool? Enable { get; set; }

    public decimal? BaseUnitCost { get; set; }

    public decimal? WeightGrams { get; set; }

    public decimal? LengthMeters { get; set; }

    public virtual Component Component { get; set; } = null!;

    public virtual Unit Unit { get; set; } = null!;
}
