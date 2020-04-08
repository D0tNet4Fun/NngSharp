using System.Threading;
using System.Threading.Tasks;
using NngSharp.Data;

namespace NngSharp.Contracts
{
    public interface IAsyncReceiver
    {
        Task<Message> ReceiveMessageAsync();
        Task<Message> ReceiveMessageAsync(CancellationToken cancellationToken);
    }
}