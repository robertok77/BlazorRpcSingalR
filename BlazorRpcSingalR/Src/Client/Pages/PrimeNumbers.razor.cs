using System;
using System.Threading;
using System.Threading.Tasks;
using BlazorRpcSingalR.Client.HubConnections;
using BlazorRpcSingalR.Shared.Domain;
using BlazorRpcSingalR.Shared.Infrastructure.EventAggregator;
using Microsoft.AspNetCore.Components;

namespace BlazorRpcSingalR.Client.Pages
{
    public partial class PrimeNumbers : IHandle<PrimeRet>, IDisposable
    {
        private int[] _primeNumbers = { };
        [Inject] public PrimeHubConnectionClient PrimeHubConnectionClient { get; set; }
        [Inject] public IEventAggregator EventAggregator { get; set; }
        
        protected override async Task OnInitializedAsync()
        {
            EventAggregator.SubscribeOnPublishedThread(this);
            await PrimeHubConnectionClient.RegisterUserIdAsync(Guid.NewGuid().ToString());
        }
        /// <summary>
        /// Handle event aggregator message and refresh ui
        /// </summary>
        /// <param name="message"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task HandleAsync(PrimeRet message, CancellationToken cancellationToken)
        {
            if (message == null) throw new NullReferenceException(nameof(message));

            _primeNumbers = message.Primes;
            await InvokeAsync(() => StateHasChanged());
        }

        public void Dispose()
        {
            EventAggregator.Unsubscribe(this);
        }
    }
}
