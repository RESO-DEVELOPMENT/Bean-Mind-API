using System;
using System.Collections.Generic;

namespace Bean_Mind_Data.Models;

public partial class School
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public string? Logo { get; set; }

    public string? Description { get; set; }

    public string? Email { get; set; }

    public DateTime? InsDate { get; set; }

    public DateTime? UpdDate { get; set; }

    public bool? DelFlg { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();

    public virtual ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
}
