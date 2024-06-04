using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Bean_Mind_Data.Models;

[Table("WorksheetQuestion")]
public partial class WorksheetQuestion
{
    [Key]
    public Guid Id { get; set; }

    public Guid WorksheetId { get; set; }

    public Guid QuestionId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? InsDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdDate { get; set; }

    public bool? DelFlg { get; set; }

    [ForeignKey("QuestionId")]
    [InverseProperty("WorksheetQuestions")]
    public virtual Question Question { get; set; } = null!;

    [ForeignKey("WorksheetId")]
    [InverseProperty("WorksheetQuestions")]
    public virtual WorkSheet Worksheet { get; set; } = null!;
}
