using System;
using System.Collections.Generic;

namespace SGCP.Models;

public partial class InventoryMovement
{
    public long Id { get; set; }

    public long InventoryId { get; set; }

    public string MovementType { get; set; } = null!;

    public decimal Quantity { get; set; }

    public DateTime? Date { get; set; }

    public string? Description { get; set; }

    public DateTime? InsertDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public long? UserInsert { get; set; }

    public long? UserUpdate { get; set; }

    public bool Enable { get; set; }

    public long LocationId { get; set; }

    public virtual Inventory Inventory { get; set; } = null!;

    public virtual Location Location { get; set; } = null!;
}
