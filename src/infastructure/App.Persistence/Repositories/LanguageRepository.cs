using App.Application.Contracts.Persistence.Repositories;
using App.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace App.Persistence.Repositories;

/// <summary>
/// REPOSITORY IMPLEMENTATION FOR LANGUAGE ENTITY.
/// </summary>
public class LanguageRepository(AppDbContext context) : GenericRepository<Language, int>(context), ILanguageRepository
{

    public async Task<List<Language>> GetLanguagesAsync()
    {
        return await Context.Languages
            .AsNoTracking()
            .ToListAsync();
    }

    // HELPER
    public async Task<Language?> ExistsByNameAsync(string name)
    {
        return await Context.Languages
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.Name == name);
    }
}
