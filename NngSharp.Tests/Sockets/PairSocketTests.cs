using NngSharp.Sockets;
using Xunit;

namespace NngSharp.Tests.Sockets
{
    public class PairSocketTests
    {
        [Fact]
        public void CreatePairSocket()
        {
            using var socket1 = new PairSocket();
            using var socket2 = new PairSocket();
            var url1 = "tcp://127.0.0.1:25555";
            socket1.Listen(url1);
            socket2.Dial(url1);
        }
    }
}