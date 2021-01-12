using BlazorRpcSingalR.Server;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace BlazorRpcSignalR.IntegrationTests
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            var builder= base.CreateWebHostBuilder();
            return builder;
        }
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseStartup<Startup>();

        }
    }
}
