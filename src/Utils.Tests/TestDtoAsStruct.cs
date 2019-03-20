namespace Utils.Tests
{
    public struct TestDtoAsStruct
    {
        public TestDtoAsStruct(int id, string foo, int bar)
        {
            Id = id;
            Foo = foo;
            Bar = bar;
        }

        public int Id { get; }
        public string Foo { get; }
        public int Bar { get; }
    }
}