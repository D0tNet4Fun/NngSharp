using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Serialization;
using System.Text;
using FluentAssertions;
using NngSharp.Data;
using Xunit;

namespace NngSharp.Tests.Data
{
    public class DataExtensionTests
    {
        public static TheoryData<IMemory> Implementations => new TheoryData<IMemory>
        {
            new Buffer(),
            new ZeroCopyBuffer(),
            new Message()
        };

        [Theory]
        [MemberData(nameof(Implementations))]
        public void SetString_And_GetString(IMemory memory)
        {
            memory.SetString("hello");
            var result = memory.GetString();
            result.Should().Be("hello");
        }

        [Theory]
        [MemberData(nameof(Implementations))]
        public void SetString_And_GetString_Encoding(IMemory memory)
        {
            var encoding = Encoding.Unicode;
            memory.SetString("hello", encoding);
            var result = memory.GetString(encoding);
            result.Should().Be("hello");
        }

        [Theory]
        [MemberData(nameof(Implementations))]
        public void SetStruct_And_GetStruct(IMemory memory)
        {
            var point = new Point(1, 2);
            memory.SetStruct(point);
            var result = memory.GetStruct<Point>();
            result.Should().Match<Point>(p => p.X == 1 && p.Y == 2);
        }

        [Theory]
        [MemberData(nameof(Implementations))]
        public void SetDataContract_And_GetDataContract(IMemory memory)
        {
            var entity = new Entity() {Property = "value"};
            memory.SetDataContract(entity);
            var result = memory.GetDataContract<Entity>();
            result.Should().Match<Entity>(x => x.Property == "value");
        }

        [DataContract]
        private class Entity
        {
            [DataMember]
            public string Property { get; set; }
        }
    }
}