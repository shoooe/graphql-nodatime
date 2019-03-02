using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NodaTime;
using Xunit;

namespace GraphQL.NodaTime.Tests
{
    using Builder = TestBuilder<LocalDateTimeGraphType, LocalDateTime>;

    public class LocalDateTimeGraphTypeTests
    {
        [Fact]
        public void QueryReturnsSerializedData()
        {
            var result = Builder.Serialize<string>(new LocalDateTime(2019, 10, 2, 12, 14, 19, 23));
            Assert.Equal("2019-10-02T12:14:19.023", result.Data.Test);
        }

        [Fact]
        public void QueryReturnsSerializedDataWithoutDecimals()
        {
            var result = Builder.Serialize<string>(new LocalDateTime(2019, 10, 2, 12, 14, 19));
            Assert.Equal("2019-10-02T12:14:19", result.Data.Test);
        }

        [Fact]
        public void QueryReturnsSerializedDataWithoutMinutes()
        {
            var result = Builder.Serialize<string>(new LocalDateTime(2019, 10, 2, 12, 14, 0));
            Assert.Equal("2019-10-02T12:14:00", result.Data.Test);
        }

        [Fact]
        public void MutationParsesLiteral()
        {
            var result = Builder.ParseLiteral<string, string>(
                "2019-10-02T12:14:18", x => x + Period.FromHours(3));
            Assert.Equal("2019-10-02T15:14:18", result.Data.Test);
        }

        [Fact]
        public void MutationDoesntParseMalformedLiteral()
        {
            var result = Builder.ParseLiteral<string, string>(
                "malformed", x => x + Period.FromHours(3));
            Assert.Null(result.Data);
            Assert.NotEmpty(result.Errors);
        }

        [Fact]
        public void MutationDoesntTryToParseDateLiteral()
        {
            var result = Builder.ParseLiteral<string, string>(
                "2019-10-02", x => x + Period.FromHours(3));
            Assert.Null(result.Data);
            Assert.NotEmpty(result.Errors);
        }

        [Fact]
        public void MutationDoesntTryToParseTimeLiteral()
        {

            var result = Builder.ParseLiteral<string, string>(
                "15:14:18", x => x + Period.FromHours(3));
            Assert.Null(result.Data);
            Assert.NotEmpty(result.Errors);
        }

        [Fact]
        public void MutationParsesInput()
        {
            var result = Builder.ParseInput<string, string>(
                "2019-10-02T12:14:18", x => x + Period.FromHours(3));
            Assert.Equal("2019-10-02T15:14:18", result.Data.Test);
        }

        [Fact]
        public void MutationDoesntParseMalformedInput()
        {
            var result = Builder.ParseInput<string, string>(
                "malformed", x => x + Period.FromHours(3));
            Assert.Null(result.Data);
            Assert.NotEmpty(result.Errors);
        }

        [Fact]
        public void MutationDoesntTryToParseDateInput()
        {
            var result = Builder.ParseInput<string, string>(
                "2019-10-02", x => x + Period.FromHours(3));
            Assert.Null(result.Data);
            Assert.NotEmpty(result.Errors);
        }

        [Fact]
        public void MutationDoesntTryToParseTimeInput()
        {

            var result = Builder.ParseInput<string, string>(
                "15:14:18", x => x + Period.FromHours(3));
            Assert.Null(result.Data);
            Assert.NotEmpty(result.Errors);
        }
    }
}
