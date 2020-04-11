using System;
using System.Threading;
using System.Threading.Tasks;
using NngSharp.Native;

namespace NngSharp.Async
{
    public class AsyncOperation : IDisposable
    {
        private readonly NngAio _nngAio;
        private readonly CancellationTokenRegistration _cancellationTokenRegistration;
        private readonly TaskCompletionSource<object> _taskCompletionSource = new TaskCompletionSource<object>();

        public AsyncOperation(NngAio nngAio, CancellationToken cancellationToken)
        {
            _nngAio = nngAio;
            _cancellationTokenRegistration = cancellationToken.Register(OnCancellationRequested);
        }

        public void Dispose()
        {
            if (!Task.IsCompleted)
            {
                throw new NotImplementedException(); // todo
            }
            _cancellationTokenRegistration.Dispose();
        }

        public Task Task => _taskCompletionSource.Task;

        public void Complete()
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
                case NngErrorCode.TimedOut:
                    _taskCompletionSource.SetException(new TimeoutException());
                    break;
                default:
                    _taskCompletionSource.SetException(new NngException(errorCode));
                    break;
            }
        }

        private void OnCancellationRequested()
        {
            NativeMethods.nng_aio_cancel(_nngAio);
        }
    }
}