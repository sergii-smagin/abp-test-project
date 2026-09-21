using ConferenceRoomApi.Domain;
using ConferenceRoomApi.DTOs;
using ConferenceRoomApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace ConferenceRoomApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServiceController : ControllerBase
{
    private readonly IServiceService _serviceService;

    public ServiceController(IServiceService serviceService)
    {
        _serviceService = serviceService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ServiceResponse>>> GetAll()
    {
        var services = await _serviceService.GetAllAsync();
        return Ok(services);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ServiceResponse>> GetById(int id)
    {
        var service = await _serviceService.GetByIdAsync(id);

        if (service is null)
        {
            return NotFound();
        }

        return Ok(service);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateServiceRequest request)
    {
        var service = await _serviceService.AddAsync(request);

        return CreatedAtAction(
            nameof(GetById),
            new { id = service.Id },
            service);
    }
}