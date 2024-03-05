using Application.Abstractions.Caching;
using Application.Abstractions.Data;
using Domain.Battles;
using Domain.Monsters;
using Infrastructure.Caching;
using Infrastructure.Database;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration
            )
        {
            services.AddMediatR(config =>
                config.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

            string? connectionString = configuration.GetConnectionString("Database");

            services.AddDbContext<BattleOfMonstersContext>(
                options => options
                    .UseNpgsql(connectionString)
                    .UseSnakeCaseNamingConvention()
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

            services.AddDbContext<BattleOfMonstersContext>(
                (sp, options) => options
                    .UseNpgsql(connectionString)
                    .UseSnakeCaseNamingConvention());

            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<BattleOfMonstersContext>());

            services.AddScoped<IMonsterRepository, MonsterRepository>();
            services.AddScoped<IBattleRepository, BattleRepository>();

            services.AddDistributedMemoryCache();
            services.AddSingleton<ICacheService, CacheService>();
        }
    }
}
