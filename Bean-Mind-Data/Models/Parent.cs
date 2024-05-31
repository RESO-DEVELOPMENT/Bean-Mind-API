using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Bean_Mind_Data.Models;

[Table("Parent")]
public partial class Parent
{
    [Key]
    public Guid Id { get; set; }

    [StringLength(50)]
    public string FirstName { get; set; } = null!;

    [StringLength(50)]
    public string LastName { get; set; } = null!;

    [StringLength(50)]
    public string Phone { get; set; } = null!;

    [StringLength(50)]
    public string? Email { get; set; }

    public string? Address { get; set; }

    public Guid AccountId { get; set; }

    public Guid SchoolId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? InsDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdDate { get; set; }

    public bool? DelFlg { get; set; }

    [ForeignKey("AccountId")]
    [InverseProperty("Parents")]
    public virtual Account Account { get; set; } = null!;

    [ForeignKey("SchoolId")]
    [InverseProperty("Parents")]
    public virtual School School { get; set; } = null!;

    [InverseProperty("Parent")]
    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
