using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Bean_Mind_Data.Models;

[Table("Subject")]
public partial class Subject
{
    [Key]
    public Guid Id { get; set; }

    [StringLength(50)]
    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public Guid? CourseId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? InsDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdDate { get; set; }

    public bool? DelFlg { get; set; }

    [InverseProperty("Subject")]
    public virtual ICollection<Chapter> Chapters { get; set; } = new List<Chapter>();

    [ForeignKey("CourseId")]
    [InverseProperty("Subjects")]
    public virtual Course? Course { get; set; }

    [InverseProperty("Subject")]
    public virtual ICollection<WorksheetTemplate> WorksheetTemplates { get; set; } = new List<WorksheetTemplate>();
}
