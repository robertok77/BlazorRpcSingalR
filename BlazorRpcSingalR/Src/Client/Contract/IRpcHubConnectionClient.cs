using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace BlazorRpcSingalR.Client.Contract
{
    public interface IRpcHubConnectionClient : IHostedService
    {
        /// <summary>
        /// Method to call hub server from client
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task RegisterUserIdAsync(string userId);
    }
}
