using ConferenceRoomApi.Domain;
using ConferenceRoomApi.DTOs;
using ConferenceRoomApi.Services;
using ConferenceRoomApi.Results;
using Microsoft.AspNetCore.Mvc;

namespace ConferenceRoomApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomController : ControllerBase
{
    private readonly IRoomService _roomService;

    public RoomController(IRoomService roomService)
    {
        _roomService = roomService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RoomResponse>> GetById(int id)
    {
        var room = await _roomService.GetByIdAsync(id);

        if (room is null)
        {
            return NotFound();
        }

        return Ok(room);
    }

    [HttpGet]
    public async Task<ActionResult<List<RoomResponse>>> GetAll()
    {
        var rooms = await _roomService.GetAllAsync();
        return Ok(rooms);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateRoomRequest request)
    {
        var room = await _roomService.AddAsync(request);

        return CreatedAtAction(
            nameof(GetById),
            new { id = room.Id },
            room);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateRoomRequest request)
    {
        var updated = await _roomService.UpdateAsync(id, request);

        if (!updated)
        {
            return NotFound();
        }

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _roomService.DeleteAsync(id);

        return result switch
        {
            RoomDeleteResult.Deleted => NoContent(),
            RoomDeleteResult.RoomNotFound => NotFound(),
            RoomDeleteResult.HasBookings =>
                Conflict("The room cannot be deleted because it has existing bookings."),
            _ => StatusCode(500)
        };
    }

    [HttpGet("available")]
    public async Task<ActionResult<List<RoomResponse>>> GetAvailable(
        [FromQuery] AvailableRoomsRequest request)
    {
        try
        {
            var rooms = await _roomService.GetAvailableAsync(request);

            return Ok(rooms);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(exception.Message);
        }
    }
}