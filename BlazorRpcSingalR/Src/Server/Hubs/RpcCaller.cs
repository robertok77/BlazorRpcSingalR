using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using BlazorRpcSingalR.Shared;
using BlazorRpcSingalR.Shared.Contract;
using Microsoft.AspNetCore.SignalR;

namespace BlazorRpcSingalR.Server.Hubs
{
    public class RpcCaller<THub, RequestParam, ResponseParam> : IRpcCaller<THub, RequestParam, ResponseParam>
        where THub : Hub<IRpcCalls<RequestParam>>, IRpcResponseHandlers<ResponseParam> where RequestParam : class, new()
    {
        private readonly IHubContext<THub, IRpcCalls<RequestParam>> _hubContext;
        private readonly ConcurrentDictionary<Guid, TaskCompletionSource<MethodResponse<ResponseParam>>> _pendingMethodCalls = new ConcurrentDictionary<Guid, TaskCompletionSource<MethodResponse<ResponseParam>>>();

        public RpcCaller(IHubContext<THub, IRpcCalls<RequestParam>> hubContext)
        {
            _hubContext = hubContext;
        }
        /// <summary>
        /// Rpc method to call client and receive result
        /// </summary>
        /// <param name="userId"> client id</param>
        /// <param name="requestParam"> object to send </param>
        /// <returns></returns>
        public async Task<ResponseParam> MethodCall(string userId, RequestParam requestParam)
        {
            if (userId ==null) throw new NullReferenceException(nameof(userId));

            var result = await MethodCall(userId, new MethodParams<RequestParam>() { MethodCallId = Guid.NewGuid(), Parameter = requestParam })
                .ConfigureAwait(false);
            return result != null ? result.Parameter : default;
        }
        public async Task<MethodResponse<ResponseParam>> MethodCall(string userId, MethodParams<RequestParam> methodParams)
        {
            if (userId == null) throw new NullReferenceException(nameof(userId));
            if (methodParams == null) throw new NullReferenceException(nameof(methodParams));
            try
            {
                var methodCallCompletionSource = new TaskCompletionSource<MethodResponse<ResponseParam>>();
                if (_pendingMethodCalls.TryAdd(methodParams.MethodCallId, methodCallCompletionSource))
                {
                    await _hubContext.Clients.Groups(userId).MethodCall(methodParams).ConfigureAwait(false);

                    return await methodCallCompletionSource.Task.ConfigureAwait(false);
                }
            }
            finally
            {
                _pendingMethodCalls.TryRemove(methodParams.MethodCallId, out _);
            }
            return default;
        }
        /// <summary>
        /// Handler to be called by Hub when receive response
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public Task MethodResponseHandler(MethodResponse<ResponseParam> response)
        {
            if (response == null) throw new NullReferenceException(nameof(response));

            if (_pendingMethodCalls.TryRemove(response.MethodCallId, out TaskCompletionSource<MethodResponse<ResponseParam>> methodCallCompletionSource))
            {
                methodCallCompletionSource.TrySetResult(response);
            }

            return Task.CompletedTask;
        }
    }
}
