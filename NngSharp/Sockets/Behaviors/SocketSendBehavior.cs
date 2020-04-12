using System;
using NngSharp.Contracts;
using NngSharp.Data;
using NngSharp.Native;
using Buffer = NngSharp.Data.Buffer;

namespace NngSharp.Sockets.Behaviors
{
    internal class SocketSendBehavior : ISender
    {
        private readonly NngSocket _nngSocket;

        public SocketSendBehavior(NngSocket nngSocket)
        {
            _nngSocket = nngSocket;
        }

        public void Send(Buffer buffer)
        {
            NativeMethods.nng_send(_nngSocket, buffer.Ptr, (UIntPtr)buffer.Length, default).ThrowIfError();
        }

        public bool TrySend(Buffer buffer)
        {
            var errorCode = NativeMethods.nng_send(_nngSocket, buffer.Ptr, (UIntPtr)buffer.Length, NativeMethods.NngFlags.Async);
            switch (errorCode)
            {
                case NngErrorCode.TryAgain:
                    return false;
                default:
                    errorCode.ThrowIfError();
                    return true; // only if success
            }
        }

        public void SendZeroCopy(ZeroCopyBuffer buffer)
        {
            NativeMethods.nng_send(_nngSocket, buffer.Ptr, (UIntPtr)buffer.Length, NativeMethods.NngFlags.Allocate).ThrowIfError();
            // from doc: data is "owned" by the function, and it will assume responsibility for calling nng_free() when it is no longer needed.
            // clear the data to mark it as no longer needed
            buffer.Clear();
        }

        public bool TrySendZeroCopy(ZeroCopyBuffer buffer)
        {
            var errorCode = NativeMethods.nng_send(_nngSocket, buffer.Ptr, (UIntPtr)buffer.Length, NativeMethods.NngFlags.Async | NativeMethods.NngFlags.Allocate);
            if (errorCode == NngErrorCode.TryAgain) return false;
            errorCode.ThrowIfError();
            // from doc: data is "owned" by the function, and it will assume responsibility for calling nng_free() when it is no longer needed.
            // clear the data to mark it as no longer needed
            buffer.Clear();
            return true;
        }

        public void SendMessage(Message message)
        {
            NativeMethods.nng_sendmsg(_nngSocket, message, default).ThrowIfError();
        }

        public bool TrySendMessage(Message message)
        {
            var errorCode = NativeMethods.nng_sendmsg(_nngSocket, message, NativeMethods.NngFlags.Async);
            if (errorCode == NngErrorCode.TryAgain) return false;
            errorCode.ThrowIfError();
            return true;
        }
    }
}