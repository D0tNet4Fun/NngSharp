using System;
using System.Threading;
using System.Threading.Tasks;
using NngSharp.Data;
using NngSharp.Native;
using Buffer = NngSharp.Data.Buffer;

namespace NngSharp.Sockets.Behaviors
{
    public class SocketSendBehavior : ISender
    {
        private readonly NngSocket _nngSocket;

        public SocketSendBehavior(NngSocket nngSocket)
        {
            _nngSocket = nngSocket;
        }

        public void Send(Buffer buffer)
        {
            var errorCode = NativeMethods.nng_send(_nngSocket, buffer.Ptr, (UIntPtr)buffer.Length, default);
            ErrorHandler.ThrowIfError(errorCode);
        }

        public bool TrySend(Buffer buffer)
        {
            var errorCode = NativeMethods.nng_send(_nngSocket, buffer.Ptr, (UIntPtr)buffer.Length, NativeMethods.NngFlags.Async);
            switch (errorCode)
            {
                case NngErrorCode.TryAgain:
                    return false;
                default:
                    ErrorHandler.ThrowIfError(errorCode);
                    return true; // only if success
            }
        }

        public void SendZeroCopy(ZeroCopyBuffer buffer)
        {
            var errorCode = NativeMethods.nng_send(_nngSocket, buffer.Ptr, (UIntPtr)buffer.Length, NativeMethods.NngFlags.Allocate);
            ErrorHandler.ThrowIfError(errorCode);
            // from doc: data is "owned" by the function, and it will assume responsibility for calling nng_free() when it is no longer needed.
            // clear the data to mark it as no longer needed
            buffer.Clear();
        }

        public bool TrySendZeroCopy(ZeroCopyBuffer buffer)
        {
            var errorCode = NativeMethods.nng_send(_nngSocket, buffer.Ptr, (UIntPtr)buffer.Length, NativeMethods.NngFlags.Async | NativeMethods.NngFlags.Allocate);
            if (errorCode == NngErrorCode.TryAgain) return false;
            ErrorHandler.ThrowIfError(errorCode);
            // from doc: data is "owned" by the function, and it will assume responsibility for calling nng_free() when it is no longer needed.
            // clear the data to mark it as no longer needed
            buffer.Clear();
            return true;
        }

        public void SendMessage(Message message)
        {
            var errorCode = NativeMethods.nng_sendmsg(_nngSocket, message, default);
            ErrorHandler.ThrowIfError(errorCode);
        }

        public bool TrySendMessage(Message message)
        {
            var errorCode = NativeMethods.nng_sendmsg(_nngSocket, message, NativeMethods.NngFlags.Async);
            if (errorCode == NngErrorCode.TryAgain) return false;
            ErrorHandler.ThrowIfError(errorCode);
            return true;
        }

        public Task SendMessageAsync(Message message) => SendMessageAsync(message, CancellationToken.None);

        public async Task SendMessageAsync(Message message, CancellationToken cancellationToken)
        {
            // ReSharper disable once ConvertToUsingDeclaration - does not work correctly with await (todo why??)
            using (var asyncOperation = new AsyncOperation(cancellationToken))
            {
                asyncOperation.SetMessage(message);
                NativeMethods.nng_send_aio(_nngSocket, asyncOperation);
                await asyncOperation.Task;
            }
        }
    }
}