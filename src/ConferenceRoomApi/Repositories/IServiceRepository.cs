using ConferenceRoomApi.Domain;

namespace ConferenceRoomApi.Repositories;

public interface IServiceRepository
{
    Task<Service?> GetByIdAsync(int id);
    Task<List<Service>> GetAllAsync();
    Task AddAsync(Service service);
}