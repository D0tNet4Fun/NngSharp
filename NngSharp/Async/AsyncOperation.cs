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

        public TaskCompletionSource<object> TaskCompletionSource { get; } = new TaskCompletionSource<object>();

        public Task Task => TaskCompletionSource.Task;

        private void OnCancellationRequested()
        {
            NativeMethods.nng_aio_cancel(_nngAio);
        }
    }
}