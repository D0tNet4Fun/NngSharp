using System.Threading;
using System.Threading.Tasks;
using NngSharp.Async;
using NngSharp.Data;
using NngSharp.Native;

namespace NngSharp.Sockets.Behaviors
{
    public class SocketReceiveAsyncBehavior : IAsyncReceiver
    {
        private readonly NngSocket _nngSocket;
        private readonly AsyncContext _asyncContext;

        public SocketReceiveAsyncBehavior(NngSocket nngSocket, AsyncContext asyncContext)
        {
            _nngSocket = nngSocket;
            _asyncContext = asyncContext;
        }

        public Task<Message> ReceiveMessageAsync() => ReceiveMessageAsync(CancellationToken.None);

        public async Task<Message> ReceiveMessageAsync(CancellationToken cancellationToken)
        {
            using var asyncOperation = new AsyncOperation(_asyncContext, cancellationToken);
            _asyncContext.SetCurrentOperation(asyncOperation);
            NativeMethods.nng_recv_aio(_nngSocket, _asyncContext);
            await asyncOperation.Task;
            return new Message(_asyncContext.GetMessage());
        }
    }
}