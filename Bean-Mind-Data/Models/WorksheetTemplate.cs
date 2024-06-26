﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Bean_Mind_Data.Models;

[Table("WorksheetTemplate")]
public partial class WorksheetTemplate
{
    [Key]
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public int EasyCount { get; set; }

    public int MediumCount { get; set; }

    public int HardCount { get; set; }

    public Guid? SubjectId { get; set; }

    public Guid SchoolId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? InsDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdDate { get; set; }

    public bool? DelFlg { get; set; }

    [ForeignKey("SubjectId")]
    [InverseProperty("WorksheetTemplates")]
    public virtual Subject? Subject { get; set; }

    [InverseProperty("WorksheetTemplate")]
    public virtual ICollection<WorkSheet> WorkSheets { get; set; } = new List<WorkSheet>();
}
