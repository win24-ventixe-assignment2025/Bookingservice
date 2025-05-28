using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;
using Presentation.Services;

namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookingsController(IBookingService bookingsService) : ControllerBase
{
    private readonly IBookingService _bookingsService = bookingsService;

    [HttpPost]
    public async Task<IActionResult> Create (CreateBookingRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var result = await _bookingsService.CreateBookingAsync(request);

        return result.Success ? Ok()
            : StatusCode(StatusCodes.Status500InternalServerError, "Unable to create booking.");
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _bookingsService.GetAllBookingsAsync();
        if (!result.Success)
            return BadRequest(result.Error);

        return Ok(result.Result);
    }

}
