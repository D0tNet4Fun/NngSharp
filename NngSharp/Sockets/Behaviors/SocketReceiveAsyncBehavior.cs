using System.Threading;
using System.Threading.Tasks;
using NngSharp.Async;
using NngSharp.Data;

namespace NngSharp.Sockets.Behaviors
{
    public class SocketReceiveAsyncBehavior : IAsyncReceiver
    {
        private readonly AsyncContext _asyncContext;

        public SocketReceiveAsyncBehavior(AsyncContext asyncContext)
        {
            _asyncContext = asyncContext;
        }

        public Task<Message> ReceiveMessageAsync() => ReceiveMessageAsync(CancellationToken.None);

        public Task<Message> ReceiveMessageAsync(CancellationToken cancellationToken) => _asyncContext.ReceiveMessageAsync(cancellationToken);
    }
}