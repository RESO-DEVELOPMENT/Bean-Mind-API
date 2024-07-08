using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Bean_Mind_Data.Models;

[Table("Curriculum")]
public partial class Curriculum
{
    [Key]
    public Guid Id { get; set; }

    [StringLength(50)]
    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime StartDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime EndDate { get; set; }

    public Guid SchoolId { get; set; }

    [StringLength(50)]
    public string? CurriculumCode { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? InsDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdDate { get; set; }

    public bool? DelFlg { get; set; }

    [InverseProperty("Curriculum")]
    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

    [ForeignKey("SchoolId")]
    [InverseProperty("Curricula")]
    public virtual School School { get; set; } = null!;
}
