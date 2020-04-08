using System;
using System.Net.Sockets;
using NngSharp.Sockets;
using Xunit;

namespace NngSharp.Tests.Sockets
{
    public class SocketBaseTests : IDisposable
    {
        private SocketBase _socket;

        public SocketBaseTests()
        {
            _socket = new Pair0Socket();
        }

        public void Dispose()
        {
            _socket?.Dispose();
        }

        [Fact]
        public void Listen_When_Same_Url_Throws()
        {
            var url = "tcp://127.0.0.1:25000";
            _socket.Listen(url);
            Assert.Throws<NngException>(() => _socket.Listen(url));
        }
    }
}