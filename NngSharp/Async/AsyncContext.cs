using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Nito.AsyncEx;
using NngSharp.Data;
using NngSharp.Native;

namespace NngSharp.Async
{
    public class AsyncContext : IDisposable
    {
        private readonly NngSocket _nngSocket;
        private NngAio _nngAio;

        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable - we don't want GC to collect it until AsyncContext goes out of scope
        private readonly NngAioCallback _callback;
        private readonly AsyncAutoResetEvent _isAvailable = new AsyncAutoResetEvent(true);
        private AsyncOperation _currentOperation;

        public AsyncContext(NngSocket nngSocket)
        {
            _nngSocket = nngSocket;
            _callback = OnCompleted;
            NativeMethods.nng_aio_alloc(out _nngAio, _callback, IntPtr.Zero).ThrowIfError();
            Options = new AsyncOptions(_nngAio);
        }

        public void Dispose()
        {
            NativeMethods.nng_aio_free(_nngAio);
            _nngAio = default;
        }

        public AsyncOptions Options { get; }

        private void OnCompleted(IntPtr arg)
        {
            RequireCurrentOperation();
            _currentOperation.Complete();
        }

        private async Task ExecuteAsyncOperation(Func<Task> asyncOperationCallback, CancellationToken cancellationToken)
        {
            // wait until the event is set async. this means the context can accept a new async operation.
            await _isAvailable.WaitAsync(cancellationToken);

            try
            {
                await asyncOperationCallback();
            }
            finally
            {
                // the context finished executing the current operation and can now accept a new async operation
                _currentOperation = null;
                _isAvailable.Set();
            }
        }

        public Task SendMessageAsync(Message message, CancellationToken cancellationToken)
        {
            async Task SendMessageAsync()
            {
                using (var asyncOperation = new AsyncOperation(_nngAio, cancellationToken))
                {
                    SetMessage(message);
                    _currentOperation = asyncOperation;
                    await SendAsync();
                }
            }

            return ExecuteAsyncOperation(SendMessageAsync, cancellationToken);
        }

        public async Task<Message> ReceiveMessageAsync(CancellationToken cancellationToken)
        {
            NngMsg nngMessage = default;
            async Task ReceiveMessageAsync()
            {
                using (var asyncOperation = new AsyncOperation(_nngAio, cancellationToken))
                {
                    _currentOperation = asyncOperation;
                    await ReceiveAsync();
                    nngMessage = GetMessage();
                }
            }

            await ExecuteAsyncOperation(ReceiveMessageAsync, cancellationToken);
            return new Message(nngMessage);
        }

        private void SetMessage(NngMsg message) => NativeMethods.nng_aio_set_msg(_nngAio, message);

        private NngMsg GetMessage() => NativeMethods.nng_aio_get_msg(_nngAio);

        private Task SendAsync()
        {
            RequireCurrentOperation();
            NativeMethods.nng_send_aio(_nngSocket, _nngAio);
            return _currentOperation.Task;
        }

        private Task ReceiveAsync()
        {
            RequireCurrentOperation();
            NativeMethods.nng_recv_aio(_nngSocket, _nngAio);
            return _currentOperation.Task;
        }

        [Conditional("DEBUG")]
        private void RequireCurrentOperation()
        {
            Debug.Assert(_currentOperation != null, "Current operation is not set");
        }
    }
}