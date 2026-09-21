using ConferenceRoomApi.DTOs;
using ConferenceRoomApi.Services;
using ConferenceRoomApi.Results;
using Microsoft.AspNetCore.Mvc;

namespace ConferenceRoomApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpGet]
    public async Task<ActionResult<List<BookingResponse>>> GetAll()
    {
        var bookings = await _bookingService.GetAllAsync();

        return Ok(bookings);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateBookingRequest request)
    {
        var result = await _bookingService.CreateAsync(request);

        return result switch
        {
            BookingCreateResult.Created => StatusCode(StatusCodes.Status201Created),
            BookingCreateResult.RoomNotFound => NotFound(),
            BookingCreateResult.ServiceNotFound => NotFound(),
            BookingCreateResult.DuplicateService => BadRequest("The same service cannot be added more than once."),
            BookingCreateResult.InvalidTime => BadRequest("EndTime must be later than StartTime."),
            BookingCreateResult.OutsideWorkingHours => BadRequest("Booking time must be between 06:00 and 23:00."),
            BookingCreateResult.Conflict => Conflict("The room is already booked for the requested time."),
            _ => StatusCode(500)
        };
    }
}