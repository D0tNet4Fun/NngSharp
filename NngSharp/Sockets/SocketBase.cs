using System;
using NngSharp.Native;

namespace NngSharp.Sockets
{
    public class SocketBase : IDisposable
    {
        private uint _id;

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
                return;
            }

            // todo: log error?
        }
    }

    public delegate NngErrorCode OpenSocket(out uint id); // common method signature for opening a socket using NativeMethods
}