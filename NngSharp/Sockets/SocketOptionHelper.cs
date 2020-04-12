using System;
using System.Runtime.InteropServices;
using NngSharp.Native;

namespace NngSharp.Sockets
{
    internal static class SocketOptionHelper
    {
        internal static bool GetBoolValue(NngSocket socket, string name)
        {
            NativeMethods.nng_socket_get_bool(socket, name, out var value).ThrowIfError();
            return value;
        }

        public static int GetInt32Value(NngSocket socket, string name)
        {
            NativeMethods.nng_socket_get_int(socket, name, out var value).ThrowIfError();
            return value;
        }

        public static void SetInt32Value(NngSocket socket, string name, int value)
        {
            NativeMethods.nng_socket_set_int(socket, name, value).ThrowIfError();
        }

        public static string GetStringValue(NngSocket socket, string name)
        {
            NativeMethods.nng_socket_get_string(socket, name, out var valuePtr).ThrowIfError();
            try
            {
                return Marshal.PtrToStringAnsi(valuePtr);
            }
            finally
            {
                NativeMethods.nng_strfree(valuePtr);
            }
        }

        public static void SetStringValue(NngSocket socket, string name, string value)
        {
            NativeMethods.nng_socket_set_string(socket, name, value).ThrowIfError();
        }

        public static TimeSpan GetTimeSpanValue(NngSocket socket, string name)
        {
            NativeMethods.nng_socket_get_ms(socket, name, out var value).ThrowIfError();
            return value.AsTimeSpan();
        }

        public static void SetTimeSpanValue(NngSocket socket, string name, in TimeSpan value)
        {
            NativeMethods.nng_socket_set_ms(socket, name, new NativeMethods.NngDuration(value)).ThrowIfError();
        }
    }
}