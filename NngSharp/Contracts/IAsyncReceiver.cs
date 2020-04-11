using System;
using System.Threading;
using System.Threading.Tasks;
using NngSharp.Async;
using NngSharp.Data;

namespace NngSharp.Contracts
{
    public interface IAsyncReceiver : IDisposable
    {
        public AsyncOptions Options { get; }
        Task<Message> ReceiveMessageAsync();
        Task<Message> ReceiveMessageAsync(CancellationToken cancellationToken);
    }
}