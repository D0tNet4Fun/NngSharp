using NngSharp.Native;

namespace NngSharp.Sockets
{
    public class RequesterSocket : SocketBase
    {
        public RequesterSocket()
            : base(NativeMethods.nng_req0_open)
        {
        }
    }
}