using System;

namespace BlazorRpcSingalR.Shared.Contract
{
    public class MethodResponse<T>
    {
        public Guid MethodCallId { get; set; }

        public T Parameter { get; set; }
    }
}
