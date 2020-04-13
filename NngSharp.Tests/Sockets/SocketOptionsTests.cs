using System;
using System.Threading.Tasks;
using FluentAssertions;
using NngSharp.Data;
using NngSharp.Sockets;
using Xunit;

namespace NngSharp.Tests.Sockets
{
    public class SocketOptionsTests : IDisposable
    {
        private readonly Pair0Socket _socket;

        public SocketOptionsTests()
        {
            _socket = new Pair0Socket();
        }

        public void Dispose()
        {
            _socket?.Dispose();
        }

        [Fact]
        public void SendBufferDepth()
        {
            _socket.Options.SendBufferDepth = 3;
            _socket.Options.SendBufferDepth.Should().Be(3);
        }

        [Fact]
        public void ReceiveBufferDepth()
        {
            _socket.Options.ReceiveBufferDepth = 3;
            _socket.Options.ReceiveBufferDepth.Should().Be(3);
        }

        [Fact]
        public void SendTimeout__Throws()
        {
            var timeSpan = TimeSpan.FromMilliseconds(100);
            _socket.Options.SendTimeout = timeSpan;
            _socket.Options.SendTimeout.Should().Be(timeSpan);
            // try to send a message without being connected
            using var message = new Message();
            message.SetString("x");
            Action send = () => _socket.SendMessage(message);
            send.Should().Throw<TimeoutException>();
        }

        [Fact]
        public void ReceiveTimeout__Throws()
        {
            var timeSpan = TimeSpan.FromMilliseconds(100);
            _socket.Options.ReceiveTimeout = timeSpan;
            _socket.Options.ReceiveTimeout.Should().Be(timeSpan);

            // try to receive a message without being connected
            Func<Message> receive = () => _socket.ReceiveMessage();
            receive.Should().Throw<TimeoutException>();
        }

        [Fact]
        public void SendAsyncTimeout__Throws()
        {
            var timeSpan = TimeSpan.FromMilliseconds(100);
            _socket.Options.SendAsyncTimeout = timeSpan;
            _socket.Options.SendAsyncTimeout.Should().Be(timeSpan);

            // try to send a message async without being connected
            using var message = new Message();
            message.SetString("x");
            Func<Task> sendAsync = () => _socket.SendMessageAsync(message);
            sendAsync.Should().Throw<TimeoutException>();
        }

        [Fact]
        public void ReceiveAsyncTimeout__Throws()
        {
            var timeSpan = TimeSpan.FromMilliseconds(100);
            _socket.Options.ReceiveAsyncTimeout = timeSpan;
            _socket.Options.ReceiveAsyncTimeout.Should().Be(timeSpan);

            // try to receive a message async without being connected
            Func<Task<Message>> receiveAsync = () => _socket.ReceiveMessageAsync();
            receiveAsync.Should().Throw<TimeoutException>();
        }
    }
}