using System;
using System.Collections.Generic;

namespace Bean_Mind_Data.Models;

public partial class Parent
{
    public Guid Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public DateTime? InsDate { get; set; }

    public DateTime? UpdDate { get; set; }

    public bool? DelFlg { get; set; }
}
