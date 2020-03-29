using System.Runtime.InteropServices;

namespace NngSharp.Native
{
    using static Constants;

    internal static partial class NativeMethods
    {
        [DllImport(NngDll, CallingConvention = NngCallingConvention)]
        public static extern NngErrorCode nng_dial(NngSocket nngSocket, string url, out NngDialer nngDialer, NngFlags flags);

        [DllImport(NngDll, CallingConvention = NngCallingConvention)]
        public static extern NngErrorCode nng_listen(NngSocket nngSocket, string url, out NngListener nngListener, NngFlags flags);
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct NngDialer
    {
        private uint Id;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct NngListener
    {
        private uint Id;
    }
}