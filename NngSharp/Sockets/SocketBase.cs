using System;
using System.Collections.Generic;
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
    }

    public delegate NngErrorCode OpenSocket(out uint id); // common method signature for opening a socket using NativeMethods
}