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
            var url = "ipc://hello/world";
            socket1.Listen(url);
        }
    }
}