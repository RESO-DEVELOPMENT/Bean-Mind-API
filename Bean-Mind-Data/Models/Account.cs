using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Bean_Mind_Data.Models;

[Table("Account ")]
public partial class Account
{
    [Key]
    public Guid Id { get; set; }

    [StringLength(50)]
    public string? UserName { get; set; }

    [StringLength(500)]
    public string? Password { get; set; }

    public Guid SchoolId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? InsDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdDate { get; set; }

    public bool? DelFlg { get; set; }

    [ForeignKey("SchoolId")]
    [InverseProperty("Accounts")]
    public virtual School School { get; set; } = null!;
}
