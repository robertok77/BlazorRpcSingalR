using System;
using System.Threading;
using System.Threading.Tasks;
using BlazorRpcSingalR.Client.HubConnections;
using BlazorRpcSingalR.Server;
using BlazorRpcSingalR.Server.Domain;
using BlazorRpcSingalR.Server.Services;
using BlazorRpcSingalR.Shared.Contract;
using BlazorRpcSingalR.Shared.Infrastructure.EventAggregator;
using FluentAssertions;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;

namespace BlazorRpcSignalR.IntegrationTests
{
    public class RpcHubTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public RpcHubTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData(2, 10, new int[] { }, new int[] { 2, 3, 5, 7 })]
        public async Task ServerSendPrimaryRangetoFindNumbers_ExpectPrimaryArray(int beginNo, int count, int[] primes, int[] expected)
        {
            //arrange
            _factory.CreateClient().Should().NotBeNull();
            var server = _factory.Server;

            //arrange client
            Func<string, Uri> uriBuilder = endpoint => new Uri($"ws://localhost{endpoint}");
            Func<HubConnection> hubConnectionFact = () => new HubConnectionBuilder()
                .WithAutomaticReconnect()
                .WithUrl(uriBuilder(HubConnectionConst.PrimesNoEndpoint),
                    async options =>
                    {
                        options.HttpMessageHandlerFactory = _ => server.CreateHandler();
                    })
                .Build();
            var primeHubConnection = new PrimeHubConnectionClient(hubConnectionFact, new EventAggregator(), 
                new Logger<PrimeHubConnectionClient>(new LoggerFactory()));

            //arrange server
            Func<PrimeNumbersPersistance> primeNumberFactoryFunc = () => new PrimeNumbersPersistance(beginNo, count, primes);
            var primeNumberService = ActivatorUtilities.CreateInstance<PrimeNumberBackgroundService>(server.Services, primeNumberFactoryFunc);
            await primeHubConnection.StartAsync(CancellationToken.None);
             
            //act
            await primeHubConnection.RegisterUserIdAsync(Guid.NewGuid().ToString()).ConfigureAwait(false);
            await Task.Delay(100);

            //assert
            primeNumberService.PrimeNumbersPersist.PrimeNumbersArr.Should().BeEquivalentTo(expected);
        }
    }
}
