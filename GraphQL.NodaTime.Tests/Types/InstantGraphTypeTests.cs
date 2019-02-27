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
            var json = schema.Execute(options =>
            {
                options.Query = "query { test }";
                options.Schema = schema;
            });
            var result = JsonConvert.DeserializeObject<QueryResult<string>>(json);
            Assert.Equal("2019-10-02T12:14:18Z", result.Data.Test);
        }

        private void AssertParseLiteral(string from, string to, Duration difference)
        {
            var schema = SchemaBuilder<InstantGraphType, Instant>.Build(
                Instant.FromUtc(2019, 10, 2, 12, 14, 18),
                instant => instant + difference
            );
            var json = schema.Execute(options =>
            {
                options.Query = $"mutation {{ test(arg: \"{from}\") }}";
                options.Schema = schema;
            });
            var result = JsonConvert.DeserializeObject<QueryResult<string>>(json);
            Assert.Equal(to, result.Data.Test);
        }

        [Fact]
        public void MutationParsesLiteral()
        {
            AssertParseLiteral("2019-10-02T12:14:18Z", "2019-10-02T15:14:18Z", Duration.FromHours(3));
        }

        [Fact]
        public void MutationParsesLiteralWithOffset()
        {
            AssertParseLiteral("2019-10-02T12:14:18+01:30", "2019-10-02T10:47:18Z", Duration.FromMinutes(3));
        }

        [Fact]
        public void MutationParsesLiteralWithOffsetWithoutColons()
        {
            AssertParseLiteral("2019-10-02T12:14:18+0130", "2019-10-02T10:47:18Z", Duration.FromMinutes(3));
        }

        [Fact]
        public void MutationParsesLiteralWithOffsetWithoutMinutes()
        {
            AssertParseLiteral("2019-10-02T12:14:18+01", "2019-10-02T11:17:18Z", Duration.FromMinutes(3));
        }

        [Fact]
        public void MutationParsesLiteralWithNegativeOffset()
        {
            AssertParseLiteral("2019-10-02T12:14:18-01:30", "2019-10-02T13:47:18Z", Duration.FromMinutes(3));
        }

        [Fact]
        public void MutationParsesLiteralWithNegativeOffsetWithoutColons()
        {
            AssertParseLiteral("2019-10-02T12:14:18-0130", "2019-10-02T13:47:18Z", Duration.FromMinutes(3));
        }

        [Fact]
        public void MutationParsesLiteralWithNegativeOffsetWithoutMinutes()
        {
            AssertParseLiteral("2019-10-02T12:14:18-01", "2019-10-02T13:17:18Z", Duration.FromMinutes(3));
        }

        private void AssertParsesInput(string from, string to, Duration difference)
        {
            var schema = SchemaBuilder<InstantGraphType, Instant>.Build(
                Instant.FromUtc(2019, 10, 2, 12, 14, 18),
                instant => instant + difference
            );
            var json = schema.Execute(options =>
            {
                options.Query = "mutation($arg: Instant!) { test(arg: $arg) }";
                options.Schema = schema;
                options.Inputs = $"{{ \"arg\": \"{from}\" }}".ToInputs();
            });
            var result = JsonConvert.DeserializeObject<QueryResult<string>>(json);
            Assert.Equal(to, result.Data.Test);
        }

        [Fact]
        public void MutationParsesInput()
        {
            AssertParsesInput("2019-10-02T12:14:18Z", "2019-10-02T15:14:18Z", Duration.FromHours(3));
        }

        [Fact]
        public void MutationParsesInputWithOffset()
        {
            AssertParsesInput("2019-10-02T12:14:18+01:30", "2019-10-02T10:47:18Z", Duration.FromMinutes(3));
        }

        [Fact]
        public void MutationParsesInputWithOffsetWithoutColons()
        {
            AssertParsesInput("2019-10-02T12:14:18+0130", "2019-10-02T10:47:18Z", Duration.FromMinutes(3));
        }

        [Fact]
        public void MutationParsesInputWithOffsetWithoutMinutes()
        {
            AssertParsesInput("2019-10-02T12:14:18+01", "2019-10-02T11:17:18Z", Duration.FromMinutes(3));
        }

        [Fact]
        public void MutationParsesInputWithNegativeOffset()
        {
            AssertParsesInput("2019-10-02T12:14:18-01:30", "2019-10-02T13:47:18Z", Duration.FromMinutes(3));
        }

        [Fact]
        public void MutationParsesInputWithNegativeOffsetWithoutColons()
        {
            AssertParsesInput("2019-10-02T12:14:18-0130", "2019-10-02T13:47:18Z", Duration.FromMinutes(3));
        }

        [Fact]
        public void MutationParsesInputWithNegativeOffsetWithoutMinutes()
        {
            AssertParsesInput("2019-10-02T12:14:18-01", "2019-10-02T13:17:18Z", Duration.FromMinutes(3));
        }
    }
}
