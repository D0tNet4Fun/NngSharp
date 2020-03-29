using System;
using System.Runtime.InteropServices;

namespace NngSharp.Native
{
    using static Constants;

    internal static partial class NativeMethods
    {
        [DllImport(NngDll, CallingConvention = NngCallingConvention)]
        public static extern NngErrorCode nng_close(NngSocket nngSocket);

        [DllImport(NngDll, CallingConvention = NngCallingConvention)]
        public static extern NngErrorCode nng_recv(NngSocket nngSocket, IntPtr data, ref UIntPtr size, NngFlags flags);

        [DllImport(NngDll, CallingConvention = NngCallingConvention)]
        public static extern NngErrorCode nng_recv(NngSocket nngSocket, out IntPtr data, out UIntPtr size, NngFlags flags);

        [DllImport(NngDll, CallingConvention = NngCallingConvention)]
        public static extern NngErrorCode nng_send(NngSocket nngSocket, IntPtr data, UIntPtr size, NngFlags flags);
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct NngSocket
    {
        public uint Id;
    }
}