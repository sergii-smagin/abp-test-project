namespace ConferenceRoomApi.DTOs;

public record RoomReportResponse(
    int RoomId,
    string RoomName,
    int BookingCount,
    decimal BookedHours,
    decimal Revenue
);