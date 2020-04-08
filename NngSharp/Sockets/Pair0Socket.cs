using NngSharp.Native;

namespace NngSharp.Sockets
{
    public class Pair0Socket : SocketBase
    {
        public Pair0Socket()
            : base(NativeMethods.nng_pair0_open)
        {

        }
    }
}