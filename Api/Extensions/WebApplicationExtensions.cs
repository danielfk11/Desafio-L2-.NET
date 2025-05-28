using Microsoft.EntityFrameworkCore;

namespace api.Extensions
{
    public static class WebApplicationExtensions
    {
        public static void AplicarMigracoes(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<Data.AppDbContext>();
            db.Database.Migrate();
        }
    }
}
