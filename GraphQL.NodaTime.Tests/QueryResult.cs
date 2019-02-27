namespace GraphQL.NodaTime.Tests
{
    public class QueryResult<T>
    {
        public class DataResult
        {
            public T Test { get; set; }
        }

        public DataResult Data { get; set; }
    }
}