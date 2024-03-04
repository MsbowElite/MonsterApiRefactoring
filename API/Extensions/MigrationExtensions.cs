using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;

public static class MigrationExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        using BattleOfMonstersWriteDbContext dbContext =
            scope.ServiceProvider.GetRequiredService<BattleOfMonstersWriteDbContext>();

        dbContext.Database.Migrate();
    }
}
