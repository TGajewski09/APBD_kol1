using System.ComponentModel.DataAnnotations;

namespace kolokwium.DTOs;

public class StudentUpdateDto
{
    [MaxLength(50)] 
    [Required] 
    public string FirstName { get; set; } = null!;
    
    [MaxLength(50)]
    [Required]
    public string LastName { get; set; } = null!;
    
    [Required]
    public int Age { get; set; }
    
    public float? EntranceExamScore { get; set; }
}