namespace ConferenceRoomApi.Repositories;

public interface IReportsRepository
{
    Task<ReportsData> GetSummaryAsync(
        DateTime from,
        DateTime to);
}