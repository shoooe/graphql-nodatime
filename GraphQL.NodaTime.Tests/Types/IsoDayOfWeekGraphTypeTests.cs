using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NodaTime;
using Xunit;

namespace GraphQL.NodaTime.Tests
{
    public class IsoDayOfWeekGraphTypeTests
    {
        public static IsoDayOfWeek Increment(IsoDayOfWeek day)
        {
            var intRepr = (int)day;
            var nextIntRepr = Math.Max(1, (intRepr + 1) % 8);
            return (IsoDayOfWeek)nextIntRepr;
        }

        [Fact]
        public void QueryReturnsFriday()
        {
            var schema = SchemaBuilder<IsoDayOfWeekGraphType, IsoDayOfWeek>.Build(
                IsoDayOfWeek.Friday,
                Increment
            );
            var json = schema.Execute(options =>
            {
                options.Query = "query { test }";
                options.Schema = schema;
            });
            var result = JsonConvert.DeserializeObject<QueryResult<int>>(json);
            Assert.Equal(5, result.Data.Test);
        }

        [Fact]
        public void QueryReturnsMonday()
        {
            var schema = SchemaBuilder<IsoDayOfWeekGraphType, IsoDayOfWeek>.Build(
                IsoDayOfWeek.Monday,
                Increment
            );
            var json = schema.Execute(options =>
            {
                options.Query = "query { test }";
                options.Schema = schema;
            });
            var result = JsonConvert.DeserializeObject<QueryResult<int>>(json);
            Assert.Equal(1, result.Data.Test);
        }

        [Fact]
        public void QueryReturnsSunday()
        {
            var schema = SchemaBuilder<IsoDayOfWeekGraphType, IsoDayOfWeek>.Build(
                IsoDayOfWeek.Sunday,
                Increment
            );
            var json = schema.Execute(options =>
            {
                options.Query = "query { test }";
                options.Schema = schema;
            });
            var result = JsonConvert.DeserializeObject<QueryResult<int>>(json);
            Assert.Equal(7, result.Data.Test);
        }

        [Fact]
        public void MutationParsesLiteral()
        {
            var schema = SchemaBuilder<IsoDayOfWeekGraphType, IsoDayOfWeek>.Build(
                IsoDayOfWeek.Friday,
                Increment
            );
            var json = schema.Execute(options =>
            {
                options.Query = "mutation { test(arg: 3) }";
                options.Schema = schema;
            });
            var result = JsonConvert.DeserializeObject<QueryResult<int>>(json);
            Assert.Equal(4, result.Data.Test);
        }

        [Fact]
        public void MutationParsesInput()
        {
            var schema = SchemaBuilder<IsoDayOfWeekGraphType, IsoDayOfWeek>.Build(
                IsoDayOfWeek.Friday,
                Increment
            );
            var json = schema.Execute(options =>
            {
                options.Query = "mutation($arg: IsoDayOfWeek!) { test(arg: $arg) }";
                options.Schema = schema;
                options.Inputs = "{ \"arg\": 7 }".ToInputs();
            });
            var result = JsonConvert.DeserializeObject<QueryResult<int>>(json);
            Assert.Equal(1, result.Data.Test);
        }
    }
}
