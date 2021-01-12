using System;
using System.Linq;
using System.Threading.Tasks;
using BlazorRpcSingalR.Shared.Domain;
using BlazorRpcSingalR.Shared.Infrastructure.EventAggregator;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

namespace BlazorRpcSingalR.Client.HubConnections
{
    public class PrimeHubConnectionClient : RpcHubConnectionClient<PrimeParam, PrimeRet>
    {
        private readonly IEventAggregator _eventAggregator;

        public PrimeHubConnectionClient(Func<HubConnection> hubConnectionFactory, IEventAggregator eventAggregator, ILogger<PrimeHubConnectionClient> logger)
            : base(hubConnectionFactory, logger)
        {
            _eventAggregator = eventAggregator;
        }
        /// <summary>
        /// Calculate prime numbers. Send message with result to ui handlers. Return result to server.
        /// </summary>
        /// <param name="requestParam"></param>
        /// <returns></returns>
        protected override async Task<PrimeRet> ResponseHandler(PrimeParam requestParam)
        {
            if (requestParam == null) throw new NullReferenceException(nameof(requestParam));

            var result = new PrimeRet()
            {
                Primes = await Task.Run(() =>
                            PrimeCalculation.EratosthenesSieve(requestParam.BeginNumber, requestParam.Count, requestParam.Primes)
                            .ToArray()).ConfigureAwait(false)
            };
            await _eventAggregator.PublishOnCurrentThreadAsync(result);
            return result;
        }


    }
}