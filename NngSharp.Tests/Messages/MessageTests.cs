using System;
using System.Text;
using NngSharp.Messages;
using Xunit;

namespace NngSharp.Tests.Messages
{
    public class MessageTests
    {
        private readonly Encoding _encoding;

        public MessageTests()
        {
            _encoding = Encoding.UTF8;
        }

        [Fact]
        public void Dispose()
        {
            var message = new Message(1);
            message.Dispose();

            Assert.Equal(0, message.Length);
            Assert.Empty(message.Body.ToArray());
        }

        [Fact]
        public void Get_Body()
        {
            const string data = "hello world";

            using var message = new Message(data.Length);
            _encoding.GetBytes(data, message); // copy data to message

            var bodyAsString = _encoding.GetString(message.Body);
            Assert.Equal(data, bodyAsString);
        }

        [Fact]
        public void Insert()
        {
            using var message = new Message(1); // 1 byte = \0
            const string data = "hello world";

            message.Insert(_encoding.GetBytes(data)); // shift-right the initial data so that byte[0] = \0 becomes byte[11]

            Assert.Equal(12, message.Length);
            var bodyAsString = _encoding.GetString(message.Body);
            Assert.Equal($"{data}\0", bodyAsString);
        }

        [Fact]
        public void Append()
        {
            using var message = new Message(1); // 1 byte = \0
            const string data = "hello world";

            message.Append(_encoding.GetBytes(data));

            Assert.Equal(12, message.Length);
            var bodyAsString = _encoding.GetString(message.Body);
            Assert.Equal($"\0{data}", bodyAsString);
        }

        [Fact]
        public void Clear()
        {
            const string data = "hello world";
            using var message = new Message(data.Length);
            _encoding.GetBytes(data, message);
            Assert.True(message.Length > 0);

            message.Clear();
            Assert.Equal(0, message.Length);
        }
    }
}