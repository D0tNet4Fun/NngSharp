using System;
using FluentAssertions;
using NngSharp.Data;
using Xunit;

namespace NngSharp.Tests.Data
{
    public class ZeroCopyBufferTests
    {
        [Fact]
        public void Default_Constructor__Does_Not_Allocate_Memory()
        {
            var buffer = new ZeroCopyBuffer();
            buffer.Ptr.Should().Be(IntPtr.Zero);
            buffer.Capacity.Should().Be(0);
            buffer.Length.Should().Be(0);
        }

        [Fact]
        public void Dispose__Frees_Memory()
        {
            var buffer = new ZeroCopyBuffer();
            buffer.SetString("hello");
            buffer.Ptr.Should().NotBe(IntPtr.Zero);
            buffer.Dispose();
            buffer.Ptr.Should().Be(IntPtr.Zero);
            buffer.Capacity.Should().Be(0);
            buffer.Length.Should().Be(0);
        }

        [Fact]
        public void SetString__Allocates_Memory()
        {
            var buffer = new ZeroCopyBuffer();
            buffer.SetString("hello");
            buffer.Ptr.Should().NotBe(IntPtr.Zero);
            buffer.Capacity.Should().BePositive();
            buffer.Length.Should().BePositive();
        }

        [Fact]
        public void SetString_Again__When_Enough_Memory_Is_Available__Uses_Existing_Memory()
        {
            var buffer = new ZeroCopyBuffer();
            buffer.SetString("hello");
            buffer.Capacity.Should().Be(5);
            buffer.Length.Should().Be(5);
            var ptr1 = buffer.Ptr;
            buffer.SetString("hi");
            buffer.Capacity.Should().Be(5);
            buffer.Length.Should().Be(2);
            var ptr2 = buffer.Ptr;
            ptr2.Should().Be(ptr1);
        }

        [Fact]
        public void SetString_Again__When_Not_Enough_Memory_Is_Available__Allocates_New_Memory() // and also frees previous one but we can't prove it
        {
            var buffer = new ZeroCopyBuffer();
            buffer.SetString("hello");
            buffer.Capacity.Should().Be(5);
            var ptr1 = buffer.Ptr;
            buffer.SetString("hello there");
            buffer.Capacity.Should().Be(11);
            var ptr2 = buffer.Ptr;
            ptr2.Should().NotBe(ptr1);
        }
    }
}