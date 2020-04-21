using System;
using NngSharp.Async;
using NngSharp.Native;

namespace NngSharp.Sockets
{
    public class SocketOptions
    {
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
            get => SocketOptionHelper.GetInt32Value(_socket, Constants.Options.NNG_OPT_SENDBUF);
            set => SocketOptionHelper.SetInt32Value(_socket, Constants.Options.NNG_OPT_SENDBUF, value);
        }

        public int ReceiveBufferDepth
        {
            get => SocketOptionHelper.GetInt32Value(_socket, Constants.Options.NNG_OPT_RECVBUF);
            set => SocketOptionHelper.SetInt32Value(_socket, Constants.Options.NNG_OPT_RECVBUF, value);
        }

        public TimeSpan SendTimeout
        {
            get => SocketOptionHelper.GetTimeSpanValue(_socket, Constants.Options.NNG_OPT_SENDTIMEO);
            set => SocketOptionHelper.SetTimeSpanValue(_socket, Constants.Options.NNG_OPT_SENDTIMEO, value);
        }

        public TimeSpan ReceiveTimeout
        {
            get => SocketOptionHelper.GetTimeSpanValue(_socket, Constants.Options.NNG_OPT_RECVTIMEO);
            set => SocketOptionHelper.SetTimeSpanValue(_socket, Constants.Options.NNG_OPT_RECVTIMEO, value);
        }
    }
}