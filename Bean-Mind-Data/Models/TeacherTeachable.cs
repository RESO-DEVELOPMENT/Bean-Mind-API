using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Bean_Mind_Data.Models;

[Table("TeacherTeachable")]
public partial class TeacherTeachable
{
    [Key]
    public Guid Id { get; set; }

    public Guid TeacherId { get; set; }

    public Guid SubjectId { get; set; }

    [StringLength(50)]
    public string TeacherFirstName { get; set; } = null!;

    [StringLength(50)]
    public string TeacherLastname { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? InsDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdDate { get; set; }

    public bool? DelFlg { get; set; }

    [ForeignKey("SubjectId")]
    [InverseProperty("TeacherTeachables")]
    public virtual Subject Subject { get; set; } = null!;

    [ForeignKey("TeacherId")]
    [InverseProperty("TeacherTeachables")]
    public virtual Teacher Teacher { get; set; } = null!;
}
