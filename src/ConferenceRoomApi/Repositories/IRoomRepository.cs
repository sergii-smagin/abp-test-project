using ConferenceRoomApi.Domain;
using ConferenceRoomApi.Results;

namespace ConferenceRoomApi.Repositories;

public interface IRoomRepository
{
    Task<Room?> GetByIdAsync(int id);

    Task<List<Room>> GetAllAsync();

    Task AddAsync(Room room);

    Task UpdateAsync(Room room);

    Task<RoomDeleteResult> DeleteAsync(Room room);

    Task<List<Room>> GetAvailableAsync(
        int capacity,
        DateTime startTime,
        DateTime endTime);
}