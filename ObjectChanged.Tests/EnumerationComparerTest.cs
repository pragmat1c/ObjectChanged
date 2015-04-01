using System.Collections.Generic;
using Xunit;

namespace ObjectChanged.Tests
{
    public class EnumerationComparerTest
    {
        public class EnumTest 
        {
            [ChangesId]
            public int Id { get; set; }
            [Changes]
            public bool Changed { get; set; }
        }

        public class EnumTest2
        {
            [ChangesId]
            public int Id;
            [Changes]
            public bool Changed { get; set; }
        }

        [Fact]
        public void TestHaveChangedNull()
        {
            Assert.False(ObjectComparer.HasChanged(null, null));
        }

        [Fact]
        public void TestHaveNotChanged()
        {
            var newList = new List<EnumTest> { new EnumTest { Id = 1 } };
            var oldList = new List<EnumTest> { new EnumTest { Id = 1 } };
            Assert.False(ObjectComparer.HasChanged(newList, oldList));
        }

        [Fact]
        public void TestHaveChanged()
        {
            var newList = new List<EnumTest> { new EnumTest { Id = 1, Changed = true} };
            var oldList = new List<EnumTest> { new EnumTest { Id = 1 } };
            Assert.False(ObjectComparer.HasChanged(newList, oldList));
        }

        [Fact]
        public void TestMismatchedNumbers()
        {
            var newList = new List<EnumTest> { new EnumTest { Id = 1 } };
            var oldList = new List<EnumTest> { new EnumTest { Id = 1 }, new EnumTest { Id = 2 } };
            Assert.True(ObjectComparer.HasChanged(newList, oldList));
            Assert.True(ObjectComparer.HasChanged(oldList, newList));
        }

        [Fact]
        public void TestEnumFieldandType()
        {
            var newList = new List<EnumTest> { new EnumTest { Id = 1 } };
            var oldList = new List<EnumTest2> { new EnumTest2 { Id = 1 },  };
            Assert.False (ObjectComparer.HasChanged(newList, oldList));
        }

        [Fact]
        public void TestEnumFieldandType2()
        {
            var newList = new List<EnumTest> { new EnumTest { Id = 1 } };
            var oldList = new List<EnumTest2> { new EnumTest2 { Id = 2 }, };

            // need to check if there is no matching value in other list
            Assert.True(ObjectComparer.HasChanged(newList, oldList));
            
        }
    }
}
