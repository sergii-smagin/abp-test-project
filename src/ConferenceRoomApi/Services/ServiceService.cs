using ConferenceRoomApi.Domain;
using ConferenceRoomApi.DTOs;
using ConferenceRoomApi.Repositories;

namespace ConferenceRoomApi.Services;

public class ServiceService : IServiceService
{
    private readonly IServiceRepository _serviceRepository;

    public ServiceService(IServiceRepository serviceRepository)
    {
        _serviceRepository = serviceRepository;
    }

    public async Task<ServiceResponse?> GetByIdAsync(int id)
    {
        var service = await _serviceRepository.GetByIdAsync(id);

        if (service is null)
        {
            return null;
        }

        return new ServiceResponse(
            service.Id,
            service.Name,
            service.Price);
    }

    public async Task<List<ServiceResponse>> GetAllAsync()
    {
        var services = await _serviceRepository.GetAllAsync();

        return services
            .Select(service => new ServiceResponse(
                service.Id,
                service.Name,
                service.Price))
            .ToList();
    }

    public async Task<ServiceResponse> AddAsync(CreateServiceRequest request)
    {
        var service = new Service
        {
            Name = request.Name,
            Price = request.Price
        };

        await _serviceRepository.AddAsync(service);

        return new ServiceResponse(
            service.Id,
            service.Name,
            service.Price);
    }
}