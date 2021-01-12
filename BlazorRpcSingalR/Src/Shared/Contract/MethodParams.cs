using System;

namespace BlazorRpcSingalR.Shared.Contract
{
    public class MethodParams<T> where T: class, new()
    {
        public Guid MethodCallId { get; set; }

        public T Parameter { get; set; }
        
    }

    
}
