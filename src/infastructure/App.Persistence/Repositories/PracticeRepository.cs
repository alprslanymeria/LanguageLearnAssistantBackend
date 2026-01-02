using App.Application.Contracts.Persistence.Repositories;
using App.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace App.Persistence.Repositories;

/// <summary>
/// REPOSITORY IMPLEMENTATION FOR PRACTICE ENTITY.
/// </summary>
public class PracticeRepository(AppDbContext context) : GenericRepository<Practice, int>(context), IPracticeRepository
{

    public async Task<List<Practice>> GetPracticesByLanguageAsync(string language)
    {
        return await Context.Practices
            .AsNoTracking()
            .Where(p => p.Language.Name == language)
            .ToListAsync();
    }


    // HELPER
    public async Task<Practice?> ExistsByLanguageIdAsync(int languageId)
    {
        return await Context.Practices
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.LanguageId == languageId);
    }

    public async Task<Practice?> ExistsByNameAndLanguageIdAsync(string name, int languageId)
    {
        return await Context.Practices
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Name == name && p.LanguageId == languageId);
    }
}
