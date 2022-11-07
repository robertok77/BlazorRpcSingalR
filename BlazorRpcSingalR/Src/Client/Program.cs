using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using BlazorRpcSingalR.Client.Contract;
using BlazorRpcSingalR.Client.Extensions;
using BlazorRpcSingalR.Client.HubConnections;
using BlazorRpcSingalR.Shared.Contract;
using BlazorRpcSingalR.Shared.Infrastructure.EventAggregator;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http.Connections.Client;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;

namespace BlazorRpcSingalR.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.Services.AddSingleton<IEventAggregator, EventAggregator>();
            builder.Services.AddSingleton<IRpcHubConnectionClient, PrimeHubConnectionClient>();
            builder.Services.AddHostedService(x => x.GetRequiredService<IRpcHubConnectionClient>());
            builder.Services.AddTransient<IHubConnectionBuilder, HubConnectionBuilder>();
            builder.Services.AddTransient<Func<HubConnection>>(provider =>
                () => provider.GetRequiredService<IHubConnectionBuilder>()
                    .WithAutomaticReconnect()
                    .WithUrl(provider.GetRequiredService<NavigationManager>().ToAbsoluteUri(HubConnectionConst.PrimesNoEndpoint))
                    .Build());
            var webAssemblyHost = builder.Build().RunHostedServices(CancellationToken.None);
            await webAssemblyHost.RunAsync();
        }
    }
}
