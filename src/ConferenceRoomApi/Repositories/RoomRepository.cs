using ConferenceRoomApi.Data;
using ConferenceRoomApi.Domain;
using ConferenceRoomApi.Results;
using Microsoft.EntityFrameworkCore;

namespace ConferenceRoomApi.Repositories;

public class RoomRepository : IRoomRepository
{
    private readonly AppDbContext _dbContext;

    public RoomRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Room?> GetByIdAsync(int id)
    {
        return await _dbContext.Rooms.FindAsync(id);
    }

    public async Task<List<Room>> GetAllAsync()
    {
        return await _dbContext.Rooms.ToListAsync();
    }

    public async Task AddAsync(Room room)
    {
        await _dbContext.Rooms.AddAsync(room);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Room room)
    {
        _dbContext.Rooms.Update(room);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<RoomDeleteResult> DeleteAsync(Room room)
    {
        var hasBookings = await _dbContext.Bookings
            .AnyAsync(booking => booking.RoomId == room.Id);

        if (hasBookings)
        {
            return RoomDeleteResult.HasBookings;
        }

        _dbContext.Rooms.Remove(room);
        await _dbContext.SaveChangesAsync();

        return RoomDeleteResult.Deleted;
    }

    public async Task<List<Room>> GetAvailableAsync(
        int capacity,
        DateTime startTime,
        DateTime endTime)
    {
        return await _dbContext.Rooms
            .Where(room =>
                room.Capacity >= capacity &&
                !_dbContext.Bookings.Any(booking =>
                    booking.RoomId == room.Id &&
                    booking.StartTime < endTime &&
                    booking.EndTime > startTime))
            .ToListAsync();
    }
}