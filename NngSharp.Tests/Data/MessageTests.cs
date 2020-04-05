using System;
using FluentAssertions;
using NngSharp.Data;
using Xunit;

namespace NngSharp.Tests.Data
{
    public class MessageTests
    {
        [Fact]
        public void Constructor()
        {
            var message = new Message();
            message.Ptr.Should().Be(IntPtr.Zero);
            message.Capacity.Should().Be(0);
            message.Length.Should().Be(0);
        }

        [Fact]
        public void Dispose()
        {
            var message = new Message();
            message.Allocate(5);
            message.Dispose();

            message.Ptr.Should().Be(IntPtr.Zero);
            message.Capacity.Should().Be(0);
            message.Length.Should().Be(0);
        }

        [Fact]
        public void Allocate()
        {
            using var message = new Message();
            message.Allocate(5);
            message.Capacity.Should().Be(5);
            message.Length.Should().Be(5);
        }

        [Fact]
        public void Allocate_Again_When_No_Extra_Memory_Is_Needed()
        {
            using var message = new Message();
            message.Allocate(5);
            message.Capacity.Should().Be(5);
            message.Length.Should().Be(5);
            var ptr1 = message.Ptr;
            message.Allocate(3);
            message.Capacity.Should().Be(5);
            message.Length.Should().Be(3);
            var ptr2 = message.Ptr;
            ptr2.Should().Be(ptr1);
        }

        [Fact]
        public void Allocate_Again_When_Extra_Memory_Is_Needed()
        {
            using var message = new Message();
            message.Allocate(3);
            message.Capacity.Should().Be(3);
            message.Length.Should().Be(3);
            var ptr1 = message.Ptr;
            message.Allocate(5);
            message.Capacity.Should().Be(5);
            message.Length.Should().Be(5);
            var ptr2 = message.Ptr;
            ptr2.Should().Be(ptr1, "because this is what nng_msg_realloc does when possible");
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