using System;
using BlazorRpcSingalR.Server.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BlazorRpcSingalR.Server.Hubs;
using BlazorRpcSingalR.Shared.Contract;
using BlazorRpcSingalR.Shared.Domain;
using BlazorRpcSingalR.Shared.Infrastructure.EventAggregator;

namespace BlazorRpcSingalR.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddSingleton<IRpcCaller<RpcHub<PrimeParam, PrimeRet>, PrimeParam, PrimeRet>, RpcCaller<RpcHub<PrimeParam, PrimeRet>, PrimeParam, PrimeRet>>();
            services.AddSingleton<IEventAggregator, EventAggregator>();
            services.AddTransient<Func<PrimeNumbersPersistance>>(provider => () => new PrimeNumbersPersistance(2, 10_000, new int[] { }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapHub<RpcHub<PrimeParam, PrimeRet>>(HubConnectionConst.PrimesNoEndpoint);
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
