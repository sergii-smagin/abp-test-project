namespace ConferenceRoomApi.DTOs;

public record ReportSummaryResponse(
    int TotalBookings,
    decimal TotalRevenue,
    decimal TotalBookedHours,
    List<RoomReportResponse> RoomUtilization,
    List<ServiceReportResponse> ServiceUsage
);