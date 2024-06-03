using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Bean_Mind_Data.Models;

[Table("WorkSheet")]
public partial class WorkSheet
{
    [Key]
    public Guid Id { get; set; }

    [StringLength(50)]
    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public Guid ActivityId { get; set; }

    public Guid WorksheetTemplateId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? InsDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdDate { get; set; }

    public bool? DelFlg { get; set; }

    [ForeignKey("ActivityId")]
    [InverseProperty("WorkSheets")]
    public virtual Activity Activity { get; set; } = null!;

    [InverseProperty("Worksheet")]
    public virtual ICollection<WorksheetQuestion> WorksheetQuestions { get; set; } = new List<WorksheetQuestion>();

    [ForeignKey("WorksheetTemplateId")]
    [InverseProperty("WorkSheets")]
    public virtual WorksheetTemplate WorksheetTemplate { get; set; } = null!;
}
