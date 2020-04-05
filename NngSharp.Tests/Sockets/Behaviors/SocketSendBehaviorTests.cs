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
    public class SocketSendBehaviorTests : IDisposable
    {
        private readonly PairSocket _server;
        private readonly PairSocket _client;
        private readonly SocketSendBehavior _behavior;
        private static readonly string Url = $"inproc://{nameof(SocketSendBehaviorTests)}";

        public SocketSendBehaviorTests()
        {
            _server = new PairSocket();
            _server.Listen(Url);
            _client = new PairSocket();
            _client.Dial(Url);
            _behavior = new SocketSendBehavior(_client);
        }

        public void Dispose()
        {
            _server?.Dispose();
            _client?.Dispose();
        }

        private static void FillMemory(IMemory memory) => memory.SetString("hello");

        private void ReceiveAndVerify()
        {
            var buffer = _server.Receive(); // this blocks unless the client sends something, be it Buffer or ZeroCopyBuffer or Message
            buffer.GetString().Should().Be("hello");
        }

        [Fact]
        public void Send()
        {
            var buffer = new Buffer();
            FillMemory(buffer);
            _behavior.Send(buffer);
            ReceiveAndVerify();
        }

        [Fact]
        public void TrySend()
        {
            var buffer = new Buffer();
            FillMemory(buffer);
            var result = _behavior.TrySend(buffer);
            result.Should().BeTrue();
            ReceiveAndVerify();
        }

        [Fact]
        public void SendZeroCopy()
        {
            var buffer = new ZeroCopyBuffer();
            FillMemory(buffer);
            _client.SendZeroCopy(buffer);
            ReceiveAndVerify();
        }

        [Fact]
        public void TrySendZeroCopy()
        {
            var buffer = new ZeroCopyBuffer();
            FillMemory(buffer);
            var result = _behavior.TrySendZeroCopy(buffer);
            result.Should().BeTrue();
            ReceiveAndVerify();
        }

        [Fact]
        public void SendMessage()
        {
            var message = new Message();
            FillMemory(message);
            _behavior.SendMessage(message);
            ReceiveAndVerify();
        }

        [Fact]
        public void TrySendMessage()
        {
            var message = new Message();
            FillMemory(message);
            var result = _behavior.TrySendMessage(message);
            result.Should().BeTrue();
            ReceiveAndVerify();
        }

        [Fact]
        public async Task SendMessageAsync()
        {
            var message = new Message();
            FillMemory(message);
            await _client.SendMessageAsync(message);
            ReceiveAndVerify();
        }
    }
}