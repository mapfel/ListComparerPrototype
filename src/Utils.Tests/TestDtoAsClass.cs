namespace Utils.Tests
{
    public class TestDtoAsClass
    {
        public TestDtoAsClass(int id, string foo, int bar)
        {
            Id = id;
            Foo = foo;
            Bar = bar;
        }

        public int Id { get; }
        public string Foo { get; set; }
        public int Bar { get; }
    }
}