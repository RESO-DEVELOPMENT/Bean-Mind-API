using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Bean_Mind_Data.Models;

[Table("Activity")]
public partial class Activity
{
    [Key]
    public Guid Id { get; set; }

    [StringLength(50)]
    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public Guid? TopicId { get; set; }

    public Guid SchoolId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? InsDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdDate { get; set; }

    public bool? DelFlg { get; set; }

    [InverseProperty("Activity")]
    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();

    [ForeignKey("TopicId")]
    [InverseProperty("Activities")]
    public virtual Topic? Topic { get; set; }

    [InverseProperty("Activity")]
    public virtual ICollection<Video> Videos { get; set; } = new List<Video>();

    [InverseProperty("Activity")]
    public virtual ICollection<WorkSheet> WorkSheets { get; set; } = new List<WorkSheet>();
}
