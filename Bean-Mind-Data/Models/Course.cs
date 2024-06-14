using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bean_Mind_Data.Models;

[Table("Course")]
public partial class Course
{
    [Key]
    public Guid Id { get; set; }

    [StringLength(50)]
    public string Title { get; set; } = null!;

    [StringLength(50)]
    public string Description { get; set; } = null!;

    public int Status { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime StartDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime EndDate { get; set; }

    public Guid? CurriculumId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? InsDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdDate { get; set; }

    public bool? DelFlg { get; set; }

    [ForeignKey("CurriculumId")]
    [InverseProperty("Courses")]
    public virtual Curriculum? Curriculum { get; set; }

    [InverseProperty("Course")]
    public virtual ICollection<Subject> Subjects { get; set; } = new List<Subject>();
}
