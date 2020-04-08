using System.Threading;
using System.Threading.Tasks;
using NngSharp.Data;

namespace NngSharp.Contracts
{
    public interface IAsyncSender
    {
        Task SendMessageAsync(Message message);
        Task SendMessageAsync(Message message, CancellationToken cancellationToken);
    }
}