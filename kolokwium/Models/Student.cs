using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace kolokwium.Models;

[Table("Students")]
public class Student
{
    [Key]
    public int Id { get; set; }

    [MaxLength(50)] 
    public string FirstName { get; set; } = null!;

    [MaxLength(50)] 
    public string LastName { get; set; } = null!;
    
    public int Age { get; set; }
    
    public float? EntranceExamScore { get; set; }

    public virtual ICollection<GroupAssigment> GroupAssigments { get; set; } = null!;
}