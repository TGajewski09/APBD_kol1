namespace kolokwium.DTOs;

public class EnrollmentGetDto
{
    public EnrollmentStudentGetDto Student { get; set; }
    public EnrollmentCourseGetDto Course { get; set; }
    public DateTime EnrollmentDate { get; set; }
}

public class EnrollmentStudentGetDto
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
}

public class EnrollmentCourseGetDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Teacher { get; set; }
}