using System.ComponentModel.DataAnnotations;
using kolokwium.Models;

namespace kolokwium.DTOs;

public class CourseCreateDto
{
    [Required]
    [MaxLength(150)]
    public string Title { get; set; } = null!;
    
    [MaxLength(300)]
    public string? Credits { get; set; }
    
    [Required]
    [MaxLength(150)]
    public string Teacher { get; set; }
    
    [Required(ErrorMessage = "Lista studentów jest wymagana")]
    public ICollection<StudentCreateDto> Students { get; set; } = null!;
}

public class StudentCreateDto
{
    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; } = null!;
    
    [Required]
    [MaxLength(100)]
    public string LastName { get; set; }
    
    [MaxLength(150)]
    public string? Email { get; set; }
}
