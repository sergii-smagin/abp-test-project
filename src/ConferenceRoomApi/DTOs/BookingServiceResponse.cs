namespace ConferenceRoomApi.DTOs;

public record BookingServiceResponse(
    int ServiceId,
    string Name,
    decimal Price
);