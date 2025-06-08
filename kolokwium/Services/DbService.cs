using kolokwium.Data;
using kolokwium.DTOs;
using kolokwium.Exceptions;
using kolokwium.Models;
using Microsoft.EntityFrameworkCore;

namespace kolokwium.Services;

public interface IDbService
{
    public Task<ICollection<StudentGetDto>> GetStudentsDetailsAsync();
    public Task<StudentGetDto> CreateStudentAsync(StudentCreateDto dto);
    public Task<StudentGetDto> GetStudentDetailsByIdAsync(int studentId);
    public Task UpdateStudentAsync(int id, StudentUpdateDto studentData);
    public Task RemoveStudentAsync(int studentId);
}

public class DbService(AppDbContext data) : IDbService
{
    public async Task<ICollection<StudentGetDto>> GetStudentsDetailsAsync()
    {
        return await data.Students.Select(s => new StudentGetDto
        {
            Id = s.Id,
            FirstName = s.FirstName,
            LastName = s.LastName,
            Age = s.Age,
            EntranceExamScore = s.EntranceExamScore,
            Groups = s.GroupAssigments.Select(ga => new StudentGetDtoGroup()
            {
                Id = ga.GroupId,
                Name = ga.Group.Name,
            }).ToList()
        }).ToListAsync();
    }

    public async Task<StudentGetDto> CreateStudentAsync(StudentCreateDto studentCreateDto)
    {
        var groups = new List<Group>();
        
        if (studentCreateDto.GroupIds != null && studentCreateDto.GroupIds.Any())
        {
            foreach (var groupId in studentCreateDto.GroupIds)
            {
                var group = await data.Groups.FirstOrDefaultAsync(g => g.Id == groupId);
                if (group == null)
                {
                    throw new NotFoundException($"Group {groupId} not found");
                }
                groups.Add(group);
            }
        }

        var student = new Student
        {
            FirstName = studentCreateDto.FirstName,
            LastName = studentCreateDto.LastName,
            Age = studentCreateDto.Age,
            EntranceExamScore = studentCreateDto.EntranceExamScore,
            GroupAssigments = (studentCreateDto.GroupIds ?? []).Select(groupId => new GroupAssigment()
            {
                GroupId = groupId
            }).ToList()
        };
        
        await data.Students.AddAsync(student);
        await data.SaveChangesAsync();

        return new StudentGetDto()
        {
            Id = student.Id,
            FirstName = student.FirstName,
            LastName = student.LastName,
            Age = student.Age,
            EntranceExamScore = student.EntranceExamScore,
            Groups = groups.Select(g => new StudentGetDtoGroup()
            {
                Id = g.Id,
                Name = g.Name
            }).ToList()
        };
    }

    public async Task<StudentGetDto> GetStudentDetailsByIdAsync(int studentId)
    {
        var result = await data.Students.Select(s => new StudentGetDto()
        {
            Id = s.Id,
            FirstName = s.FirstName,
            LastName = s.LastName,
            Age = s.Age,
            EntranceExamScore = s.EntranceExamScore,
            Groups = s.GroupAssigments.Select(ga => new StudentGetDtoGroup()
            {
                Id = ga.GroupId,
                Name = ga.Group.Name,
            }).ToList()
        }).FirstOrDefaultAsync(s => s.Id == studentId);
        
        return result ?? throw new NotFoundException($"Student with id {studentId} not found");
    }

    public async Task UpdateStudentAsync(int studentId, StudentUpdateDto studentData)
    {
        var student = await data.Students.FirstOrDefaultAsync(s => s.Id == studentId);
        if (student is null)
        {
            throw new NotFoundException($"Student with id {studentId} not found");
        }
        
        student.FirstName = studentData.FirstName;
        student.LastName = studentData.LastName;
        student.Age = studentData.Age;
        student.EntranceExamScore = studentData.EntranceExamScore;
        
        await data.SaveChangesAsync();
    }

    public async Task RemoveStudentAsync(int studentId)
    {
        var student = await data.Students.FirstOrDefaultAsync(st => st.Id == studentId);
        if (student is null)
        {
            throw new NotFoundException($"Student with id {studentId} not found");
        }
        
        data.Students.Remove(student);
        await data.SaveChangesAsync();
    }
}