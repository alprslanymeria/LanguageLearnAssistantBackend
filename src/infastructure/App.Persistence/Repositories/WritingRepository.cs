using App.Application.Contracts.Persistence.Repositories;
using App.Domain.Entities.WritingEntities;
using Microsoft.EntityFrameworkCore;

namespace App.Persistence.Repositories;

/// <summary>
/// REPOSITORY IMPLEMENTATION FOR WRITING ENTITY.
/// </summary>
public class WritingRepository(AppDbContext context) : IWritingRepository
{
    public async Task<List<Writing>> GetByUserIdWithDetailsAsync(string userId)
    {
        return await context.Writings
            .AsNoTracking()
            .Include(w => w.Language)
            .Include(w => w.Practice)
            .Where(w => w.UserId == userId)
            .ToListAsync();
    }

    public async Task CreateAsync(Writing entity) => await context.Writings.AddAsync(entity);

    public Task<Writing?> GetByIdAsync(int id) =>
        context.Writings
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.Id == id);

    public Writing Update(Writing entity)
    {
        context.Writings.Update(entity);

        return entity;
    }

    public void Delete(Writing entity)
    {
        context.Writings
            .Remove(entity);
    }
}
