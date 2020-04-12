using System.Threading;
using System.Threading.Tasks;
using NngSharp.Async;
using NngSharp.Contracts;
using NngSharp.Data;
using NngSharp.Native;

namespace NngSharp.Sockets.Behaviors
{
    internal class SocketReceiveAsyncBehavior : IAsyncReceiver
    {
        private readonly AsyncContext _asyncContext;

        public SocketReceiveAsyncBehavior(NngSocket nngSocket)
        {
            _asyncContext = new AsyncContext(nngSocket);
        }

        public void Dispose()
        {
            _asyncContext?.Dispose();
        }

        public AsyncOptions Options => _asyncContext.Options;

        public Task<Message> ReceiveMessageAsync() => ReceiveMessageAsync(CancellationToken.None);

        public Task<Message> ReceiveMessageAsync(CancellationToken cancellationToken) => _asyncContext.ReceiveMessageAsync(cancellationToken);
    }
}