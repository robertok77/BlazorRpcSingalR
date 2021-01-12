using System.Threading.Tasks;

namespace BlazorRpcSingalR.Shared.Contract
{
    public interface IRpcCalls<T> where T : class, new()
    {
        Task MethodCall(MethodParams<T> methodParams);
    }
}
