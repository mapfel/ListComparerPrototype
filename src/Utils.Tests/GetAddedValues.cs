using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Utils.Tests
{
    public class GetAddedValues
    {
        [Fact]
        public void Performance_Check_With_1Mio()
        {
            var oldValues = new List<TestDtoAsStruct>();
            for (int i = 0; i < 1000000; i++)
            {
                oldValues.Add(new TestDtoAsStruct(i, i.ToString(), -1 * i));
            }

            var newValues = new List<TestDtoAsStruct>();
            for (int i = 0; i < 1000099; i++)
            {
                newValues.Add(new TestDtoAsStruct(i, i.ToString(), -1 * i));
            }

            var sut = Comparer<TestDtoAsStruct>.GetAddedValues(oldValues, newValues);
            Assert.Equal(99, sut.Count());
        }
    }
}