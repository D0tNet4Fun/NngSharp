using System;
using NngSharp.Async;
using NngSharp.Native;

namespace NngSharp.Sockets
{
    public class SocketOptions
    {
        // ReSharper disable InconsistentNaming
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
        // ReSharper restore InconsistentNaming

        private readonly NngSocket _socket;
        private readonly AsyncOptions _asyncSendOptions;
        private readonly AsyncOptions _asyncReceiveOptions;

        public SocketOptions(NngSocket socket, AsyncOptions asyncSendOptions, AsyncOptions asyncReceiveOptions)
        {
            _socket = socket;
            _asyncSendOptions = asyncSendOptions;
            _asyncReceiveOptions = asyncReceiveOptions;
        }

        public TimeSpan SendAsyncTimeout
        {
            get => _asyncSendOptions.TimeoutInMilliseconds;
            set => _asyncSendOptions.TimeoutInMilliseconds = value;
        }

        public TimeSpan ReceiveAsyncTimeout
        {
            get => _asyncReceiveOptions.TimeoutInMilliseconds;
            set => _asyncReceiveOptions.TimeoutInMilliseconds = value;
        }

        public int SendBufferDepth
        {
            get => SocketOptionHelper.GetInt32Value(_socket, NNG_OPT_SENDBUF);
            set => SocketOptionHelper.SetInt32Value(_socket, NNG_OPT_SENDBUF, value);
        }

        public int ReceiveBufferDepth
        {
            get => SocketOptionHelper.GetInt32Value(_socket, NNG_OPT_RECVBUF);
            set => SocketOptionHelper.SetInt32Value(_socket, NNG_OPT_RECVBUF, value);
        }

        public TimeSpan SendTimeout
        {
            get => SocketOptionHelper.GetTimeSpanValue(_socket, NNG_OPT_SENDTIMEO);
            set => SocketOptionHelper.SetTimeSpanValue(_socket, NNG_OPT_SENDTIMEO, value);
        }

        public TimeSpan ReceiveTimeout
        {
            get => SocketOptionHelper.GetTimeSpanValue(_socket, NNG_OPT_RECVTIMEO);
            set => SocketOptionHelper.SetTimeSpanValue(_socket, NNG_OPT_RECVTIMEO, value);
        }
    }
}