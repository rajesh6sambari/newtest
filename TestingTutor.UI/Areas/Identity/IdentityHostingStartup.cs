using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(TestingTutor.UI.Areas.Identity.IdentityHostingStartup))]
namespace TestingTutor.UI.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}