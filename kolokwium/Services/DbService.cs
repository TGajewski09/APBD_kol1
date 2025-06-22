using kolokwium.Data;
using kolokwium.DTOs;
using kolokwium.Exceptions;
using kolokwium.Models;
using Microsoft.EntityFrameworkCore;

namespace kolokwium.Services;

public interface IDbService
{
    public Task<ICollection<EnrollmentGetDto>> GetEnrollmentsDetailsByAsync();
    public Task<CourseCreateResponseDto> CreateCourseWithEnrollmentsAsync(CourseCreateDto createData);
    // Task<CourseWithEnrollmentsGetDto> GetCourseWithEnrollmentsDetailsByIdAsync(int id);
}

public class DbService(AppDbContext data) : IDbService
{
    public async Task<ICollection<EnrollmentGetDto>>  GetEnrollmentsDetailsByAsync()
    {
        return await data.Enrollments.Select(enr => new EnrollmentGetDto
        {
            Student = new EnrollmentStudentGetDto
            {
                Id = enr.StudentId,
                FirstName = enr.Student.FirstName,
                LastName = enr.Student.LastName,
                Email = enr.Student.Email,
            },
            Course = new EnrollmentCourseGetDto
            {
                Id = enr.CourseId,
                Teacher = enr.Course.Teacher,
                Title = enr.Course.Title,
            },
            EnrollmentDate = enr.EnrollmentDate
        }).ToListAsync();
    }

    /*
     1. Utworzyć nowy kurs na podstawie danych z żądania. 
    2. Dla każdego przekazanego studenta: 
        a. Jeśli w bazie danych znajduje się już student o takim imieniu, nazwisku oraz emailu, dodaj go do kursu. 
        b. Jeśli nie istnieje, dodaj go do bazy, a następnie przydziel go do tworzonego kursu. 
    3. Utworzyć wpisy w tabeli Enrollment, łączące studenta z nowo utworzonym kursem.
    */
    /*
    public async Task<CourseCreateResponseDto> CreateCourseWithEnrollmentsAsync(CourseCreateDto createData)
    {
        await using var transaction = await data.Database.BeginTransactionAsync();

        try
        {
            var newCourse = new Course
            {
                Title = createData.Title,
                Credits = createData.Credits,
                Teacher = createData.Teacher,
            };
            data.Courses.Add(newCourse);
            await data.SaveChangesAsync(); // Req1

            ICollection<CourseCreateResponseEnrollmentDto> enrollments = new List<CourseCreateResponseEnrollmentDto>();

            if (createData.Students != null && createData.Students.Any())
            {
                foreach (var createStudentDto in createData.Students)
                {
                    var student = await data.Students.FirstOrDefaultAsync(student =>
                        student.FirstName == createStudentDto.FirstName
                        && student.LastName == createStudentDto.LastName
                        && student.Email == createStudentDto.Email);

                    if (student is null)
                    {
                        student = new Student
                        {
                            FirstName = createStudentDto.FirstName,
                            LastName = createStudentDto.LastName,
                            Email = createStudentDto.Email,
                        };
                        data.Students.Add(student);
                        await data.SaveChangesAsync(); // Req2
                    }

                    var newEnrollment = new Enrollment
                    {
                        CourseId = newCourse.Id,
                        StudentId = student.Id,
                        EnrollmentDate = DateTime.Now
                    };
                    data.Enrollments.Add(newEnrollment);
                    await data.SaveChangesAsync(); // Req3

                    CourseCreateResponseEnrollmentDto response = new CourseCreateResponseEnrollmentDto()
                    {
                        StudentId = newEnrollment.StudentId,
                        FirstName = student.FirstName,
                        LastName = student.LastName,
                        Email = student.Email,
                        EnrollmentDate = newEnrollment.EnrollmentDate
                    };
                    enrollments.Add(response);
                }
            }
            
            await transaction.CommitAsync();

            CourseCreateResponseDto returnResponse = new CourseCreateResponseDto
            {
                Course = new CourseCreateResponseCourseDto
                {
                    Id = newCourse.Id,
                    Title = newCourse.Title,
                    Credits = newCourse.Credits,
                    Teacher = newCourse.Teacher,
                },
                Enrollments = enrollments
            };

            return returnResponse;
        }
        catch(Exception e)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
    */
    
    // Wersja z grupowym dodawaniem
    public async Task<CourseCreateResponseDto> CreateCourseWithEnrollmentsAsync(CourseCreateDto createData)
    {
        await using var transaction = await data.Database.BeginTransactionAsync();

        try
        {
            var newCourse = new Course
            {
                Title = createData.Title,
                Credits = createData.Credits,
                Teacher = createData.Teacher,
            };
            data.Courses.Add(newCourse);
            await data.SaveChangesAsync(); // Req1
            
            var enrolledStudent = new List<Student>();
            var studentsToCreate = new List<Student>();
            if (createData.Students != null && createData.Students.Any())
            {
                foreach (var createStudentDto in createData.Students)
                {
                    var student = await data.Students.FirstOrDefaultAsync(student =>
                        student.FirstName == createStudentDto.FirstName
                        && student.LastName == createStudentDto.LastName
                        && student.Email == createStudentDto.Email);

                    if (student is null)
                    {
                        student = new Student
                        {
                            FirstName = createStudentDto.FirstName,
                            LastName = createStudentDto.LastName,
                            Email = createStudentDto.Email,
                        };
                        studentsToCreate.Add(student);
                    }

                    enrolledStudent.Add(student);
                }
            }
            
            data.Students.AddRange(studentsToCreate);
            await data.SaveChangesAsync();
            
            var newEnrollments = enrolledStudent.Select(student => new Enrollment()
            {
                CourseId = newCourse.Id,
                StudentId = student.Id,
                EnrollmentDate = DateTime.Now
            }).ToList();
            
            data.Enrollments.AddRange(newEnrollments);
            await data.SaveChangesAsync();
            await transaction.CommitAsync();

            var enrollments = newEnrollments.Select(enr => new CourseCreateResponseEnrollmentDto
            {
                StudentId = enr.StudentId,
                FirstName = enr.Student.FirstName,
                LastName = enr.Student.LastName,
                Email = enr.Student.Email,
                EnrollmentDate = enr.EnrollmentDate,
            }).ToList();


            CourseCreateResponseDto returnResponse = new CourseCreateResponseDto
            {
                Course = new CourseCreateResponseCourseDto
                {
                    Id = newCourse.Id,
                    Title = newCourse.Title,
                    Credits = newCourse.Credits,
                    Teacher = newCourse.Teacher,
                },
                Enrollments = enrollments
            };

            return returnResponse;
        }
        catch(Exception e)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
    
    
    // public Task<CourseWithEnrollmentsGetDto> GetCourseWithEnrollmentsDetailsByIdAsync(int id)
    // {
    //     throw new NotImplementedException();
    // }
}