using System;
using System.Threading;
using System.Threading.Tasks;
using BlazorRpcSingalR.Client.Contract;
using BlazorRpcSingalR.Shared;
using BlazorRpcSingalR.Shared.Contract;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
 
namespace BlazorRpcSingalR.Client.HubConnections
{
    public abstract class RpcHubConnectionClient<RequestParam, ResponseParam> 
        : HubConnectionClient<RequestParam>, IRpcHubConnectionClient 
        where RequestParam : class, new() where ResponseParam : class
    {
        protected RpcHubConnectionClient(Func<HubConnection> hubConnectionFactory,ILogger logger)
            :base(hubConnectionFactory, logger)
        {
        }
        
        protected abstract Task<ResponseParam> ResponseHandler(RequestParam requestParam);
        /// <summary>
        /// Invoke hub server method 
        /// </summary>
        /// <param name="methodParams"></param>
        /// <returns></returns>
        protected override async Task MethodHandler(MethodParams<RequestParam> methodParams)
        {
            if (methodParams == null) throw new NullReferenceException(nameof(methodParams));

            var response = await ResponseHandler(methodParams.Parameter).ConfigureAwait(false);
            
            await InvokeAsync(nameof(IRpcResponseHandlers<ResponseParam>.MethodResponseHandler),
                new MethodResponse<ResponseParam>() { MethodCallId = methodParams.MethodCallId, Parameter = response }).ConfigureAwait(false);
        }
    }
}