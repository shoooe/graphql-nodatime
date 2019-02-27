using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NodaTime;
using Xunit;

namespace GraphQL.NodaTime.Tests
{
    public class LocalTimeGraphTypeTests
    {
        [Fact]
        public void QueryReturnsSerializedDataWithDecimals()
        {
            var schema = SchemaBuilder<LocalTimeGraphType, LocalTime>.Build(
                LocalTime.FromHourMinuteSecondMillisecondTick(7, 53, 10, 9, 19),
                time => time + Period.FromMinutes(3)
            );
            var json = schema.Execute(options =>
            {
                options.Query = "query { test }";
                options.Schema = schema;
            });
            var result = JsonConvert.DeserializeObject<QueryResult<string>>(json);
            Assert.Equal("07:53:10.0090019", result.Data.Test);
        }

        [Fact]
        public void QueryReturnsSerializedDataWithoutDecimals()
        {
            var schema = SchemaBuilder<LocalTimeGraphType, LocalTime>.Build(
                LocalTime.FromHourMinuteSecondMillisecondTick(7, 53, 10, 0, 0),
                time => time + Period.FromMinutes(3)
            );
            var json = schema.Execute(options =>
            {
                options.Query = "query { test }";
                options.Schema = schema;
            });
            var result = JsonConvert.DeserializeObject<QueryResult<string>>(json);
            Assert.Equal("07:53:10", result.Data.Test);
        }

        [Fact]
        public void MutationParsesLiteralWithDecimals()
        {
            var schema = SchemaBuilder<LocalTimeGraphType, LocalTime>.Build(
                LocalTime.FromHourMinuteSecondMillisecondTick(7, 53, 10, 0, 0),
                time => time + Period.FromMinutes(3)
            );
            var json = schema.Execute(options =>
            {
                options.Query = "mutation { test(arg: \"09:34:01.2340087\") }";
                options.Schema = schema;
            });
            var result = JsonConvert.DeserializeObject<QueryResult<string>>(json);
            Assert.Equal("09:37:01.2340087", result.Data.Test);
        }

        [Fact]
        public void MutationParsesLiteralWithoutDecimals()
        {
            var schema = SchemaBuilder<LocalTimeGraphType, LocalTime>.Build(
                LocalTime.FromHourMinuteSecondMillisecondTick(7, 53, 10, 0, 0),
                time => time + Period.FromMinutes(3)
            );
            var json = schema.Execute(options =>
            {
                options.Query = "mutation { test(arg: \"09:34:01\") }";
                options.Schema = schema;
            });
            var result = JsonConvert.DeserializeObject<QueryResult<string>>(json);
            Assert.Equal("09:37:01", result.Data.Test);
        }

        [Fact]
        public void MutationParsesInputWithDecimals()
        {
            var schema = SchemaBuilder<LocalTimeGraphType, LocalTime>.Build(
                LocalTime.FromHourMinuteSecondMillisecondTick(7, 53, 10, 0, 0),
                time => time + Period.FromMinutes(3)
            );
            var json = schema.Execute(options =>
            {
                options.Query = "mutation($arg: LocalTime!) { test(arg: $arg) }";
                options.Schema = schema;
                options.Inputs = "{ \"arg\": \"13:04:43.0010876\" }".ToInputs();
            });
            var result = JsonConvert.DeserializeObject<QueryResult<string>>(json);
            Assert.Equal("13:07:43.0010876", result.Data.Test);
        }

        [Fact]
        public void MutationParsesInputWithoutDecimals()
        {
            var schema = SchemaBuilder<LocalTimeGraphType, LocalTime>.Build(
                LocalTime.FromHourMinuteSecondMillisecondTick(7, 53, 10, 0, 0),
                time => time + Period.FromMinutes(3)
            );
            var json = schema.Execute(options =>
            {
                options.Query = "mutation($arg: LocalTime!) { test(arg: $arg) }";
                options.Schema = schema;
                options.Inputs = "{ \"arg\": \"13:04:43\" }".ToInputs();
            });
            var result = JsonConvert.DeserializeObject<QueryResult<string>>(json);
            Assert.Equal("13:07:43", result.Data.Test);
        }
    }
}
