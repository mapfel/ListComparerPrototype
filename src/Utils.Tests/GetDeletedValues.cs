using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Utils.Tests
{
    public class GetDeletedValues
    {
        [Fact]
        public void Comparing_Lists_With_Same_Keys_Should_Return_Empty_Result()
        {
            var oldValues = new List<TestDtoAsStruct>
            {
                new TestDtoAsStruct(1, "eins", -1), new TestDtoAsStruct(2, "zwei", -2), new TestDtoAsStruct(3, "drei", -3)
            };


            var newValues = new List<TestDtoAsStruct>
            {
                new TestDtoAsStruct(1, "eins", -1), new TestDtoAsStruct(2, "zwei", -2), new TestDtoAsStruct(3, "drei", -3)
            };

            var sut = Comparer<TestDtoAsStruct>.GetDeletedValues(oldValues, newValues);
            Assert.Empty(sut);
        }

        [Fact]
        public void Comparing_Lists_With_Same_Keys_Should_Return_Empty_Result2()
        {
            var oldValues = new List<TestDtoAsClass>
            {
                new TestDtoAsClass(1, "eins", -1), new TestDtoAsClass(2, "zwei", -2), new TestDtoAsClass(3, "drei", -3)
            };


            var newValues = new List<TestDtoAsClass>
            {
                new TestDtoAsClass(1, "eins", -1), new TestDtoAsClass(2, "zwei", -2), new TestDtoAsClass(3, "drei", -3)
            };

            var sut = Comparer<TestDtoAsClass>.GetDeletedValues(oldValues, newValues);
            Assert.Empty(sut);
        }

        [Fact]
        public void Comparing_Lists_With_One_Delete_Key_Should_Return_This_Key()
        {
            var deletedValue = new TestDtoAsStruct(1, "eins", -1);
            var oldValues = new List<TestDtoAsStruct>
            {
                deletedValue, new TestDtoAsStruct(2, "zwei", -2), new TestDtoAsStruct(3, "drei", -3)
            };


            var newValues = new List<TestDtoAsStruct>
            {
                new TestDtoAsStruct(2, "zwei", -2), new TestDtoAsStruct(3, "drei", -3)
            };

            var sut = Comparer<TestDtoAsStruct>.GetDeletedValues(oldValues, newValues);
            Assert.Contains(deletedValue, sut);
        }

        [Fact]
        public void Comparing_Lists_With_One_Additional_Key_Should_Return_Empty_Result()
        {
            var oldValues = new List<TestDtoAsStruct>
            {
                new TestDtoAsStruct(1, "eins", -1), new TestDtoAsStruct(2, "zwei", -2)
            };


            var newValues = new List<TestDtoAsStruct>
            {
                new TestDtoAsStruct(1, "eins", -1), new TestDtoAsStruct(2, "zwei", -2), new TestDtoAsStruct(3, "drei", -3)
            };

            var sut = Comparer<TestDtoAsStruct>.GetDeletedValues(oldValues, newValues);
            Assert.Empty(sut);
        }

        [Fact]
        public void Performance_Check_With_1Mio()
        {
            var oldValues = new List<TestDtoAsStruct>();
            for (int i = 0; i < 1000099; i++)
            {
                oldValues.Add(new TestDtoAsStruct(i, i.ToString(), -1*i));
            }

            var newValues = new List<TestDtoAsStruct>();
            for (int i = 0; i < 1000000; i++)
            {
                newValues.Add(new TestDtoAsStruct(i, i.ToString(), -1 * i));
            }

            var sut = Comparer<TestDtoAsStruct>.GetDeletedValues(oldValues, newValues);
            Assert.Equal(99, sut.Count());
        }
    }
}

