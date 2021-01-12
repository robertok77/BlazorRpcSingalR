using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorRpcSingalR.Shared;
using BlazorRpcSingalR.Shared.Contract;
using BlazorRpcSingalR.Shared.Infrastructure.EventAggregator;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace BlazorRpcSingalR.Server.Hubs
{
    /// <summary>
    /// Rcp hub to handle signalR messages
    /// </summary>
    /// <typeparam name="RequestParam"></typeparam>
    /// <typeparam name="ResponseParam"></typeparam>
    public class RpcHub<RequestParam, ResponseParam> : Hub<IRpcCalls<RequestParam>>, IRpcResponseHandlers<ResponseParam>, IHubInitialization where RequestParam : class, new()
    {
        private readonly IRpcCaller<RpcHub<RequestParam, ResponseParam>, RequestParam, ResponseParam> _rpcCaller;
        private readonly IEventAggregator _eventAggregator;
        private readonly ILogger<RpcHub<RequestParam, ResponseParam>> _logger;

        public RpcHub(IRpcCaller<RpcHub<RequestParam, ResponseParam>, RequestParam, ResponseParam> rpcCaller, IEventAggregator eventAggregator, ILogger<RpcHub<RequestParam, ResponseParam>> logger)
        {
            _rpcCaller = rpcCaller;
            _eventAggregator = eventAggregator;
            _logger = logger;
        }
        /// <summary>
        /// Client id and connection registration
        /// event aggregator is called asynchronous 
        /// </summary>
        /// <param name="hubRegister"></param>
        /// <returns></returns>
        public async Task RegisterUserIdAsync(HubRegister hubRegister)
        {
            if (hubRegister == null) throw new NullReferenceException(nameof(hubRegister));
            if (hubRegister.UserId == null)  throw new NullReferenceException(nameof(hubRegister.UserId));

            await Groups.AddToGroupAsync(Context.ConnectionId, hubRegister.UserId).ConfigureAwait(false);
            Context.Items.TryAdd(hubRegister.UserId, Context.ConnectionId);
            _eventAggregator.PublishOnCurrentThreadAsync(hubRegister).ConfigureAwait(false);
        }

        public Task MethodResponseHandler(MethodResponse<ResponseParam> response)
        {
            return _rpcCaller.MethodResponseHandler(response);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            foreach (var item in Context.Items.Select(x => x.Key?.ToString()).Where(x => x != null))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, item);
                Context.Items.Remove(item);
            }
            await base.OnDisconnectedAsync(exception);
        }
    }
}
