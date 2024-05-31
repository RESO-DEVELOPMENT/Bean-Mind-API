using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Bean_Mind_Data.Models;

[Table("Student")]
public partial class Student
{
    [Key]
    public Guid Id { get; set; }

    [StringLength(50)]
    public string FirstName { get; set; } = null!;

    [StringLength(50)]
    public string LastName { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime DateOfBirth { get; set; }

    public string ImgUrl { get; set; } = null!;

    public Guid SchoolId { get; set; }

    public Guid ParentId { get; set; }

    public Guid AccountId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? InsDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdDate { get; set; }

    public bool? DelFlg { get; set; }

    [ForeignKey("AccountId")]
    [InverseProperty("Students")]
    public virtual Account Account { get; set; } = null!;

    [ForeignKey("ParentId")]
    [InverseProperty("Students")]
    public virtual Parent Parent { get; set; } = null!;

    [ForeignKey("SchoolId")]
    [InverseProperty("Students")]
    public virtual School School { get; set; } = null!;
}
