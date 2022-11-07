using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BlazorRpcSingalR.Client.Extensions
{
    public static class HostExtensions
    {
        public static WebAssemblyHost RunHostedServices(this WebAssemblyHost host, CancellationToken token)
        {
            foreach (var hostedService in host.Services.GetServices<IHostedService>())
                hostedService.StartAsync(token);
            return host;
        }
    }
}
