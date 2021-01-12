using System.Threading.Tasks;

namespace BlazorRpcSingalR.Shared.Contract
{
    public interface IHubInitialization
    {
        Task RegisterUserIdAsync(HubRegister userId);
    }
}