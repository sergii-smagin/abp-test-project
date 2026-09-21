namespace ConferenceRoomApi.DTOs;

public record RoomResponse(
    int Id,
    string Name,
    int Capacity,
    decimal BaseHourlyRate
);