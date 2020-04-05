using System;
using System.Threading.Tasks;
using FluentAssertions;
using NngSharp.Data;
using NngSharp.Sockets;
using NngSharp.Sockets.Behaviors;
using Xunit;
using Buffer = NngSharp.Data.Buffer;

namespace NngSharp.Tests.Sockets.Behaviors
{
    public class SocketReceiveBehaviorTests : IDisposable
    {
        private readonly PairSocket _server;
        private readonly PairSocket _client;
        private readonly SocketReceiveBehavior _behavior;
        private static readonly string Url = $"inproc://{nameof(SocketReceiveBehaviorTests)}";

        public SocketReceiveBehaviorTests()
        {
            _server = new PairSocket();
            _server.Listen(Url);
            _client = new PairSocket();
            _client.Dial(Url);
            _behavior = new SocketReceiveBehavior(_server);
        }

        public void Dispose()
        {
            _server?.Dispose();
            _client?.Dispose();
        }

        private void Send()
        {
            var buffer = new Buffer();
            buffer.SetString("hello");
            _client.Send(buffer);
        }

        private static void Verify(IMemory memory) => memory.GetString().Should().Be("hello");

        [Fact]
        public void Receive()
        {
            Send();
            var buffer = _behavior.Receive();
            Verify(buffer);
        }

        [Fact]
        public void TryReceive()
        {
            Send();
            var result = _behavior.TryReceive(out var buffer);
            result.Should().BeTrue();
            Verify(buffer);
        }

        [Fact]
        public void ReceiveZeroCopy()
        {
            Send();
            var buffer = _behavior.ReceiveZeroCopy();
            Verify(buffer);
        }

        [Fact]
        public void TryReceiveZeroCopy()
        {
            Send();
            var result = _behavior.TryReceiveZeroCopy(out var buffer);
            result.Should().BeTrue();
            Verify(buffer);
        }

        [Fact]
        public void ReceiveMessage()
        {
            Send();
            var message = _behavior.ReceiveMessage();
            Verify(message);
        }

        [Fact]
        public void TryReceiveMessage()
        {
            Send();
            var result = _behavior.TryReceiveMessage(out var message);
            result.Should().BeTrue();
            Verify(message);
        }

        [Fact]
        public async Task ReceiveMessageAsync()
        {
            Send();
            var message = await _behavior.ReceiveMessageAsync();
            Verify(message);
        }
    }
}