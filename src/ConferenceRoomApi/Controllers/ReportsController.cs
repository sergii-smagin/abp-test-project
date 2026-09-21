using ConferenceRoomApi.DTOs;
using ConferenceRoomApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace ConferenceRoomApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly IReportsService _reportsService;

    public ReportsController(IReportsService reportsService)
    {
        _reportsService = reportsService;
    }

    [HttpGet("summary")]
    public async Task<ActionResult<ReportSummaryResponse>> GetSummary(
        [FromQuery] DateTime from,
        [FromQuery] DateTime to)
    {
        try
        {
            var report = await _reportsService.GetSummaryAsync(from, to);

            return Ok(report);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(exception.Message);
        }
    }
}