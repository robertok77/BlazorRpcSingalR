using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BlazorRpcSingalR.Client.HubConnections
{
    public abstract class HubConnectionBaseClient : IHostedService, IAsyncDisposable
    {
        protected readonly ILogger _logger;
        protected readonly HubConnection _hubConnection;

        protected HubConnectionBaseClient(Func<HubConnection> hubConnectionFactory, ILogger logger)
        {
            _logger = logger;
            _hubConnection = hubConnectionFactory();
        }

        public virtual async Task StartAsync(CancellationToken token)
        {
            await using var handler = token.Register(async () => await StopAsync(token));
            await ExecuteAsync(token);
        }

        public virtual Task StopAsync(CancellationToken token)
        {
            return _hubConnection.StopAsync(token);
        }

        protected async Task ExecuteAsync(CancellationToken token)
        {
            if (_hubConnection.State == HubConnectionState.Disconnected) await _hubConnection.StartAsync(token);
        }

        protected virtual Task InvokeAsync(string methodName, object arg1, CancellationToken token = default)
        {
            if (string.IsNullOrWhiteSpace(methodName)) throw new NullReferenceException(nameof(methodName));

            return _hubConnection.InvokeAsync(methodName, arg1, token);
        }

        #region IAsyncDisposable
        public virtual async ValueTask DisposeAsync()
        {
            await _hubConnection.DisposeAsync();
        }
        #endregion

    }
}
