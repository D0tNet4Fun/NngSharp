using System;
using FluentAssertions;
using NngSharp.Data;
using Xunit;

namespace NngSharp.Tests.Messages
{
    public class MessageTests
    {
        [Fact]
        public void Dispose()
        {
            var message = new Message();
            message.Dispose();

            message.Ptr.Should().Be(IntPtr.Zero);
            message.Length.Should().Be(0);
        }

        [Fact]
        public void SetString_And_GetString()
        {
            using var message = new Message();
            message.SetString("hello world");

            var result = message.GetString();
            result.Should().Be("hello world");
        }

        //[Fact]
        //public void Insert()
        //{
        //    using var message = new Message(1); // 1 byte = \0
        //    const string data = "hello world";

        //    message.Insert(_encoding.GetBytes(data)); // shift-right the initial data so that byte[0] = \0 becomes byte[11]

        //    Assert.Equal(12, message.Length);
        //    var bodyAsString = _encoding.GetString(message.Body);
        //    Assert.Equal($"{data}\0", bodyAsString);
        //}

        //[Fact]
        //public void Append()
        //{
        //    using var message = new Message(1); // 1 byte = \0
        //    const string data = "hello world";

        //    message.Append(_encoding.GetBytes(data));

        //    Assert.Equal(12, message.Length);
        //    var bodyAsString = _encoding.GetString(message.Body);
        //    Assert.Equal($"\0{data}", bodyAsString);
        //}

        [Fact]
        public void Clear()
        {
            const string data = "hello world";
            using var message = new Message();
            message.SetString(data);
            message.Length.Should().BePositive();

            message.Clear();
            message.Length.Should().Be(0);
        }
    }
}