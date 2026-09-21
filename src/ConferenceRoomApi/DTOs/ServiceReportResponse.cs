namespace ConferenceRoomApi.DTOs;

public record ServiceReportResponse(
    int ServiceId,
    string ServiceName,
    int BookingCount,
    decimal Revenue
);