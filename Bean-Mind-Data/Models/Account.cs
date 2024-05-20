using System;
using System.Collections.Generic;

namespace Bean_Mind_Data.Models;

public partial class Account
{
    public Guid Id { get; set; }

    public string? UserName { get; set; }

    public string? Password { get; set; }

    public Guid? SchoolId { get; set; }

    public DateTime? InsDate { get; set; }

    public DateTime? UpdDate { get; set; }

    public bool? DelFlg { get; set; }

    public string Role { get; set; } = null!;

    public virtual School? School { get; set; }
}
