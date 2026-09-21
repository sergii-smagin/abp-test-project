using ConferenceRoomApi.DTOs;
using ConferenceRoomApi.Results;

namespace ConferenceRoomApi.Services;

public interface IRoomService
{
    Task<RoomResponse?> GetByIdAsync(int id);

    Task<List<RoomResponse>> GetAllAsync();

    Task<RoomResponse> AddAsync(CreateRoomRequest request);

    Task<bool> UpdateAsync(int id, UpdateRoomRequest request);

    Task<RoomDeleteResult> DeleteAsync(int id);

    Task<List<RoomResponse>> GetAvailableAsync(AvailableRoomsRequest request);
}