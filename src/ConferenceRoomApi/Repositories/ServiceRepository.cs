using ConferenceRoomApi.Data;
using ConferenceRoomApi.Domain;
using Microsoft.EntityFrameworkCore;

namespace ConferenceRoomApi.Repositories;

public class ServiceRepository : IServiceRepository
{
    private readonly AppDbContext _dbContext;

    public ServiceRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Service?> GetByIdAsync(int id)
    {
        return await _dbContext.Services.FindAsync(id);
    }

    public async Task<List<Service>> GetAllAsync()
    {
        return await _dbContext.Services.ToListAsync();
    }

    public async Task AddAsync(Service service)
    {
        await _dbContext.Services.AddAsync(service);
        await _dbContext.SaveChangesAsync();
    }
}