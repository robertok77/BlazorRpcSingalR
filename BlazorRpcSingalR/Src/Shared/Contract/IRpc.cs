using System.Threading.Tasks;

namespace BlazorRpcSingalR.Shared.Contract
{
    public interface IRpc<RequestParam, ResponseParam> where RequestParam : class, new()
    {
        Task<ResponseParam> MethodCall(string userId, RequestParam requestParam);
        Task<MethodResponse<ResponseParam>> MethodCall(string userId, MethodParams<RequestParam> methodParams);
    }
}
