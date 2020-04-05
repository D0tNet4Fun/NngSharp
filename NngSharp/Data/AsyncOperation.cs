using System;
using System.Threading;
using System.Threading.Tasks;
using NngSharp.Native;

namespace NngSharp.Data
{
    public class AsyncOperation : IDisposable
    {
        private NngAio _nngAio;

        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable - we don't want GC to collect it until AsyncOperation goes out of scope
        private readonly NngAioCallback _callback;
        private readonly TaskCompletionSource<object> _taskCompletionSource = new TaskCompletionSource<object>();
        private readonly CancellationTokenRegistration _cancellationTokenRegistration;

        public AsyncOperation(CancellationToken cancellationToken)
        {
            _callback = OnCompleted;
            var errorCode = NativeMethods.nng_aio_alloc(out _nngAio, _callback, IntPtr.Zero);
            ErrorHandler.ThrowIfError(errorCode);
            _cancellationTokenRegistration = cancellationToken.Register(OnCancellationRequested);
        }

        public void Dispose()
        {
            if (!Task.IsCompleted)
            {
                throw new NotImplementedException(); // todo
            }
            _cancellationTokenRegistration.Dispose();
            NativeMethods.nng_aio_free(_nngAio);
            _nngAio = default;
        }

        public static implicit operator NngAio(AsyncOperation op) => op._nngAio;

        public Task Task => _taskCompletionSource.Task;

        private void OnCancellationRequested()
        {
            NativeMethods.nng_aio_cancel(_nngAio);
        }

        private void OnCompleted(IntPtr arg)
        {
            var errorCode = NativeMethods.nng_aio_result(_nngAio);
            switch (errorCode)
            {
                case NngErrorCode.Success:
                    _taskCompletionSource.SetResult(null);
                    break;
                case NngErrorCode.OperationCanceled:
                    _taskCompletionSource.SetCanceled();
                    break;
                default:
                    _taskCompletionSource.SetException(new NngException(errorCode));
                    break;
            }
        }

        public void SetMessage(NngMsg message)
        {
            NativeMethods.nng_aio_set_msg(_nngAio, message);
        }

        public NngMsg GetMessage()
        {
            if(!Task.IsCompleted) throw new InvalidOperationException("Message is not available");
            return NativeMethods.nng_aio_get_msg(_nngAio);
        }
    }
}