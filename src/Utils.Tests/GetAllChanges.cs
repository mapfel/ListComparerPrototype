using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Utils.Tests
{
    public class GetAllChanges
    {
        [Fact]
        public void Performance_Check_With_1Mio_4Classes()
        {
            var oldValues = new List<TestDtoAsClass>();
            for (int i = 0; i < 10000; i++)
            {
                oldValues.Add(new TestDtoAsClass(i, i.ToString(), -1 * i));
            }

            // Create 1000 more
            var newValues = new List<TestDtoAsClass>();
            for (int i = 0; i < 11000; i++)
            {
                newValues.Add(new TestDtoAsClass(i, i.ToString(), -1 * i));
            }

            // remove randomly 1000
            var random = new Random();
            for (int i = 0; i < 1000; i++)
            {
                var index = random.Next(100, 9900);
                newValues.RemoveAt(index);
            }

            // change randomly 1000
            for (int i = 0; i < 1000; i++)
            {
                var index = random.Next(100, 9000);
                var newValue = newValues[index];
                newValue.Foo = $"new: {i.ToString()}";
            }

            var sut = Comparer<TestDtoAsClass>.GetAllChanges(oldValues, newValues);
            Assert.InRange(sut.Count(), 2700, 3000);
        }
    }
}