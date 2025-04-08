using System;
using System.Collections.Generic;

namespace SGCP.Models;

public partial class RefreshToken
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public string Token { get; set; } = null!;

    public DateTime ExpirationDate { get; set; }

    public bool IsUsed { get; set; }

    public bool IsRevoked { get; set; }

    public DateTime InsertDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public long? UserInsert { get; set; }

    public long? UserUpdate { get; set; }

    public virtual User User { get; set; } = null!;
}
