using System.ComponentModel.DataAnnotations;

namespace kolokwium.DTOs;

public class StudentCreateDto
{
    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string LastName { get; set; }
    
    [Required]
    public int Age { get; set; }
    
    public float? EntranceExamScore { get; set; }
    
    public ICollection<int>? GroupIds { get; set; }
}