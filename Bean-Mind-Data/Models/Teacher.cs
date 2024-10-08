﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Bean_Mind_Data.Models;

[Table("Teacher")]
public partial class Teacher
{
    [Key]
    public Guid Id { get; set; }

    [StringLength(50)]
    public string FirstName { get; set; } = null!;

    [StringLength(50)]
    public string LastName { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime DateOfBirth { get; set; }

    public string ImgUrl { get; set; } = null!;

    [StringLength(50)]
    public string Email { get; set; } = null!;

    [StringLength(50)]
    public string Phone { get; set; } = null!;

    public Guid AccountId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? InsDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdDate { get; set; }

    public bool? DelFlg { get; set; }

    [ForeignKey("AccountId")]
    [InverseProperty("Teachers")]
    public virtual Account Account { get; set; } = null!;

    [InverseProperty("Teacher")]
    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();

    [InverseProperty("Teacher")]
    public virtual ICollection<TeacherTeachable> TeacherTeachables { get; set; } = new List<TeacherTeachable>();

    [InverseProperty("Teacher")]
    public virtual ICollection<Video> Videos { get; set; } = new List<Video>();
}
