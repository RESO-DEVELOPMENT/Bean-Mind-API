using System;
using System.Collections.Generic;

namespace Bean_Mind_Data.Models;

public partial class Student
{
    public Guid Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public string? ImgUrl { get; set; }

    public Guid SchoolId { get; set; }

    public bool? DelFlg { get; set; }

    public DateTime? InsDate { get; set; }

    public DateTime? UpdDate { get; set; }

    public virtual School School { get; set; } = null!;
}
