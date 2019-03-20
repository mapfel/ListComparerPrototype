using Xunit;

namespace Utils.Tests
{
    public class ExtensionTest
    {


        private TestDtoAsStruct _sut = new TestDtoAsStruct(99, "test", -1);

        [Fact]
        public void Calling_Extension_Method_Should_Return_Value_Of_Id_Property()
        {
            var e = ComparisonResult.Added;
            var v = _sut.KeyValue();
            Assert.Equal(_sut.KeyValue(), 99);
        }
    }
}
