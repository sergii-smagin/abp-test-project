using ConferenceRoomApi.DTOs;
using ConferenceRoomApi.Repositories;

namespace ConferenceRoomApi.Services;

public class ReportsService : IReportsService
{
    private readonly IReportsRepository _reportsRepository;

    public ReportsService(IReportsRepository reportsRepository)
    {
        _reportsRepository = reportsRepository;
    }

    public async Task<ReportSummaryResponse> GetSummaryAsync(
        DateTime from,
        DateTime to)
    {
        if (to <= from)
        {
            throw new ArgumentException(
                "The report end time must be later than the start time.");
        }
        
        var data = await _reportsRepository.GetSummaryAsync(from, to);

        return new ReportSummaryResponse(
            data.TotalBookings,
            data.TotalRevenue,
            data.TotalBookedHours,
            data.RoomUtilization
                .Select(room => new RoomReportResponse(
                    room.RoomId,
                    room.RoomName,
                    room.BookingCount,
                    room.BookedHours,
                    room.Revenue))
                .ToList(),
            data.ServiceUsage
                .Select(service => new ServiceReportResponse(
                    service.ServiceId,
                    service.ServiceName,
                    service.BookingCount,
                    service.Revenue))
                .ToList());
    }
}