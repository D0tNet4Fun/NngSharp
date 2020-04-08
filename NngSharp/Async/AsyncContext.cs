using System;
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
            var errorCode = NativeMethods.nng_aio_alloc(out _nngAio, _callback, IntPtr.Zero);
            ErrorHandler.ThrowIfError(errorCode);
        }

        public void Dispose()
        {
            NativeMethods.nng_aio_free(_nngAio);
            _nngAio = default;
        }

        private void OnCompleted(IntPtr arg)
        {
            var taskCompletionSource = _currentOperation.TaskCompletionSource;
            var errorCode = NativeMethods.nng_aio_result(_nngAio);
            switch (errorCode)
            {
                case NngErrorCode.Success:
                    taskCompletionSource.SetResult(null);
                    break;
                case NngErrorCode.OperationCanceled:
                    taskCompletionSource.SetCanceled();
                    break;
                default:
                    taskCompletionSource.SetException(new NngException(errorCode));
                    break;
            }
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

        public Task SendMessageAsync(NngMsg message, CancellationToken cancellationToken)
        {
            async Task SendMessageAsync()
            {
                using var asyncOperation = new AsyncOperation(_nngAio, cancellationToken);
                SetMessage(message);
                _currentOperation = asyncOperation;
                NativeMethods.nng_send_aio(_nngSocket, _nngAio);
                await asyncOperation.Task;
            }

            return ExecuteAsyncOperation(SendMessageAsync, cancellationToken);
        }

        public async Task<Message> ReceiveMessageAsync(CancellationToken cancellationToken)
        {
            NngMsg nngMessage = default;
            async Task ReceiveMessageAsync()
            {
                using var asyncOperation = new AsyncOperation(_nngAio, cancellationToken);
                _currentOperation = asyncOperation;
                NativeMethods.nng_recv_aio(_nngSocket, _nngAio);
                await asyncOperation.Task;
                nngMessage = GetMessage();
            }

            await ExecuteAsyncOperation(ReceiveMessageAsync, cancellationToken);
            return new Message(nngMessage);
        }

        private void SetMessage(NngMsg message) => NativeMethods.nng_aio_set_msg(_nngAio, message);

        private NngMsg GetMessage() => NativeMethods.nng_aio_get_msg(_nngAio);
    }
}