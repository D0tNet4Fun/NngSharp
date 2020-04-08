using NngSharp.Native;

namespace NngSharp.Sockets
{
    public class PusherSocket : SocketBase
    {
        public PusherSocket()
            : base(NativeMethods.nng_push0_open)
        {
        }
    }
}