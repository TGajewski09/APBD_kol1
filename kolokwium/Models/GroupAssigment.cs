using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace kolokwium.Models;

[Table("GroupAssigment")]
[PrimaryKey(nameof(GroupId), nameof(StudentId))]
public class GroupAssigment
{
    
    [Column("Student_Id")]
    public int StudentId { get; set; }
    
    [Column("Group_Id")]
    public int GroupId { get; set; }
    
    [ForeignKey(nameof(StudentId))]
    public virtual Student Student { get; set; } = null!;
    
    [ForeignKey(nameof(GroupId))]
    public virtual Group Group { get; set; } = null!;
    
}