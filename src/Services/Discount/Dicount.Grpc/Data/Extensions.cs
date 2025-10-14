using Microsoft.EntityFrameworkCore;

namespace Dicount.Grpc.Data
{
    public static class Extensions
    {
        public static IApplicationBuilder  UseMigration(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<DiscountContext>();

            context.Database.Migrate();

            return app;
        }
    }
}
