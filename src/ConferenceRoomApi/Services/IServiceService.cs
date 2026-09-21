using ConferenceRoomApi.DTOs;

namespace ConferenceRoomApi.Services;

public interface IServiceService
{
    Task<ServiceResponse?> GetByIdAsync(int id);
    Task<List<ServiceResponse>> GetAllAsync();
    Task<ServiceResponse> AddAsync(CreateServiceRequest request);
}