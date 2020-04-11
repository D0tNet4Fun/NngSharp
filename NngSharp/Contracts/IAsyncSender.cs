using System;
using System.Threading;
using System.Threading.Tasks;
using NngSharp.Async;
using NngSharp.Data;

namespace NngSharp.Contracts
{
    public interface IAsyncSender : IDisposable
    {
        AsyncOptions Options { get; }
        Task SendMessageAsync(Message message);
        Task SendMessageAsync(Message message, CancellationToken cancellationToken);
    }
}