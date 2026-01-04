using App.Application.Contracts.Persistence.Repositories;
using App.Domain.Entities.ReadingEntities;
using Microsoft.EntityFrameworkCore;

namespace App.Persistence.Repositories;

/// <summary>
/// REPOSITORY IMPLEMENTATION FOR READING ENTITY.
/// </summary>
public class ReadingRepository(AppDbContext context) : IReadingRepository
{
    public async Task CreateAsync(Reading entity) => await context.Readings.AddAsync(entity);

    public Task<Reading?> GetByIdAsync(int id) =>
        context.Readings
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == id);

    public Reading Update(Reading entity)
    {
        context.Readings.Update(entity);

        return entity;
    }

    public void Delete(Reading entity)
    {
        context.Readings
            .Remove(entity);
    }
}
