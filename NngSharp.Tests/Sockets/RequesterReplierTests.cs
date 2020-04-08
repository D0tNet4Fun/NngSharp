using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FluentAssertions;
using NngSharp.Data;
using NngSharp.Sockets;
using Xunit;

namespace NngSharp.Tests.Sockets
{
    public class RequesterReplierTests : IDisposable
    {
        private readonly RequesterSocket _requester;
        private readonly ReplierSocket _replier;

        private static readonly string Url = $"inproc://{nameof(RequesterReplierTests)}";

        public RequesterReplierTests()
        {
            _requester = new RequesterSocket();
            _replier = new ReplierSocket();
            _replier.Listen(Url);
            _requester.Dial(Url);
        }

        public void Dispose()
        {
            _requester?.Dispose();
            _replier?.Dispose();
        }

        [Fact]
        public async Task SendMessageAsync__And__ReceiveMessageAsync()
        {
            var message1 = new Message();
            message1.SetString("what's the time?");
            await _requester.SendMessageAsync(message1);

            var message2 = await _replier.ReceiveMessageAsync();
            message2.GetString().Should().Be("what's the time?");

            var dateTimeNow = DateTime.Now.ToString("F");
            message2.SetString($"it's {dateTimeNow}");
            await _replier.SendMessageAsync(message2);

            message1 = await _requester.ReceiveMessageAsync();
            message1.GetString().Should().Be($"it's {dateTimeNow}");
        }
    }
}