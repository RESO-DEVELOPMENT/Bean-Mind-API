using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Bean_Mind_Data.Models;

[Table("QuestionType")]
public partial class QuestionType
{
    [Key]
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public int OrderIndex { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? InsDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdDate { get; set; }

    public bool? DelFlg { get; set; }

    [InverseProperty("QuestionType")]
    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
}
