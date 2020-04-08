using NngSharp.Native;

namespace NngSharp.Sockets
{
    public class Pair1Socket : SocketBase
    {
        public Pair1Socket()
            : base(NativeMethods.nng_pair1_open)
        {

        }
    }
}