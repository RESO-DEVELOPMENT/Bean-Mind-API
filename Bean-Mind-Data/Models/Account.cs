using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bean_Mind_Data.Models;

[Table("Account ")]
public partial class Account
{
    [Key]
    public Guid Id { get; set; }

    [StringLength(50)]
    public string UserName { get; set; } = null!;

    [StringLength(500)]
    public string Password { get; set; } = null!;

    public Guid? SchoolId { get; set; }

    [StringLength(20)]
    public string Role { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? InsDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdDate { get; set; }

    public bool? DelFlg { get; set; }

    [InverseProperty("Account")]
    public virtual ICollection<Parent> Parents { get; set; } = new List<Parent>();

    [ForeignKey("SchoolId")]
    [InverseProperty("Accounts")]
    public virtual School? School { get; set; }

    [InverseProperty("Account")]
    public virtual ICollection<Student> Students { get; set; } = new List<Student>();

    [InverseProperty("Account")]
    public virtual ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
}
