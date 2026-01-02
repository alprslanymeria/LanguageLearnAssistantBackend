using App.Application.Contracts.Persistence.Repositories;
using App.Domain.Entities.WritingEntities;
using Microsoft.EntityFrameworkCore;

namespace App.Persistence.Repositories;

/// <summary>
/// REPOSITORY IMPLEMENTATION FOR WRITING ENTITY.
/// </summary>
public class WritingRepository(AppDbContext context) : GenericRepository<Writing, int>(context), IWritingRepository
{
    public async Task<List<Writing>> GetByUserIdWithDetailsAsync(string userId)
    {
        return await Context.Writings
            .AsNoTracking()
            .Include(w => w.Language)
            .Include(w => w.Practice)
            .Where(w => w.UserId == userId)
            .ToListAsync();
    }
}
