using System;
using System.Runtime.InteropServices;

namespace NngSharp.Native
{
    using static Constants;

    internal static partial class NativeMethods
    {
        [DllImport(NngDll, CallingConvention = NngCallingConvention)]
        public static extern NngErrorCode nng_aio_alloc(out NngAio nngAio, NngAioCallback callback, IntPtr arg);

        [DllImport(NngDll, CallingConvention = NngCallingConvention)]
        public static extern void nng_aio_free(NngAio aio);

        [DllImport(NngDll, CallingConvention = NngCallingConvention)]
        public static extern NngErrorCode nng_aio_result(NngAio aio);

        [DllImport(NngDll, CallingConvention = NngCallingConvention)]
        public static extern void nng_aio_set_msg(NngAio aio, NngMsg message);

        [DllImport(NngDll, CallingConvention = NngCallingConvention)]
        public static extern NngMsg nng_aio_get_msg(NngAio aio);

        [DllImport(NngDll, CallingConvention = NngCallingConvention)]
        public static extern void nng_send_aio(NngSocket socket, NngAio aio);

        [DllImport(NngDll, CallingConvention = NngCallingConvention)]
        public static extern void nng_recv_aio(NngSocket socket, NngAio aio);

        [DllImport(NngDll, CallingConvention = NngCallingConvention)]
        public static extern void nng_aio_cancel(NngAio aio);
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct NngAio
    {
        public IntPtr Ptr;
    }

    [UnmanagedFunctionPointer(NngCallingConvention)]
    public delegate void NngAioCallback(IntPtr arg);
}