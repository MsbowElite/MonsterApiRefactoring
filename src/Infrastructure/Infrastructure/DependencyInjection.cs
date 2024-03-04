using Domain.Battles;
using Domain.Monsters;
using Infrastructure.Database;
using Infrastructure.Repositories;
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
            services.AddScoped<IMonsterRepository, MonsterRepository>();
            services.AddScoped<IBattleRepository, BattleRepository>();

            services.AddMediatR(config =>
                config.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
        }
    }
}
