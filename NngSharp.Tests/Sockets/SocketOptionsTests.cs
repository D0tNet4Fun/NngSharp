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
        public async Task SendAsync_Timeout()
        {
            // try to send a message async without being connected
            _socket.Options.SendTimeout = TimeSpan.FromMilliseconds(100);
            using var message = new Message();
            message.SetString("x");
            Func<Task> sendAsync = async () => await _socket.SendMessageAsync(message);
            await sendAsync.Should().ThrowAsync<TimeoutException>();
        }

        [Fact]
        public async Task ReceiveAsync_Timeout()
        {
            // try to receive a message async without being connected
            _socket.Options.ReceiveTimeout = TimeSpan.FromMilliseconds(100);
            Func<Task<Message>> receiveAsync = async () => await _socket.ReceiveMessageAsync();
            await receiveAsync.Should().ThrowAsync<TimeoutException>();
        }
    }
}