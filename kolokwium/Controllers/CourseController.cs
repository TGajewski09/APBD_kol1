using kolokwium.DTOs;
using kolokwium.Exceptions;
using kolokwium.Services;
using Microsoft.AspNetCore.Mvc;

namespace kolokwium.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CourseController(IDbService dbService): ControllerBase
{
    // [HttpGet("{id}")]
    // public async Task<IActionResult> GetCourseWithEnrollmentsDetails([FromRoute] int id)
    // {
    //     try
    //     {
    //         return Ok(await dbService.GetCourseWithEnrollmentsDetailsByIdAsync(id));
    //     }
    //     catch (NotFoundException e)
    //     {
    //         return NotFound(e.Message);
    //     }
    // }
    
    [HttpPost]
    public async Task<IActionResult> CreateCourseWithEnrollments([FromBody] CourseCreateDto data)
    {
        try
        {
            var result = await dbService.CreateCourseWithEnrollmentsAsync(data);
            return StatusCode(201, result);
            // return CreatedAtAction(nameof(GetCourseWithEnrollmentsDetails), new { id = result.Course.Id }, result);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
}