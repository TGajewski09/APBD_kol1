namespace kolokwium.DTOs;

public class StudentGetDto
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
    public float? EntranceExamScore { get; set; }
    public ICollection<StudentGetDtoGroup> Groups { get; set; }
}

public class StudentGetDtoGroup
{
    public int Id { get; set; }
    public string Name { get; set; }
}