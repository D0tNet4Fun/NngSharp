using System;
using System.Runtime.InteropServices;
using System.Text;
using NngSharp.Messages;
using NngSharp.Sockets;
using Xunit;

namespace NngSharp.Tests.Sockets
{
    public class SocketTests
    {
        [Fact]
        public void Send_And_Receive()
        {
            using var sender = new PairSocket();
            using var receiver = new PairSocket();
            var url = "inproc://here";
            sender.Listen(url);
            receiver.Dial(url);
            sender.Send("hello!");
            var result = receiver.Receive();

            Assert.Equal("hello!", result);
        }

        [Fact]
        public void SendZeroCopy_And_ReceiveZeroCopy()
        {
            using var sender = new PairSocket();
            using var receiver = new PairSocket();
            var url = "inproc://here";
            sender.Listen(url);
            receiver.Dial(url);
            sender.SendZeroCopy("hello!");
            var result = receiver.ReceiveZeroCopy();

            Assert.Equal("hello!", result);
        }

        [Fact]
        public void SendMessage_And_ReceiveMessage_String()
        {
            // arrange
            var url = "inproc://here";
            using var server = new PairSocket();
            server.Listen(url);
            using var client = new PairSocket();
            client.Dial(url);

            const string data = "hello world";
            var message1 = new Message(data.Length);
            var encoding = Encoding.UTF8;
            encoding.GetBytes(data, message1);

            // act
            client.SendMessage(message1);
            var message2 = server.ReceiveMessage();

            // assert
            Assert.Equal(11, message2.Length);
            Assert.Equal(message1, message2);
        }

        [Fact]
        public void SendMessage_And_ReceiveMessage_Struct()
        {
            // arrange
            var url = "inproc://here";
            using var server = new PairSocket();
            server.Listen(url);
            using var client = new PairSocket();
            client.Dial(url);

            var data = new Point {x = 1, y = 2, z = 3};
            var message1 = new Message<Point>(data);

            // act
            client.SendMessage(message1);
            var message2 = server.ReceiveMessage<Point>();

            // assert
            Assert.Equal(12, message2.Length);
            var receivedData = message2.Value;
            Assert.Equal(data, receivedData);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    struct Point : IEquatable<Point>
    {
        public int x;
        public int y;
        public int z;

        public bool Equals(Point other)
        {
            return x == other.x && y == other.y && z == other.z;
        }

        public override bool Equals(object obj)
        {
            return obj is Point other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(x, y, z);
        }
    }
}