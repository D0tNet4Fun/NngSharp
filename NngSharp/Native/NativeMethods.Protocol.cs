using System.Runtime.InteropServices;

namespace NngSharp.Native
{
    using static Constants;

    internal static partial class NativeMethods
    {
        [DllImport(NngDll, CallingConvention = NngCallingConvention)]
        public static extern NngErrorCode nng_pair0_open(out uint socket);

        [DllImport(NngDll, CallingConvention = NngCallingConvention)]
        public static extern NngErrorCode nng_pub0_open(out uint socket);
    }
}