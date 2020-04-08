using System;
using System.Linq;
using System.Threading;
using FluentAssertions;
using NngSharp.Async;
using NngSharp.Data;
using NngSharp.Sockets;
using Xunit;

namespace NngSharp.Tests.Async
{
    public class AsyncContextTests : IDisposable
    {
        private readonly PairSocket _server;
        private readonly PairSocket _client;
        private readonly AsyncContext _serverAsyncContext;
        private readonly AsyncContext _clientAsyncContext;
        private static readonly string Url = $"inproc://{nameof(AsyncContextTests)}";

        public AsyncContextTests()
        {
            _server = new PairSocket();
            _server.Listen(Url);
            _client = new PairSocket();
            _client.Dial(Url);
            _serverAsyncContext = new AsyncContext(_server);
            _clientAsyncContext = new AsyncContext(_client);
        }

        public void Dispose()
        {
            _client?.Dispose();
            _server?.Dispose();
        }

        [Fact]
        public void SendMessageAsync__In_Parallel__Is_Synchronized()
        {
            var messages = new[] {"a", "b", "c"}.Select(x =>
            {
                var message = new Message();
                message.SetString(x);
                return message;
            }).ToArray();

            // send the messages at the same time to verify they are sent in order because only one can be handled at a time
            var sendTasks = messages.Select(m => _clientAsyncContext.SendMessageAsync(m, CancellationToken.None)).ToArray();

            var receivedMessages = Enumerable.Range(0, messages.Length).Select(_ => _server.ReceiveMessage()).ToArray();
            receivedMessages.Should().HaveCount(3);
            var values = receivedMessages.Select(m => m.GetString()).ToArray();
            values[0].Should().Be("a");
            values[1].Should().Be("b");
            values[2].Should().Be("c");
        }

        [Fact]
        public void ReceiveMessageAsync__In_Parallel__Is_Synchronized()
        {
            var messages = new[] { "a", "b", "c" }.Select(x =>
            {
                var message = new Message();
                message.SetString(x);
                return message;
            }).ToArray();

            // send the messages synchronously one after the other - this is not important for the test
            foreach (var message in messages)
            {
                _client.SendMessage(message);
            }

            // receive the messages at the same time to verify they are received in order because only one can be handled at a time
            var receivedTasks = Enumerable.Range(0, messages.Length).Select(_ => _serverAsyncContext.ReceiveMessageAsync(CancellationToken.None)).ToArray();
            var receivedMessages = receivedTasks.Select(x => x.GetAwaiter().GetResult()).ToArray();
            receivedMessages.Should().HaveCount(3);
            var values = receivedMessages.Select(m => m.GetString()).ToArray();
            values[0].Should().Be("a");
            values[1].Should().Be("b");
            values[2].Should().Be("c");
        }

        [Fact]
        public void SendMessageAsync__And__ReceiveMessageAsync__In_Parallel__Are_Synchronized()
        {
            var messages = new[] { "a", "b", "c" }.Select(x =>
            {
                var message = new Message();
                message.SetString(x);
                return message;
            }).ToArray();

            // send the messages at the same time to verify they are sent in order because only one can be handled at a time
            var sendTasks = messages.Select(m => _clientAsyncContext.SendMessageAsync(m, CancellationToken.None)).ToArray();
            // don't await these - it's not important

            // receive the messages at the same time to verify they are received in order because only one can be handled at a time
            var receivedTasks = Enumerable.Range(0, messages.Length).Select(_ => _serverAsyncContext.ReceiveMessageAsync(CancellationToken.None)).ToArray();
            var receivedMessages = receivedTasks.Select(x => x.GetAwaiter().GetResult()).ToArray();
            receivedMessages.Should().HaveCount(3);
            var values = receivedMessages.Select(m => m.GetString()).ToArray();
            values[0].Should().Be("a");
            values[1].Should().Be("b");
            values[2].Should().Be("c");
        }
    }
}