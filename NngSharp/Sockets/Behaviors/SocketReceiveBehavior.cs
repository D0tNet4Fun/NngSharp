using System;
using NngSharp.Contracts;
using NngSharp.Data;
using NngSharp.Native;
using Buffer = NngSharp.Data.Buffer;

namespace NngSharp.Sockets.Behaviors
{
    internal class SocketReceiveBehavior : IReceiver
    {
        private readonly NngSocket _nngSocket;

        public SocketReceiveBehavior(NngSocket nngSocket)
        {
            _nngSocket = nngSocket;
        }

        public Buffer Receive()
        {
            var buffer = new Buffer(128); // todo 

            var sizePtr = (UIntPtr)buffer.Length;
            var errorCode = NativeMethods.nng_recv(_nngSocket, buffer.Ptr, ref sizePtr, default);
            ErrorHandler.ThrowIfError(errorCode);

            buffer.Length = (int)sizePtr;
            return buffer;
        }

        public bool TryReceive(out Buffer buffer)
        {
            buffer = new Buffer(128); // todo 

            var sizePtr = (UIntPtr)buffer.Length;
            var errorCode = NativeMethods.nng_recv(_nngSocket, buffer.Ptr, ref sizePtr, NativeMethods.NngFlags.Async);
            if (errorCode == NngErrorCode.TryAgain)
            {
                buffer = null;
                return false;
            }
            ErrorHandler.ThrowIfError(errorCode);

            buffer.Length = (int)sizePtr;
            return true;
        }

        public ZeroCopyBuffer ReceiveZeroCopy()
        {
            var errorCode = NativeMethods.nng_recv(_nngSocket, out var ptr, out var sizePtr, NativeMethods.NngFlags.Allocate);
            ErrorHandler.ThrowIfError(errorCode);
            return new ZeroCopyBuffer(ptr, (int)sizePtr);
        }

        public bool TryReceiveZeroCopy(out ZeroCopyBuffer buffer)
        {
            var errorCode = NativeMethods.nng_recv(_nngSocket, out var ptr, out var sizePtr, NativeMethods.NngFlags.Allocate | NativeMethods.NngFlags.Async);
            if (errorCode == NngErrorCode.TryAgain)
            {
                buffer = null;
                return false;
            }
            ErrorHandler.ThrowIfError(errorCode);
            buffer = new ZeroCopyBuffer(ptr, (int)sizePtr);
            return true;
        }

        public Message ReceiveMessage()
        {
            var errorCode = NativeMethods.nng_recvmsg(_nngSocket, out var nngMessage, default);
            ErrorHandler.ThrowIfError(errorCode);
            return new Message(nngMessage);
        }

        public bool TryReceiveMessage(out Message message)
        {
            var errorCode = NativeMethods.nng_recvmsg(_nngSocket, out var nngMessage, NativeMethods.NngFlags.Async);
            if (errorCode == NngErrorCode.TryAgain)
            {
                message = null;
                return false;
            }
            ErrorHandler.ThrowIfError(errorCode);
            message = new Message(nngMessage);
            return true;
        }
    }
}