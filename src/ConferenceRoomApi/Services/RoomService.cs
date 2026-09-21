using ConferenceRoomApi.Domain;
using ConferenceRoomApi.DTOs;
using ConferenceRoomApi.Repositories;
using ConferenceRoomApi.Results;

namespace ConferenceRoomApi.Services;

public class RoomService : IRoomService
{
    private readonly IRoomRepository _roomRepository;

    public RoomService(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    public async Task<RoomResponse?> GetByIdAsync(int id)
    {
        var room = await _roomRepository.GetByIdAsync(id);

        if (room is null)
        {
            return null;
        }

        return new RoomResponse(
            room.Id,
            room.Name,
            room.Capacity,
            room.BaseHourlyRate);
    }

    public async Task<List<RoomResponse>> GetAllAsync()
    {
        var rooms = await _roomRepository.GetAllAsync();

        return rooms
            .Select(room => new RoomResponse(
                room.Id,
                room.Name,
                room.Capacity,
                room.BaseHourlyRate))
            .ToList();
    }

    public async Task<RoomResponse> AddAsync(CreateRoomRequest request)
    {
        var room = new Room
        {
            Name = request.Name,
            Capacity = request.Capacity,
            BaseHourlyRate = request.BaseHourlyRate
        };

        await _roomRepository.AddAsync(room);

        return new RoomResponse(
            room.Id,
            room.Name,
            room.Capacity,
            room.BaseHourlyRate);
    }

    public async Task<bool> UpdateAsync(int id, UpdateRoomRequest request)
    {
        var room = await _roomRepository.GetByIdAsync(id);

        if (room is null)
        {
            return false;
        }

        room.Name = request.Name;
        room.Capacity = request.Capacity;
        room.BaseHourlyRate = request.BaseHourlyRate;

        await _roomRepository.UpdateAsync(room);

        return true;
    }

    public async Task<RoomDeleteResult> DeleteAsync(int id)
    {
        var room = await _roomRepository.GetByIdAsync(id);

        if (room is null)
        {
            return RoomDeleteResult.RoomNotFound;
        }

        return await _roomRepository.DeleteAsync(room);
    }

    public async Task<List<RoomResponse>> GetAvailableAsync(
    AvailableRoomsRequest request)
    {
        if (request.EndTime <= request.StartTime)
        {
            throw new ArgumentException(
                "EndTime must be later than StartTime.");
        }

        var openingTime = TimeSpan.FromHours(6);
        var closingTime = TimeSpan.FromHours(23);

        if (request.StartTime.TimeOfDay < openingTime ||
            request.EndTime.TimeOfDay > closingTime)
        {
            throw new ArgumentException(
                "Search time must be between 06:00 and 23:00.");
        }

        var rooms = await _roomRepository.GetAvailableAsync(
            request.Capacity,
            request.StartTime,
            request.EndTime);

        return rooms
            .Select(room => new RoomResponse(
                room.Id,
                room.Name,
                room.Capacity,
                room.BaseHourlyRate))
            .ToList();
    }
}