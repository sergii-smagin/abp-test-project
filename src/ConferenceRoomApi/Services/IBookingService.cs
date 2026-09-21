using ConferenceRoomApi.DTOs;
using ConferenceRoomApi.Results;

namespace ConferenceRoomApi.Services;

public interface IBookingService
{
    Task<BookingCreateResult> CreateAsync(CreateBookingRequest request);
    Task<List<BookingResponse>> GetAllAsync();
}