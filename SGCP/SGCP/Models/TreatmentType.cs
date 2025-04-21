using System;
using System.Collections.Generic;

namespace SGCP.Models;

public partial class TreatmentType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool Enable { get; set; }

    public virtual ICollection<ComponentTreatment> ComponentTreatments { get; set; } = new List<ComponentTreatment>();
}
