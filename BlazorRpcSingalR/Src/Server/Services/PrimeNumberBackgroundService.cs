using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BlazorRpcSingalR.Server.Contract;
using BlazorRpcSingalR.Server.Domain;
using BlazorRpcSingalR.Server.Hubs;
using BlazorRpcSingalR.Shared.Contract;
using BlazorRpcSingalR.Shared.Domain;
using BlazorRpcSingalR.Shared.Infrastructure.EventAggregator;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BlazorRpcSingalR.Server.Services
{
    public class PrimeNumberBackgroundService : BackgroundService, IHandle<HubRegister>
    {
        private readonly IRpcCaller<RpcHub<PrimeParam, PrimeRet>, PrimeParam, PrimeRet> _rpcCaller;
        private readonly IEventAggregator _eventAggregator;
        private readonly ILogger<PrimeNumberBackgroundService> _logger;
        public PrimeNumbersPersistance PrimeNumbersPersist { get; }
        
        public PrimeNumberBackgroundService(Func<PrimeNumbersPersistance> primeNumbersFactoryFunc,
            IRpcCaller<RpcHub<PrimeParam, PrimeRet>, PrimeParam, PrimeRet> rpcCaller, 
            IEventAggregator eventAggregator, 
            ILogger<PrimeNumberBackgroundService> logger)
        {
            PrimeNumbersPersist = primeNumbersFactoryFunc();
            _rpcCaller = rpcCaller;
            _eventAggregator = eventAggregator;
            _logger = logger;
            _eventAggregator.SubscribeOnPublishedThread(this);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.CompletedTask;
        }
         
        public override void Dispose()
        {
            _eventAggregator.Unsubscribe(this);
            base.Dispose();
        }

        public async Task HandleAsync(HubRegister message, CancellationToken cancellationToken)
        {
            if (message == null) throw new NullReferenceException(nameof(message));

            PrimeNumbersPersist.Get(out var end, out var count , out var primeNo);
            var primeParam = new PrimeParam();
            {
                primeParam.BeginNumber = end;
                primeParam.Count = count;
                primeParam.Primes = primeNo;
            }
            try
            {
                var result = await _rpcCaller.MethodCall(message.UserId, primeParam).ConfigureAwait(false);
                if (result?.Primes?.Length > 0 && ( primeNo.LastOrDefault() < result.Primes.First()))
                {
                    PrimeNumbersPersist.Increase(primeParam.Count, primeNo: result.Primes);
                }
            }
            catch (Exception e)
            {
                _logger.LogCritical(e,e.Message);
            }
        }
    }
}
