using System;
using System.Collections.Generic;
using System.Text;
using NngSharp.Messages;
using NngSharp.Native;

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

        public void Send(string data)
        {
            var encoding = Encoding.UTF8;
            var size = encoding.GetByteCount(data);
            var bytes = encoding.GetBytes(data);

            NngErrorCode errorCode;
            unsafe
            {
                fixed (byte* ptr = bytes)
                {
                    errorCode = NativeMethods.nng_send(_nngSocket, (IntPtr)ptr, (UIntPtr)size, default);
                }
            }

            ErrorHandler.ThrowIfError(errorCode);
        }

        public void SendZeroCopy(string data)
        {
            // allocate memory in NNG and use a span to update it with string data

            var encoding = Encoding.UTF8;
            var size = encoding.GetByteCount(data);

            var ptr = NativeMethods.nng_alloc((UIntPtr)size);

            Span<byte> span;
            unsafe
            {
                span = new Span<byte>(ptr.ToPointer(), size);
            }
            encoding.GetBytes(data, span); // now ptr contains the string data

            var errorCode = NativeMethods.nng_send(_nngSocket, ptr, (UIntPtr)size, NativeMethods.NngFlags.Allocate);
            ErrorHandler.ThrowIfError(errorCode);
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

        public string Receive()
        {
            var bytes = new byte[1024];

            var sizePtr = (UIntPtr)bytes.Length;
            NngErrorCode errorCode;
            unsafe
            {
                fixed (byte* ptr = bytes)
                {
                    errorCode = NativeMethods.nng_recv(_nngSocket, (IntPtr)ptr, ref sizePtr, default);
                }
            }
            ErrorHandler.ThrowIfError(errorCode);

            return Encoding.UTF8.GetString(bytes, 0, (int)sizePtr);
        }

        public string ReceiveZeroCopy()
        {
            // get the pointer from NNG and use a span to read the string data

            var errorCode = NativeMethods.nng_recv(_nngSocket, out var ptr, out var sizePtr, NativeMethods.NngFlags.Allocate);
            ErrorHandler.ThrowIfError(errorCode);

            ReadOnlySpan<byte> span;
            unsafe
            {
                span = new ReadOnlySpan<byte>(ptr.ToPointer(), (int)sizePtr);
            }

            return Encoding.UTF8.GetString(span);
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
    }
}

