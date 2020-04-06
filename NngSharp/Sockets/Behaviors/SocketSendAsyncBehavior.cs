using System.Threading;
using System.Threading.Tasks;
using NngSharp.Async;
using NngSharp.Data;
using NngSharp.Native;

namespace NngSharp.Sockets.Behaviors
{
    public class SocketSendAsyncBehavior : IAsyncSender
    {
        private readonly NngSocket _nngSocket;
        private readonly AsyncContext _asyncContext;

        public SocketSendAsyncBehavior(NngSocket nngSocket, AsyncContext asyncContext)
        {
            _nngSocket = nngSocket;
            _asyncContext = asyncContext;
        }

        public Task SendMessageAsync(Message message) => SendMessageAsync(message, CancellationToken.None);

        public async Task SendMessageAsync(Message message, CancellationToken cancellationToken)
        {
            using var asyncOperation = new AsyncOperation(_asyncContext, cancellationToken);
            _asyncContext.SetCurrentOperation(asyncOperation);
            _asyncContext.SetMessage(message);
            NativeMethods.nng_send_aio(_nngSocket, _asyncContext);
            await asyncOperation.Task;
        }
    }
}