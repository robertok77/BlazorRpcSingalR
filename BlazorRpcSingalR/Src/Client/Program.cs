using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using BlazorRpcSingalR.Client.HubConnections;
using BlazorRpcSingalR.Shared.Contract;
using BlazorRpcSingalR.Shared.Infrastructure.EventAggregator;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http.Connections.Client;
using Microsoft.AspNetCore.SignalR.Client;

namespace BlazorRpcSingalR.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddSingleton<IEventAggregator, EventAggregator>();
            builder.Services.AddSingleton<PrimeHubConnectionClient>();
            builder.Services.AddTransient<IHubConnectionBuilder, HubConnectionBuilder>();
            builder.Services.AddTransient<Func<HubConnection>>(provider =>
                () => provider.GetRequiredService<IHubConnectionBuilder>()
                    .WithAutomaticReconnect()
                    .WithUrl(provider.GetRequiredService<NavigationManager>().ToAbsoluteUri(HubConnectionConst.PrimesNoEndpoint))
                    .Build());
            builder.Services.AddSingleton<Func<string, Uri>>(provider => endpoint => provider.GetRequiredService<NavigationManager>().ToAbsoluteUri(endpoint));
            var webAssemblyHost = builder.Build();
            webAssemblyHost.Services.GetRequiredService<PrimeHubConnectionClient>().ExecuteAsync(CancellationToken.None);
            await webAssemblyHost.RunAsync();
        }
    }
}
