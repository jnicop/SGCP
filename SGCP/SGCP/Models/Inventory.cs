using System;
using System.Collections.Generic;

namespace SGCP.Models;

public partial class Inventory
{
    public long Id { get; set; }

    public long EntityId { get; set; }

    public string EntityTipe { get; set; } = null!;

    public decimal? Quantity { get; set; }

    public long UnitId { get; set; }

    public decimal? Min { get; set; }

    public DateTime? InsertDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public long? UserInsert { get; set; }

    public long? UserUpdate { get; set; }

    public bool Enable { get; set; }

    public long LocationId { get; set; }

    public virtual ICollection<InventoryMovement> InventoryMovements { get; set; } = new List<InventoryMovement>();

    public virtual Unit Unit { get; set; } = null!;
}
