using App.Application.Contracts.Persistence;
using App.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace App.Persistence.Extensions;

public static class RepositoryExtension
{
    public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        // DB CONTEXT
        services.AddDbContext<AppDbContext>(options =>
        {

            var connString = configuration.GetConnectionString("SqlServer");

            options.UseSqlServer(connString, sqlOptions =>
            {

                sqlOptions.MigrationsAssembly(typeof(PersistenceAssembly).Assembly.FullName);
            });
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));

        // SPECIFIC REPOSITORIES
        services.AddScoped<ILanguageRepository, LanguageRepository>();

        return services;
    }
}
