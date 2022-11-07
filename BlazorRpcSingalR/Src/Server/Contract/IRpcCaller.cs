using BlazorRpcSingalR.Shared.Contract;
using Microsoft.AspNetCore.SignalR;

namespace BlazorRpcSingalR.Server.Contract
{
    /// <summary>
    /// Generic rpc caller interface 
    /// </summary>
    /// <typeparam name="THub"></typeparam>
    /// <typeparam name="RequestParam"></typeparam>
    /// <typeparam name="ResponseParam"></typeparam>
    public interface IRpcCaller<THub, RequestParam, ResponseParam> 
        : IRpc<RequestParam, ResponseParam>, IRpcResponseHandlers<ResponseParam> 
            where THub : Hub<IRpcCalls<RequestParam>>, IRpcResponseHandlers<ResponseParam> where RequestParam : class, new()
    { }
}
