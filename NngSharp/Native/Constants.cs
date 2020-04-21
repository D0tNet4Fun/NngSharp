using System.Runtime.InteropServices;

namespace NngSharp.Native
{
    internal static class Constants
    {
        public const string NngDll = "nng.dll";
        public const CallingConvention NngCallingConvention = CallingConvention.Cdecl;

        // ReSharper disable InconsistentNaming
        internal static class Options
        {
            internal const string NNG_OPT_SOCKNAME = "socket-name";
            internal const string NNG_OPT_RAW = "raw";
            internal const string NNG_OPT_PROTO = "protocol";
            internal const string NNG_OPT_PROTONAME = "protocol-name";
            internal const string NNG_OPT_PEER = "peer";
            internal const string NNG_OPT_PEERNAME = "peer-name";
            internal const string NNG_OPT_RECVBUF = "recv-buffer";
            internal const string NNG_OPT_SENDBUF = "send-buffer";
            internal const string NNG_OPT_RECVFD = "recv-fd";
            internal const string NNG_OPT_SENDFD = "send-fd";
            internal const string NNG_OPT_RECVTIMEO = "recv-timeout";
            internal const string NNG_OPT_SENDTIMEO = "send-timeout";
            internal const string NNG_OPT_LOCADDR = "local-address";
            internal const string NNG_OPT_REMADDR = "remote-address";
            internal const string NNG_OPT_URL = "url";
            internal const string NNG_OPT_MAXTTL = "ttl-max";
            internal const string NNG_OPT_RECVMAXSZ = "recv-size-max";
            internal const string NNG_OPT_RECONNMINT = "reconnect-time-min";

            internal const string NNG_OPT_RECONNMAXT = "reconnect-time-max";

            internal static class Protocol
            {
                internal const string NNG_OPT_SUB_SUBSCRIBE = "sub:subscribe";
                internal const string NNG_OPT_SUB_UNSUBSCRIBE = "sub:unsubscribe";
                internal const string NNG_OPT_SUB_PREFNEW = "sub:prefnew";
            }
        }
        // ReSharper restore InconsistentNaming
    }
}