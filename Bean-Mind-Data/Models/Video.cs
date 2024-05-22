using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Bean_Mind_Data.Models;

[Table("Video ")]
public partial class Video
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    public string? Title { get; set; }

    [StringLength(50)]
    public string? VideoUrl { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UploadDate { get; set; }
}
