using NngSharp.Native;

namespace NngSharp.Sockets
{
    public class PairSocket : SocketBase
    {
        public PairSocket()
        {
            var errorCode = NativeMethods.nng_pair0_open(out Id);
            ErrorHandler.ThrowIfError(errorCode);
        }
    }
}