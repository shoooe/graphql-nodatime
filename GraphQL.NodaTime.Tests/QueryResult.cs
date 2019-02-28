using System.Collections.Generic;

namespace GraphQL.NodaTime.Tests
{
    public class ErrorResult
    {
        public string Message { get; set; }
        public string Code { get; set; }
    }

    public class QueryResult<T>
    {
        public class DataResult
        {
            public T Test { get; set; }
        }

        public DataResult Data { get; set; }

        public List<ErrorResult> Errors { get; set; }
    }
}