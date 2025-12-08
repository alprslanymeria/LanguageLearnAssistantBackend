using App.Domain.Entities;

namespace App.Application.Contracts.Persistence;

public interface ILanguageRepository : IGenericRepository<Language, int>
{
    // Add any Language-specific repository methods here if needed
}
