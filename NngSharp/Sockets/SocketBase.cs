using System;
using NngSharp.Native;

namespace NngSharp.Sockets
{
    public class SocketBase : IDisposable
    {
        protected uint Id;

        protected SocketBase()
        {

        }
        
        public void Dispose()
        {
            if (Id <= 0) return;

            var errorCode = NativeMethods.nng_close(Id);
            if (errorCode == NngErrorCode.Success)
            {
                Id = 0;
                return;
            }

            // todo: log error?
        }
    }
}