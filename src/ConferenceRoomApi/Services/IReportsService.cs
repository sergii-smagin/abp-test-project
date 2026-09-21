using ConferenceRoomApi.DTOs;

namespace ConferenceRoomApi.Services;

public interface IReportsService
{
    Task<ReportSummaryResponse> GetSummaryAsync(
        DateTime from,
        DateTime to);
}