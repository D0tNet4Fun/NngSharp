using System;
using FluentAssertions;
using NngSharp.Data;
using Xunit;

namespace NngSharp.Tests.Data
{
    public class ZeroCopyBufferTests
    {
        [Fact]
        public void Constructor()
        {
            var buffer = new ZeroCopyBuffer();
            buffer.Ptr.Should().Be(IntPtr.Zero);
            buffer.Capacity.Should().Be(0);
            buffer.Length.Should().Be(0);
        }

        [Fact]
        public void Dispose()
        {
            var buffer = new ZeroCopyBuffer();
            buffer.Allocate(5);
            buffer.Ptr.Should().NotBe(IntPtr.Zero);
            buffer.Dispose();
            buffer.Ptr.Should().Be(IntPtr.Zero);
            buffer.Capacity.Should().Be(0);
            buffer.Length.Should().Be(0);
        }

        [Fact]
        public void Allocate()
        {
            var buffer = new ZeroCopyBuffer();
            buffer.Allocate(5);
            buffer.Capacity.Should().Be(5);
            buffer.Length.Should().Be(5);
        }

        [Fact]
        public void Allocate_Again_When_No_Extra_Memory_Is_Needed()
        {
            var buffer = new ZeroCopyBuffer();
            buffer.Allocate(5);
            buffer.Capacity.Should().Be(5);
            buffer.Length.Should().Be(5);
            var ptr1 = buffer.Ptr;
            buffer.Allocate(3);
            buffer.Capacity.Should().Be(5);
            buffer.Length.Should().Be(3);
            var ptr2 = buffer.Ptr;
            ptr2.Should().Be(ptr1);
        }

        [Fact]
        public void Allocate_Again_When_Extra_Memory_Is_Needed()
        {
            var buffer = new ZeroCopyBuffer();
            buffer.Allocate(3);
            buffer.Capacity.Should().Be(3);
            buffer.Length.Should().Be(3);
            var ptr1 = buffer.Ptr;
            buffer.Allocate(5);
            buffer.Capacity.Should().Be(5);
            buffer.Length.Should().Be(5);
            var ptr2 = buffer.Ptr;
            ptr2.Should().NotBe(ptr1);
        }
    }
}