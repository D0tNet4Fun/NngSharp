using System.Threading;
using System.Threading.Tasks;
using NngSharp.Data;

namespace NngSharp.Async
{
    public interface IAsyncSender
    {
        Task SendMessageAsync(Message message);
        Task SendMessageAsync(Message message, CancellationToken cancellationToken);
    }
}