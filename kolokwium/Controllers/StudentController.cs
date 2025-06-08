using kolokwium.DTOs;
using kolokwium.Exceptions;
using kolokwium.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace kolokwium.Controllers;

[ApiController]
[Route("[controller]")]
public class StudentController(IDbService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetStudentsDetails()
    {
        return Ok(await service.GetStudentsDetailsAsync());
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetStudentDetails([FromRoute] int id)
    {
        try
        {
            return Ok(await service.GetStudentDetailsByIdAsync(id));
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> AddStudent([FromBody] StudentCreateDto data)
    {
        try
        {
            var student = await service.CreateStudentAsync(data);
            // return CreatedAtAction(nameof(GetStudentDetails), new { id = student.Id }, student);
            return Created($"students/{student.Id}", student);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStudent([FromRoute] int id, [FromBody] StudentUpdateDto studentData)
    {
        try
        {
            await service.UpdateStudentAsync(id, studentData);
            return NoContent();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStudent([FromRoute] int id)
    {
        try
        {
            await service.RemoveStudentAsync(id);
            return NoContent();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
    
}