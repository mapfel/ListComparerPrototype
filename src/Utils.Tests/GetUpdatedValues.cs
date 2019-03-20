using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Utils.Tests
{
    public class GetUpdatedValues
    {
        [Fact]
        public void Comparing_1()
        {
            var oldValues = new List<TestDtoAsStruct>
            {
                new TestDtoAsStruct(0, "null", 0), new TestDtoAsStruct(1, "eins", -1), new TestDtoAsStruct(2, "zwei", -2),
                new TestDtoAsStruct(3, "drei", -3), new TestDtoAsStruct(4, "vier", -4)
            };


            var newValues = new List<TestDtoAsStruct>
            {
                new TestDtoAsStruct(1, "one", -1), new TestDtoAsStruct(2, "zwei", -2), new TestDtoAsStruct(3, "three", -3),
                new TestDtoAsStruct(4, "vier", -4), new TestDtoAsStruct(5, "fünf", -5)
            };

            var sut = Comparer<TestDtoAsStruct>.GetUpdatedValues(oldValues, newValues, null);
            Assert.Equal(2, sut.Count());
        }

        [Fact]
        public void Performance_Check_With_1Mio_4Classes()
        {
            var oldValues = new List<TestDtoAsClass>();
            for (int i = 0; i < 1000000; i++)
            {
                oldValues.Add(new TestDtoAsClass(i, i.ToString(), -1 * i));
            }

            // Create 1000 more
            var newValues = new List<TestDtoAsClass>();
            for (int i = 0; i < 1001000; i++)
            {
                newValues.Add(new TestDtoAsClass(i, i.ToString(), -1 * i));
            }

            // remove randomly 1000
            var random = new Random();
            for (int i = 0; i < 1000; i++)
            {
                var index = random.Next(1000, 909000);
                newValues.RemoveAt(index);
            }

            // change randomly 10000
            for (int i = 0; i < 1000; i++)
            {
                var index = random.Next(1000, 909000);
                var newValue = newValues[index];
                newValue.Foo = $"new: {i.ToString()}";
            }

            var sut = Comparer<TestDtoAsClass>.GetUpdatedValues(oldValues, newValues, null);
            Assert.Equal(1000, sut.Count());
        }
    }
}