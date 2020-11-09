using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(RecipeBook.Areas.Identity.IdentityHostingStartup))]
namespace RecipeBook.Areas.Identity
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