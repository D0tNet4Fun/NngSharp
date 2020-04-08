using NngSharp.Native;

namespace NngSharp.Sockets
{
    public class ReplierSocket : SocketBase
    {
        public ReplierSocket()
            : base(NativeMethods.nng_rep0_open)
        {
        }
    }
}