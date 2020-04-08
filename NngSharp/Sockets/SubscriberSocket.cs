using NngSharp.Native;

namespace NngSharp.Sockets
{
    public class SubscriberSocket : SocketBase
    {
        public SubscriberSocket()
            : base(NativeMethods.nng_sub0_open)
        {
        }
    }
}