using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace kolokwium.Models;

[Table("Enrollment")]
[PrimaryKey(nameof(CourseId), nameof(StudentId))]
public class Enrollment
{
    
    [Column("Student_ID")]
    public int StudentId { get; set; }
    
    [Column("Group_ID")]
    public int CourseId { get; set; }

    public DateTime EnrollmentDate { get; set; }
    
    [ForeignKey(nameof(StudentId))]
    public virtual Student Student { get; set; } = null!;
    
    [ForeignKey(nameof(CourseId))]
    public virtual Course Course { get; set; } = null!;
    
}