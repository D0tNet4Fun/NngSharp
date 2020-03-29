using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using NngSharp.Messages;
using NngSharp.Native;

namespace NngSharp.Sockets
{
    public class SocketBase : IDisposable
    {
        private uint _id;
        private readonly List<uint> _listeners = new List<uint>();
        private readonly List<uint> _dialers = new List<uint>();

        protected SocketBase(OpenSocket openSocket)
        {
            var errorCode = openSocket(out _id);
            ErrorHandler.ThrowIfError(errorCode);
        }

        public void Dispose()
        {
            if (_id <= 0) return;

            var errorCode = NativeMethods.nng_close(_id);
            if (errorCode == NngErrorCode.Success)
            {
                _id = 0;
                _listeners.Clear(); // Listeners are implicitly closed when the socket they are associated with is closed
                _dialers.Clear(); // Dialers are implicitly closed when the socket they are associated with is closed.
                return;
            }

            // todo: log error?
        }

        public void Listen(string url)
        {
            var errorCode = NativeMethods.nng_listen(_id, url, out var listenerId, default);
            ErrorHandler.ThrowIfError(errorCode);
            _listeners.Add(listenerId);
        }

        public void Dial(string url)
        {
            var errorCode = NativeMethods.nng_dial(_id, url, out var dialerId, default);
            ErrorHandler.ThrowIfError(errorCode);
            _dialers.Add(dialerId);
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
                    errorCode = NativeMethods.nng_send(_id, (IntPtr)ptr, (UIntPtr)size, default);
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

            var errorCode = NativeMethods.nng_send(_id, ptr, (UIntPtr)size, NativeMethods.NngFlags.Allocate);
            ErrorHandler.ThrowIfError(errorCode);
        }

        public void SendMessage(Message message)
        {
            var errorCode = NativeMethods.nng_sendmsg(_id, message, default);
            ErrorHandler.ThrowIfError(errorCode);
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
                    errorCode = NativeMethods.nng_recv(_id, (IntPtr)ptr, ref sizePtr, default);
                }
            }
            ErrorHandler.ThrowIfError(errorCode);

            return Encoding.UTF8.GetString(bytes, 0, (int)sizePtr);
        }

        public string ReceiveZeroCopy()
        {
            // get the pointer from NNG and use a span to read the string data

            var errorCode = NativeMethods.nng_recv(_id, out var ptr, out var sizePtr, NativeMethods.NngFlags.Allocate);
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
            var errorCode = NativeMethods.nng_recvmsg(_id, out var messagePtr, default);
            ErrorHandler.ThrowIfError(errorCode);

            return new Message(messagePtr);
        }

        public delegate NngErrorCode
            OpenSocket(out uint id); // common method signature for opening a socket using NativeMethods
    }
}

