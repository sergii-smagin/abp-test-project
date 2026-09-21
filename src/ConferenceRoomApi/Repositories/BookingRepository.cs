using ConferenceRoomApi.Data;
using ConferenceRoomApi.Domain;
using Microsoft.EntityFrameworkCore;

namespace ConferenceRoomApi.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly AppDbContext _dbContext;

    public BookingRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(
        Booking booking,
        List<BookingServiceLink> serviceLinks)
    {
        await using var transaction =
            await _dbContext.Database.BeginTransactionAsync();

        try
        {
            await _dbContext.Bookings.AddAsync(booking);
            await _dbContext.SaveChangesAsync();

            foreach (var serviceLink in serviceLinks)
            {
                serviceLink.BookingId = booking.Id;
            }

            await _dbContext.BookingServiceLinks.AddRangeAsync(serviceLinks);
            await _dbContext.SaveChangesAsync();

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<List<Booking>> GetAllAsync()
    {
        return await _dbContext.Bookings
            .Include(booking => booking.ServiceLinks)
            .ThenInclude(link => link.Service)
            .ToListAsync();
    }

    public async Task<List<Booking>> GetOverlappingAsync(
        int roomId,
        DateTime startTime,
        DateTime endTime)
    {
        return await _dbContext.Bookings
            .Where(booking =>
                booking.RoomId == roomId &&
                booking.StartTime < endTime &&
                booking.EndTime > startTime)
            .ToListAsync();
    }
}