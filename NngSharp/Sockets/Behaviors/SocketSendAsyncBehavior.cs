using System.Threading;
using System.Threading.Tasks;
using NngSharp.Async;
using NngSharp.Contracts;
using NngSharp.Data;
using NngSharp.Native;

namespace NngSharp.Sockets.Behaviors
{
    internal class SocketSendAsyncBehavior : IAsyncSender
    {
        private readonly AsyncContext _asyncContext;

        public SocketSendAsyncBehavior(NngSocket nngSocket)
        {
            _asyncContext = new AsyncContext(nngSocket);
        }

        public void Dispose()
        {
            _asyncContext?.Dispose();
        }

        public AsyncOptions Options => _asyncContext.Options;

        public Task SendMessageAsync(Message message) => SendMessageAsync(message, CancellationToken.None);

        public Task SendMessageAsync(Message message, CancellationToken cancellationToken) => _asyncContext.SendMessageAsync(message, cancellationToken);
    }
}