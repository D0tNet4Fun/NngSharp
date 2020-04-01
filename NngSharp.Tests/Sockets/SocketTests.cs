using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using FluentAssertions;
using NngSharp.Data;
using NngSharp.Sockets;
using Xunit;
using Buffer = NngSharp.Data.Buffer;

namespace NngSharp.Tests.Sockets
{
    public class SocketTests
    {
        [Fact]
        public void Send_And_Receive_String()
        {
            using var server = new PairSocket();
            using var client = new PairSocket();
            var url = "inproc://here";
            server.Listen(url);
            client.Dial(url);

            var buffer = new Buffer();
            buffer.SetString("hello!");
            client.Send(buffer);
            
            var buffer2 = server.Receive();
            buffer2.GetString().Should().Be("hello!");
        }

        [Fact]
        public void Send_And_Receive_Struct()
        {
            using var server = new PairSocket();
            using var client = new PairSocket();
            var url = "inproc://here";
            server.Listen(url);
            client.Dial(url);

            var @struct = new Point {x = 1, y = 2, z = 3};
            var buffer = new Buffer();
            buffer.SetStruct(@struct);
            client.Send(buffer);
            
            var buffer2 = server.Receive();
            var result = buffer2.GetStruct<Point>();

            Assert.Equal(@struct, result);
        }

        [Fact]
        public void SendZeroCopy_And_ReceiveZeroCopy()
        {
            using var server = new PairSocket();
            using var client = new PairSocket();
            var url = "inproc://here";
            server.Listen(url);
            client.Dial(url);

            var encoding = Encoding.UTF8;

            using (var buffer = new ZeroCopyBuffer())
            {
                buffer.SetString("hello!");
                client.SendZeroCopy(buffer);
                Assert.Equal(0, buffer.Length);
            }

            using (var serverBuffer = server.ReceiveZeroCopy())
            {
                Assert.Equal("hello!", serverBuffer.GetString(encoding));
                serverBuffer.SetString("hi!", encoding);
                server.SendZeroCopy(serverBuffer);
            }

            using (var buffer = client.ReceiveZeroCopy())
            {
                Assert.Equal("hi!", buffer.GetString(encoding));
            }
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

        [Fact]
        public void SendMessage_And_ReceiveMessage_Class()
        {
            // arrange
            var url = "inproc://here";
            using var server = new PairSocket();
            server.Listen(url);
            using var client = new PairSocket();
            client.Dial(url);

            var data = new SmartPoint { x = 1, y = 2, z = 3, t = 4 };
            var message1 = new GenericMessage<SmartPoint>(180);
            message1.Value = data;

            // act
            client.SendMessage(message1);
            var message2 = server.ReceiveGenericMessage<SmartPoint>();

            // assert
            Assert.Equal(180, message2.Length);
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

    [DataContract]
    class SmartPoint : IEquatable<SmartPoint>
    {
        [DataMember]
        public int x;
        [DataMember]
        public int y;
        [DataMember]
        public int z;
        [DataMember]
        public int t;

        public bool Equals(SmartPoint other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return x == other.x && y == other.y && z == other.z && t == other.t;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SmartPoint) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(x, y, z, t);
        }
    }
}