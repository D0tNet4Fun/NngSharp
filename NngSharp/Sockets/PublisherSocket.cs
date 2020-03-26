using NngSharp.Native;

namespace NngSharp.Sockets
{
    public class PublisherSocket : SocketBase
    {
        public PublisherSocket()
            : base(NativeMethods.nng_pub0_open)
        {
        }
    }
}