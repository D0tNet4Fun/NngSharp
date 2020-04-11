using System;
using NngSharp.Async;

namespace NngSharp.Sockets
{
    public class SocketOptions
    {
        private readonly AsyncOptions _asyncSendOptions;
        private readonly AsyncOptions _asyncReceiveOptions;

        public SocketOptions(AsyncOptions asyncSendOptions, AsyncOptions asyncReceiveOptions)
        {
            _asyncSendOptions = asyncSendOptions;
            _asyncReceiveOptions = asyncReceiveOptions;
        }

        public TimeSpan SendTimeout
        {
            get => TimeSpan.FromMilliseconds(_asyncSendOptions.TimeoutInMilliseconds);
            set => _asyncSendOptions.TimeoutInMilliseconds = (int)value.TotalMilliseconds;
        }

        public TimeSpan ReceiveTimeout
        {
            get => TimeSpan.FromMilliseconds(_asyncReceiveOptions.TimeoutInMilliseconds);
            set => _asyncReceiveOptions.TimeoutInMilliseconds = (int)value.TotalMilliseconds;
        }
    }
}