using System;
using System.Threading.Tasks;
using NngSharp.Data;
using NngSharp.Native;

namespace NngSharp.Async
{
    public class AsyncContext : IDisposable
    {
        private NngAio _nngAio;

        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable - we don't want GC to collect it until AsyncContext goes out of scope
        private readonly NngAioCallback _callback;
        private TaskCompletionSource<object> _taskCompletionSource;

        public AsyncContext()
        {
            _callback = OnCompleted;
            var errorCode = NativeMethods.nng_aio_alloc(out _nngAio, _callback, IntPtr.Zero);
            ErrorHandler.ThrowIfError(errorCode);
        }

        public void Dispose()
        {
            NativeMethods.nng_aio_free(_nngAio);
            _nngAio = default;
        }

        public static implicit operator NngAio(AsyncContext asyncContext) => asyncContext._nngAio;

        public void SetCurrentOperation(AsyncOperation asyncOperation)
        {
            _taskCompletionSource = asyncOperation.TaskCompletionSource; // todo thread safe
        }

        private void ClearCurrentOperation()
        {
            _taskCompletionSource = null; // todo thread safe
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
            ClearCurrentOperation();
        }

        public void SetMessage(NngMsg message) => NativeMethods.nng_aio_set_msg(_nngAio, message);

        public NngMsg GetMessage() => NativeMethods.nng_aio_get_msg(_nngAio);
    }
}