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

        #region Options

        [DllImport(NngDll, CallingConvention = NngCallingConvention)]
        public static extern NngErrorCode nng_socket_get(NngSocket socket, string option, IntPtr value, ref UIntPtr valueSize);

        [DllImport(NngDll, CallingConvention = NngCallingConvention)]
        public static extern NngErrorCode nng_socket_get_bool(NngSocket socket, string option, out bool value);

        [DllImport(NngDll, CallingConvention = NngCallingConvention)]
        public static extern NngErrorCode nng_socket_get_int(NngSocket socket, string option, out int value);

        [DllImport(NngDll, CallingConvention = NngCallingConvention)]
        public static extern NngErrorCode nng_socket_get_string(NngSocket socket, string option, out IntPtr value);
        
        [DllImport(NngDll, CallingConvention = NngCallingConvention)]
        public static extern NngErrorCode nng_socket_get_ms(NngSocket socket, string name, out NngDuration value);

        [DllImport(NngDll, CallingConvention = NngCallingConvention)]
        public static extern NngErrorCode nng_socket_set(NngSocket socket, string option, IntPtr value, UIntPtr valueSize);

        [DllImport(NngDll, CallingConvention = NngCallingConvention)]
        public static extern NngErrorCode nng_socket_set_int(NngSocket socket, string option, int value);

        [DllImport(NngDll, CallingConvention = NngCallingConvention)]
        public static extern NngErrorCode nng_socket_set_string(NngSocket socket, string option, string value);

        [DllImport(NngDll, CallingConvention = NngCallingConvention)]
        public static extern NngErrorCode nng_socket_set_ms(NngSocket socket, string option, NngDuration value);

        #endregion
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct NngSocket
    {
        private uint Id;
    }
}