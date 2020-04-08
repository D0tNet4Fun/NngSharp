using NngSharp.Native;

namespace NngSharp.Sockets
{
    public class PullerSocket : SocketBase
    {
        public PullerSocket()
            : base(NativeMethods.nng_pull0_open)
        {
        }
    }
}