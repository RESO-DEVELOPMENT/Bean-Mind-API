using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Bean_Mind_Data.Models;

[Table("School")]
public partial class School
{
    [Key]
    public Guid Id { get; set; }

    [Column("Name ")]
    [StringLength(50)]
    public string? Name { get; set; }

    public string? Address { get; set; }

    [Column("Phone ")]
    [StringLength(50)]
    public string? Phone { get; set; }

    public string? Logo { get; set; }

    public string? Description { get; set; }

    [StringLength(50)]
    public string? Email { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? InsDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdDate { get; set; }

    public bool? DelFlg { get; set; }

    [InverseProperty("School")]
    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    [InverseProperty("School")]
    public virtual ICollection<Student> Students { get; set; } = new List<Student>();

    [InverseProperty("School")]
    public virtual ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
}
