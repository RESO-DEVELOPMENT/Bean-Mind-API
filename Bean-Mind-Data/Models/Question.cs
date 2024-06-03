using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Bean_Mind_Data.Models;

[Table("Question")]
public partial class Question
{
    [Key]
    public Guid Id { get; set; }

    public string Text { get; set; } = null!;

    public string? Image { get; set; }

    public int OrderIndex { get; set; }

    public Guid QuestionTypeId { get; set; }

    public Guid QuestionLevelId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? InsDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdDate { get; set; }

    public bool? DelFlg { get; set; }

    [InverseProperty("Question")]
    public virtual ICollection<QuestionAnswer> QuestionAnswers { get; set; } = new List<QuestionAnswer>();

    [ForeignKey("QuestionLevelId")]
    [InverseProperty("Questions")]
    public virtual QuestionLevel QuestionLevel { get; set; } = null!;

    [ForeignKey("QuestionTypeId")]
    [InverseProperty("Questions")]
    public virtual QuestionType QuestionType { get; set; } = null!;
}
