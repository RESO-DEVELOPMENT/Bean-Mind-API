using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Bean_Mind_Data.Models;

[Table("Chapter")]
public partial class Chapter
{
    [Key]
    public Guid Id { get; set; }

    [StringLength(50)]
    public string Title { get; set; } = null!;

    [StringLength(50)]
    public string Description { get; set; } = null!;

    public Guid SubjectId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? InsDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdDate { get; set; }

    public bool? DelFlg { get; set; }

    [ForeignKey("SubjectId")]
    [InverseProperty("Chapters")]
    public virtual Subject Subject { get; set; } = null!;

    [InverseProperty("Chapter")]
    public virtual ICollection<Topic> Topics { get; set; } = new List<Topic>();
}
