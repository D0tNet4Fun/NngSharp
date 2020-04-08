using System.Threading;
using System.Threading.Tasks;
using NngSharp.Async;
using NngSharp.Data;

namespace NngSharp.Sockets.Behaviors
{
    public class SocketSendAsyncBehavior : IAsyncSender
    {
        private readonly AsyncContext _asyncContext;

        public SocketSendAsyncBehavior(AsyncContext asyncContext)
        {
            _asyncContext = asyncContext;
        }

        public Task SendMessageAsync(Message message) => SendMessageAsync(message, CancellationToken.None);

        public Task SendMessageAsync(Message message, CancellationToken cancellationToken) => _asyncContext.SendMessageAsync(message, cancellationToken);
    }
}