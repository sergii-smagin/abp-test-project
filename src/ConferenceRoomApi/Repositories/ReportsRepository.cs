using ConferenceRoomApi.Data;
using Microsoft.EntityFrameworkCore;

namespace ConferenceRoomApi.Repositories;

public class ReportsRepository : IReportsRepository
{
    private readonly AppDbContext _dbContext;

    public ReportsRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ReportsData> GetSummaryAsync(
        DateTime from,
        DateTime to)
    {
        var bookings = await _dbContext.Bookings
            .Where(booking =>
                booking.StartTime >= from &&
                booking.StartTime < to)
            .ToListAsync();

        var roomUtilization = await _dbContext.Rooms
            .Select(room => new RoomReportData
            {
                RoomId = room.Id,
                RoomName = room.Name,

                BookingCount = _dbContext.Bookings.Count(booking =>
                    booking.RoomId == room.Id &&
                    booking.StartTime >= from &&
                    booking.StartTime < to),

                BookedHours = _dbContext.Bookings
                    .Where(booking =>
                        booking.RoomId == room.Id &&
                        booking.StartTime >= from &&
                        booking.StartTime < to)
                    .Sum(booking =>
                        (decimal?)(booking.EndTime - booking.StartTime).TotalHours) ?? 0,

                Revenue = _dbContext.Bookings
                    .Where(booking =>
                        booking.RoomId == room.Id &&
                        booking.StartTime >= from &&
                        booking.StartTime < to)
                    .Sum(booking => (decimal?)booking.TotalPrice) ?? 0
            })
            .ToListAsync();

        var serviceUsage = await _dbContext.BookingServiceLinks
            .Where(link =>
                _dbContext.Bookings.Any(booking =>
                    booking.Id == link.BookingId &&
                    booking.StartTime >= from &&
                    booking.StartTime < to))
            .GroupBy(link => new
            {
                link.ServiceId,
                link.Service.Name
            })
            .Select(group => new ServiceReportData
            {
                ServiceId = group.Key.ServiceId,
                ServiceName = group.Key.Name,
                BookingCount = group.Count(),
                Revenue = group.Sum(link => link.Price)
            })
            .ToListAsync();
        
        return new ReportsData
        {
            TotalBookings = bookings.Count,
            TotalRevenue = bookings.Sum(booking => booking.TotalPrice),
            TotalBookedHours = bookings.Sum(booking =>
                (decimal)(booking.EndTime - booking.StartTime).TotalHours),
            RoomUtilization = roomUtilization,
            ServiceUsage = serviceUsage
        };
    }
}