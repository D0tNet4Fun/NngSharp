using System;
using FluentAssertions;
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
        public void IsRaw()
        {
            _socket.IsRaw.Should().BeFalse();
        }

        [Theory]
        [InlineData("cool socket")]
        [InlineData("zurück")]
        public void Name(string name)
        {
            _socket.Name = name;
            _socket.Name.Should().Be(name);
        }

        [Theory]
        [MemberData(nameof(SocketProvider))]
        public void ProtocolName(SocketBase socket, ProtocolType expectedProtocolType, string expectedProtocolName)
        {
            using (socket)
            {
                socket.ProtocolType.Should().Be(expectedProtocolType);
                socket.ProtocolName.Should().Be(expectedProtocolName);
            }
        }

        public static TheoryData<SocketBase, ProtocolType, string> SocketProvider
            => new TheoryData<SocketBase, ProtocolType, string>
            {
                { new BusSocket(), ProtocolType.Bus, "bus"},
                { new Pair0Socket(), ProtocolType.Pair0, "pair" },
                { new Pair1Socket(), ProtocolType.Pair1, "pair1" },
                { new PublisherSocket(), ProtocolType.Publish, "pub" },
                { new PullerSocket(), ProtocolType.Pull, "pull" },
                { new PusherSocket(), ProtocolType.Push, "push" },
                { new ReplierSocket(), ProtocolType.Reply, "rep" },
                { new RequesterSocket(), ProtocolType.Request, "req" },
                { new RespondentSocket(), ProtocolType.Respondent, "respondent" },
                { new SubscriberSocket(), ProtocolType.Subscribe, "sub" },
                { new SurveyorSocket(), ProtocolType.Survey, "surveyor" }
            };

        [Fact]
        public void Listen_When_Same_Url_Throws()
        {
            var url = "tcp://127.0.0.1:25000";
            _socket.Listen(url);
            Assert.Throws<NngException>(() => _socket.Listen(url));
        }
    }
}