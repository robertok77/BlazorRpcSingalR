using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

namespace BlazorRpcSingalR.Client.HubConnections
{
    public abstract class HubConnectionBaseClient : IAsyncDisposable
    {
        protected readonly ILogger _logger;
        protected readonly HubConnection _hubConnection;

        protected HubConnectionBaseClient(Func<HubConnection> hubConnectionFactory, ILogger logger)
        {
            _logger = logger;
            _hubConnection = hubConnectionFactory();
        }

        public virtual async Task StartAsync()
        {
            if (_hubConnection.State == HubConnectionState.Disconnected) await _hubConnection.StartAsync();
        }

        public virtual Task StopAsync(CancellationToken cancellationToken)
        {
            _hubConnection.StopAsync(cancellationToken);
            return Task.CompletedTask;
        }

        public async Task ExecuteAsync(CancellationToken token = default)
        {
            await StartAsync();
            token.Register(async () => await StopAsync(token));
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
