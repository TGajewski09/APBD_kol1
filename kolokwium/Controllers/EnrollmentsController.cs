using kolokwium.DTOs;
using kolokwium.Models;
using kolokwium.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace kolokwium.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EnrollmentsController(IDbService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetEnrollmentDetails()
    {
        return Ok(await service.GetEnrollmentsDetailsByAsync());
    }
}