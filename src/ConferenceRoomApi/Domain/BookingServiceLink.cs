namespace ConferenceRoomApi.Domain;

public class BookingServiceLink
{
    public int BookingId { get; set; }
    public int ServiceId { get; set; }
    public decimal Price { get; set; }

    public Service Service { get; set; } = null!;
}