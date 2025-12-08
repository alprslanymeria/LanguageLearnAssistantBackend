using App.Application.Contracts.Persistence;
using App.Domain.Entities;

namespace App.Persistence.Repositories;

public class LanguageRepository : GenericRepository<Language, int>, ILanguageRepository
{
    public LanguageRepository(AppDbContext context) : base(context)
    {
    }

    // Add any Language-specific repository methods implementation here if needed
}
