namespace kolokwium.DTOs;

public class CourseCreateResponseDto
{
    public string Message { get; set; } = "Kurs został utworzony  i studenci zostali dodani.";
    public CourseCreateResponseCourseDto Course { get; set; }
    public ICollection<CourseCreateResponseEnrollmentDto> Enrollments { get; set; }
    
}

public class CourseCreateResponseCourseDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Credits { get; set; }
    public string Teacher { get; set; }
}

public class CourseCreateResponseEnrollmentDto
{
    public int StudentId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public DateTime EnrollmentDate { get; set; }
}