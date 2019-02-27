using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NodaTime;
using Xunit;

namespace GraphQL.NodaTime.Tests
{
    public class LocalDateGraphTypeTests
    {
        [Fact]
        public void QueryReturnsSerializedData()
        {
            var schema = SchemaBuilder<LocalDateGraphType, LocalDate>.Build(
                LocalDate.FromWeekYearWeekAndDay(2019, 42, IsoDayOfWeek.Friday),
                date => date + Period.FromDays(3)
            );
            var json = schema.Execute(options =>
            {
                options.Query = "query { test }";
                options.Schema = schema;
            });
            var result = JsonConvert.DeserializeObject<QueryResult<string>>(json);
            Assert.Equal("2019-10-18", result.Data.Test);
        }

        [Fact]
        public void MutationParsesLiteral()
        {
            var schema = SchemaBuilder<LocalDateGraphType, LocalDate>.Build(
                LocalDate.FromWeekYearWeekAndDay(2019, 42, IsoDayOfWeek.Friday),
                date => date + Period.FromDays(3)
            );
            var json = schema.Execute(options =>
            {
                options.Query = "mutation { test(arg: \"2019-07-12\") }";
                options.Schema = schema;
            });
            var result = JsonConvert.DeserializeObject<QueryResult<string>>(json);
            Assert.Equal("2019-07-15", result.Data.Test);
        }

        [Fact]
        public void MutationParsesInput()
        {
            var schema = SchemaBuilder<LocalDateGraphType, LocalDate>.Build(
                LocalDate.FromWeekYearWeekAndDay(2019, 42, IsoDayOfWeek.Friday),
                date => date + Period.FromDays(3)
            );
            var json = schema.Execute(options =>
            {
                options.Query = "mutation($arg: LocalDate!) { test(arg: $arg) }";
                options.Schema = schema;
                options.Inputs = "{ \"arg\": \"2019-12-03\" }".ToInputs();
            });
            var result = JsonConvert.DeserializeObject<QueryResult<string>>(json);
            Assert.Equal("2019-12-06", result.Data.Test);
        }
    }
}
