using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NodaTime;
using Xunit;

namespace GraphQL.NodaTime.Tests
{
    public class InstantGraphTypeTests
    {
        [Fact]
        public void QueryReturnsSerializedData()
        {
            var schema = SchemaBuilder<InstantGraphType, Instant>.Build(
                Instant.FromUtc(2019, 10, 2, 12, 14, 18),
                instant => instant + Duration.FromHours(3)
            );
            schema.UseNodaTime();
            var json = schema.Execute(options =>
            {
                options.Query = "query { test }";
                options.Schema = schema;
            });
            var result = JsonConvert.DeserializeObject<QueryResult>(json);
            Assert.Equal("2019-10-02T12:14:18Z", result.Data.Test);
        }
    }
}
