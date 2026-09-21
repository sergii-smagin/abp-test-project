using ConferenceRoomApi.Domain;

namespace ConferenceRoomApi.Repositories;

public interface IBookingRepository
{
    Task AddAsync(
        Booking booking,
        List<BookingServiceLink> serviceLinks);

    Task<List<Booking>> GetAllAsync();

    Task<List<Booking>> GetOverlappingAsync(
        int roomId,
        DateTime startTime,
        DateTime endTime);
}