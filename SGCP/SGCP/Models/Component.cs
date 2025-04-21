using System;
using System.Collections.Generic;

namespace SGCP.Models;

public partial class Component
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Type { get; set; }

    public long UnitId { get; set; }

    public decimal UnitCost { get; set; }

    public string? Description { get; set; }

    public DateTime? InsertDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public long? UserInsert { get; set; }

    public long? UserUpdate { get; set; }

    public bool Enable { get; set; }

    public long? CategoryId { get; set; }

    public int? ComponentTypeId { get; set; }

    public string? Code { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<ComponentAttribute> ComponentAttributes { get; set; } = new List<ComponentAttribute>();

    public virtual ICollection<ComponentPresentation> ComponentPresentations { get; set; } = new List<ComponentPresentation>();

    public virtual ICollection<ComponentProcess> ComponentProcesses { get; set; } = new List<ComponentProcess>();

    public virtual ICollection<ComponentTreatment> ComponentTreatments { get; set; } = new List<ComponentTreatment>();

    public virtual ComponentType? ComponentType { get; set; }

    public virtual ICollection<ProductComponent> ProductComponents { get; set; } = new List<ProductComponent>();

    public virtual ICollection<ProductPackaging> ProductPackagings { get; set; } = new List<ProductPackaging>();

    public virtual Unit Unit { get; set; } = null!;
}
