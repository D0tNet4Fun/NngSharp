using NngSharp.Native;

namespace NngSharp.Sockets
{
    public class BusSocket : SocketBase
    {
        public BusSocket()
            : base(NativeMethods.nng_bus0_open)
        {

        }
    }
}