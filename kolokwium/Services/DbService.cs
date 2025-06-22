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

    public async Task<CourseCreateResponseDto> CreateCourseWithEnrollmentsAsync(CourseCreateDto createData)
    {
        var newCourse = new Course
        {
            Title = createData.Title,
            Credits = createData.Credits,
            Teacher = createData.Teacher,
        };
        data.Courses.Add(newCourse);
        await data.SaveChangesAsync();
        
        ICollection<CourseCreateResponseEnrollmentDto> enrollments = new List<CourseCreateResponseEnrollmentDto>();

        if (createData.Students != null && createData.Students.Any())
        {
            foreach (var createStudentDto in createData.Students)
            {
                var student= data.Students.FirstOrDefault(student => 
                    student.FirstName == createStudentDto.FirstName 
                    && student.LastName == createStudentDto.LastName
                    && student.Email == createStudentDto.Email
                    );

                if (student == null)
                {
                    student = new Student
                    {
                        FirstName = createStudentDto.FirstName,
                        LastName = createStudentDto.LastName,
                        Email = createStudentDto.Email,
                    };
                    data.Students.Add(student);
                    await data.SaveChangesAsync();
                }

                var newEnrollment = new Enrollment
                {
                    CourseId = newCourse.Id,
                    StudentId = student.Id,
                    EnrollmentDate = DateTime.Now
                };
                data.Enrollments.Add(newEnrollment);
                await data.SaveChangesAsync();

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

    // public Task<CourseWithEnrollmentsGetDto> GetCourseWithEnrollmentsDetailsByIdAsync(int id)
    // {
    //     throw new NotImplementedException();
    // }
}