using System;
using System.Collections.Generic;

namespace SGCP.Models;

public partial class ComponentTreatment
{
    public long Id { get; set; }

    public long ComponentId { get; set; }

    public int TreatmentTypeId { get; set; }

    public decimal ExtraCost { get; set; }

    public DateTime? InsertDate { get; set; }

    public bool? Enable { get; set; }

    public virtual Component Component { get; set; } = null!;

    public virtual TreatmentType TreatmentType { get; set; } = null!;
}
