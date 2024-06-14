using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bean_Mind_Data.Models;

[Table("QuestionAnswer")]
public partial class QuestionAnswer
{
    [Key]
    public Guid Id { get; set; }

    public string Text { get; set; } = null!;

    public bool IsCorrect { get; set; }

    public Guid QuestionId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? IndDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdDate { get; set; }

    public bool? DelFlg { get; set; }

    [ForeignKey("QuestionId")]
    [InverseProperty("QuestionAnswers")]
    public virtual Question Question { get; set; } = null!;
}
