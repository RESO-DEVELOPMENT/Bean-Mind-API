using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bean_Mind_Data.Models;

[Table("QuestionLevel")]
public partial class QuestionLevel
{
    [Key]
    public Guid Id { get; set; }

    public int Level { get; set; }

    public Guid SchoolId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? InsDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdDate { get; set; }

    public bool? DelFlg { get; set; }

    [InverseProperty("QuestionLevel")]
    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();

    [ForeignKey("SchoolId")]
    [InverseProperty("QuestionLevels")]
    public virtual School School { get; set; } = null!;
}
