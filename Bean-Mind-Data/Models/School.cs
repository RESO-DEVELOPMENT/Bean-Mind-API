using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bean_Mind_Data.Models;

[Table("School")]
public partial class School
{
    [Key]
    public Guid Id { get; set; }

    [Column("Name ")]
    [StringLength(50)]
    public string Name { get; set; } = null!;

    public string Address { get; set; } = null!;

    [Column("Phone ")]
    [StringLength(50)]
    public string Phone { get; set; } = null!;

    public string Logo { get; set; } = null!;

    public string Description { get; set; } = null!;

    [StringLength(50)]
    public string Email { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? InsDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdDate { get; set; }

    public bool? DelFlg { get; set; }

    [InverseProperty("School")]
    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    [InverseProperty("School")]
    public virtual ICollection<Curriculum> Curricula { get; set; } = new List<Curriculum>();

    [InverseProperty("School")]
    public virtual ICollection<QuestionLevel> QuestionLevels { get; set; } = new List<QuestionLevel>();
}
