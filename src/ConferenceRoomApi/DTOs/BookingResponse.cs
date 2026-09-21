namespace ConferenceRoomApi.DTOs;

public record BookingResponse(
    int Id,
    int RoomId,
    DateTime StartTime,
    DateTime EndTime,
    decimal TotalPrice,
    List<BookingServiceResponse> Services
);