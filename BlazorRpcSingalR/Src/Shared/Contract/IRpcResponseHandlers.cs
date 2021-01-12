using System.Threading.Tasks;

namespace BlazorRpcSingalR.Shared.Contract
{
    public interface IRpcResponseHandlers<ResponseParam>
    {
        Task MethodResponseHandler(MethodResponse<ResponseParam> response);
    }
}
