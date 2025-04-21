using System;
using System.Collections.Generic;

namespace SGCP.Models;

public partial class ComponentAttribute
{
    public long Id { get; set; }

    public long ComponentId { get; set; }

    public string AttributeName { get; set; } = null!;

    public string AttributeValue { get; set; } = null!;

    public DateTime? InsertDate { get; set; }

    public bool? Enable { get; set; }

    public virtual Component Component { get; set; } = null!;
}
