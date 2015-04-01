using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ObjectChanged.Tests
{
    public class ChangesAttributeTest
    {
        public class ChangeTest 
        {
            [Changes]
            public int HasChangesAttribute { get; set; }

            public bool NoChangesAttribute { get; set; }
        }

        [Fact]
        public void TestHasAttribute()
        {
            var item = new ChangeTest();

            var type = item.GetType();
            var properties = type.GetProperties();
            var count = properties.Count(p => p.GetCustomAttributes(false).Any(a => a.GetType() == typeof(ChangesAttribute)));

            Assert.Equal(1, count);
            

            
        }

        
    }
}
