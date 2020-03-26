using NngSharp.Native;

namespace NngSharp.Sockets
{
    public class PairSocket : SocketBase
    {
        public PairSocket()
            : base(NativeMethods.nng_pair0_open)
        {

        }
    }
}