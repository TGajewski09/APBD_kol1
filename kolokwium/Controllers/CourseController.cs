using kolokwium.DTOs;
using kolokwium.Exceptions;
using kolokwium.Services;
using Microsoft.AspNetCore.Mvc;

namespace kolokwium.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CourseController(IDbService dbService): ControllerBase
{
    
    [HttpPost]
    public async Task<IActionResult> CreateCourseWithEnrollments([FromBody] CourseCreateDto data)
    {
        try
        {
            var result = await dbService.CreateCourseWithEnrollmentsAsync(data);
            return StatusCode(201, result);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
}