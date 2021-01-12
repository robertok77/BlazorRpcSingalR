using System;
using System.Threading.Tasks;
using BlazorRpcSingalR.Shared;
using BlazorRpcSingalR.Shared.Contract;
using Microsoft.AspNetCore.Http.Connections.Client;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

namespace BlazorRpcSingalR.Client.HubConnections
{
    public abstract class HubConnectionClient<RequestParam> : HubConnectionBaseClient where RequestParam : class, new()
    {
        private readonly IDisposable _methodCallHandler;

        protected HubConnectionClient(Func<HubConnection> hubConnectionFactory, ILogger logger)
            : base(hubConnectionFactory, logger)
        {
            _methodCallHandler = _hubConnection.On<MethodParams<RequestParam>>(nameof(IRpcCalls<RequestParam>.MethodCall), MethodHandler);
        }
        /// <summary>
        /// Method to call hub server from client
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task RegisterUserIdAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId)) throw new NullReferenceException(nameof(userId));

            return InvokeAsync(nameof(IHubInitialization.RegisterUserIdAsync), new HubRegister() { UserId = userId });
        }
        /// <summary>
        /// Method handler on which client listens from server
        /// </summary>
        /// <param name="methodParams"></param>
        /// <returns></returns>
        protected abstract Task MethodHandler(MethodParams<RequestParam> methodParams);

        public override ValueTask DisposeAsync()
        {
            _hubConnection.Remove(nameof(IRpcCalls<RequestParam>.MethodCall));
            _methodCallHandler.Dispose();
            return base.DisposeAsync();
        }
    }
}