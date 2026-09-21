namespace ConferenceRoomApi.Repositories;

public class ReportsData
{
    public int TotalBookings { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal TotalBookedHours { get; set; }

    public List<RoomReportData> RoomUtilization { get; set; } = [];
    public List<ServiceReportData> ServiceUsage { get; set; } = [];
}

public class RoomReportData
{
    public int RoomId { get; set; }
    public string RoomName { get; set; } = string.Empty;
    public int BookingCount { get; set; }
    public decimal BookedHours { get; set; }
    public decimal Revenue { get; set; }
}

public class ServiceReportData
{
    public int ServiceId { get; set; }
    public string ServiceName { get; set; } = string.Empty;
    public int BookingCount { get; set; }
    public decimal Revenue { get; set; }
}