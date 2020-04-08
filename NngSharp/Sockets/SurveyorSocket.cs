using NngSharp.Native;

namespace NngSharp.Sockets
{
    public class SurveyorSocket : SocketBase
    {
        public SurveyorSocket()
            : base(NativeMethods.nng_surveyor0_open)
        {
        }
    }
}