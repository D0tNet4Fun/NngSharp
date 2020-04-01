using System;
using System.Collections.Generic;
using System.Text;
using NngSharp.Data;
using NngSharp.Native;
using Buffer = NngSharp.Data.Buffer;

namespace NngSharp.Sockets
{
    public class SocketBase : IDisposable
    {
        private NngSocket _nngSocket;
        private readonly List<NngListener> _listeners = new List<NngListener>();
        private readonly List<NngDialer> _dialers = new List<NngDialer>();

        protected SocketBase(OpenSocket openSocket)
        {
            var errorCode = openSocket(out _nngSocket);
            ErrorHandler.ThrowIfError(errorCode);
        }

        public void Dispose()
        {
            var errorCode = NativeMethods.nng_close(_nngSocket);
            if (errorCode == NngErrorCode.Success)
            {
                _nngSocket = default;
                _listeners.Clear(); // Listeners are implicitly closed when the socket they are associated with is closed
                _dialers.Clear(); // Dialers are implicitly closed when the socket they are associated with is closed.
                return;
            }

            // todo: log error?
        }

        public void Listen(string url)
        {
            var errorCode = NativeMethods.nng_listen(_nngSocket, url, out var nngListener, default);
            ErrorHandler.ThrowIfError(errorCode);
            _listeners.Add(nngListener);
        }

        public void Dial(string url)
        {
            var errorCode = NativeMethods.nng_dial(_nngSocket, url, out var nngDialer, default);
            ErrorHandler.ThrowIfError(errorCode);
            _dialers.Add(nngDialer);
        }

        public void Send(Buffer buffer)
        {
            var errorCode = NativeMethods.nng_send(_nngSocket, buffer.Ptr, (UIntPtr)buffer.Length, default);
            ErrorHandler.ThrowIfError(errorCode);
        }

        public void SendZeroCopy(ZeroCopyBuffer buffer)
        {
            var errorCode = NativeMethods.nng_send(_nngSocket, buffer.Ptr, (UIntPtr)buffer.Length, NativeMethods.NngFlags.Allocate);
            ErrorHandler.ThrowIfError(errorCode);
            // from doc: data is "owned" by the function, and it will assume responsibility for calling nng_free() when it is no longer needed.
            // clear the data to mark it as no longer needed
            buffer.Clear();
        }

        public void SendMessage(Message message)
        {
            var errorCode = NativeMethods.nng_sendmsg(_nngSocket, message, default);
            ErrorHandler.ThrowIfError(errorCode);
        }

        public void SendMessage<T>(Message<T> message)
            where T : struct
        {
            SendMessage((Message)message);
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

        public ZeroCopyBuffer ReceiveZeroCopy()
        {
            var errorCode = NativeMethods.nng_recv(_nngSocket, out var ptr, out var sizePtr, NativeMethods.NngFlags.Allocate);
            ErrorHandler.ThrowIfError(errorCode);
            return new ZeroCopyBuffer(ptr, (int)sizePtr);
        }

        public Message ReceiveMessage()
        {
            var errorCode = NativeMethods.nng_recvmsg(_nngSocket, out var nngMessage, default);
            ErrorHandler.ThrowIfError(errorCode);

            return new Message(nngMessage);
        }

        public Message<T> ReceiveMessage<T>()
            where T : struct
        {
            var message = ReceiveMessage();
            return new Message<T>(message);
        }

        public GenericMessage<T> ReceiveGenericMessage<T>()
        {
            var message = ReceiveMessage();
            return new GenericMessage<T>(message);
        }
    }
}

