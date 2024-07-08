using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Bean_Mind_Data.Models;

[Table("StudentInCourse")]
public partial class StudentInCourse
{
    [Key]
    public Guid Id { get; set; }

    public Guid StudentId { get; set; }

    public Guid CourseId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? InsDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdDate { get; set; }

    public bool? DelFlg { get; set; }

    [ForeignKey("CourseId")]
    [InverseProperty("StudentInCourses")]
    public virtual Course Course { get; set; } = null!;

    [ForeignKey("StudentId")]
    [InverseProperty("StudentInCourses")]
    public virtual Student Student { get; set; } = null!;
}
