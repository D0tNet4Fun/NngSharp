using System.Threading;
using System.Threading.Tasks;
using NngSharp.Data;

namespace NngSharp.Async
{
    public interface IAsyncReceiver
    {
        Task<Message> ReceiveMessageAsync();
        Task<Message> ReceiveMessageAsync(CancellationToken cancellationToken);
    }
}