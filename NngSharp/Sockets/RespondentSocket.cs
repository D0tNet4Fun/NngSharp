using NngSharp.Native;

namespace NngSharp.Sockets
{
    public class RespondentSocket : SocketBase
    {
        public RespondentSocket()
            : base(NativeMethods.nng_respondent0_open)
        {
        }
    }
}