using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TestingTutor.UI.Data;


namespace TestingTutor.Tests.Utilities
{
    public static class Utilities
    {
        public static DbContextOptions<ApplicationDbContext> TestDbContextOptions()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var builder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("InMemoryDb")
                .UseInternalServiceProvider(serviceProvider);

            return builder.Options;
        }
    }
}
