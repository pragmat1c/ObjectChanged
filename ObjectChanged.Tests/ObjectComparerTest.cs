using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ObjectChanged.Tests
{
    public class ObjectComparerTest
    {
        public class ChangeTest
        {
            [Changes]
            public int HasChangesAttribute { get; set; }

            public bool NoChangesAttribute { get; set; }
        }

        public class ChangeTest2
        {
            [Changes]
            public int HasChangesAttribute { get; set; }

        }

        public class ChangeTest3
        {
            [Changes]
            public int HasChangesAttribute;
        }

        public class ChangeTestIndexer
        {
            [Changes]
            public int this[int i]
            {
                get
                {
                    // This indexer is very simple, and just returns or sets 
                    // the corresponding element from the internal array. 
                    return i;
                }

            }
        }

        public class ChangeTestIndexer2
        {
            [Changes]
            public int this[int i]
            {
                get
                {
                    // This indexer is very simple, and just returns or sets 
                    // the corresponding element from the internal array. 
                    return i * 2;
                }
            }
        }

        public class ChangeTestTypes
        {
            [Changes]
            public int Int { get; set; }
            [Changes]
            public decimal Decimal { get; set; }
            [Changes]
            public bool Bool { get; set; }
            [Changes]
            public string String { get; set; }
            [Changes]
            public DateTime DateTime { get; set; }

        }

        public class ChangeTestGeneric<T>
        {
            [Changes]
            public T HasChangesAttribute { get; set; }
        }

        [Fact]
        public void TestHasChanged()
        {
            var orig = new ChangeTest { HasChangesAttribute = 1 };
            var curr = new ChangeTest { HasChangesAttribute = 2 };

            Assert.True(ObjectComparer.HasChanged(orig, curr));
        }

        [Fact]
        public void TestHasChangedField()
        {
            var orig = new ChangeTest3 { HasChangesAttribute = 1 };
            var curr = new ChangeTest3 { HasChangesAttribute = 2 };

            Assert.True(ObjectComparer.HasChanged(orig, curr));
        }

        [Fact]
        public void TestHasChangedDifferentTypes()
        {
            var orig = new ChangeTest { HasChangesAttribute = 1 };
            var curr = new ChangeTest2 { HasChangesAttribute = 2 };

            Assert.True(ObjectComparer.HasChanged(orig, curr));
        }

        [Fact]
        public void TestHasChangedFieldandProperty()
        {
            var orig = new ChangeTest { HasChangesAttribute = 1 };
            var curr = new ChangeTest3 { HasChangesAttribute = 2 };

            Assert.True(ObjectComparer.HasChanged(orig, curr));
        }

        [Fact]
        public void TestHasChangedIndex2()
        {
            var orig = new ChangeTestIndexer();
            var curr = new ChangeTestIndexer2();

            Assert.Throws<InvalidComparisionException>(() => { ObjectComparer.HasChanged(orig, curr); });
        }

        [Fact]
        public void TestHasChangedGeneric()
        {
            var orig = new ChangeTestGeneric<int> { HasChangesAttribute = 1 };
            var curr = new ChangeTestGeneric<string> { HasChangesAttribute = "1" };

            Assert.True(ObjectComparer.HasChanged(orig, curr));
        }

        [Fact]
        public void TestHasChangedGenericNumeric()
        {
            var orig = new ChangeTestGeneric<int> { HasChangesAttribute = 1 };
            var curr = new ChangeTestGeneric<decimal> { HasChangesAttribute = 1 };

            Assert.True(ObjectComparer.HasChanged(orig, curr));
        }

        [Fact]
        public void TestHasChangedGenericDateTime()
        {
            var orig = new ChangeTestGeneric<DateTime> { HasChangesAttribute = DateTime.MinValue };
            var curr = new ChangeTestGeneric<DateTime> { HasChangesAttribute = DateTime.MaxValue };

            Assert.True(ObjectComparer.HasChanged(orig, curr));
        }

        [Fact]
        public void TestHasChangedGenericNullableDateTime()
        {
            var orig = new ChangeTestGeneric<DateTime?> { HasChangesAttribute = DateTime.MinValue };
            var curr = new ChangeTestGeneric<DateTime?> { HasChangesAttribute = null };

            Assert.True(ObjectComparer.HasChanged(orig, curr));
        }

        [Fact]
        public void TestHasChangedTypes()
        {
            var orig = new ChangeTestTypes { Bool = false, DateTime = DateTime.MinValue, Decimal = Decimal.MinValue, Int = int.MinValue, String = "" };
            var curr = new ChangeTestTypes { Bool = true, DateTime = DateTime.MaxValue, Decimal = Decimal.MaxValue, Int = int.MaxValue, String = "blahblah" };

            Assert.True(ObjectComparer.HasChanged(orig, curr));
        }


    }
}
