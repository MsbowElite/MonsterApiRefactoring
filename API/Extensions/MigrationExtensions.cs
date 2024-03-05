using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;

public static class MigrationExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        using BattleOfMonstersContext dbContext =
            scope.ServiceProvider.GetRequiredService<BattleOfMonstersContext>();

        dbContext.Database.Migrate();
    }
}
