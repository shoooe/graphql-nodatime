using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NodaTime;
using Xunit;

namespace GraphQL.NodaTime.Tests
{
    using Builder = TestBuilder<InstantGraphType, Instant>;

    public class InstantGraphTypeTests
    {
        [Fact]
        public void QueryReturnsSerializedData()
        {
            var result = Builder.Serialize<string>(
                Instant.FromUtc(2019, 10, 2, 12, 14, 18));
            Assert.Equal("2019-10-02T12:14:18Z", result.Data.Test);
        }

        [Fact]
        public void MutationParsesLiteral()
        {
            var result = Builder.ParseLiteral<string, string>(
                "2019-10-02T12:14:18Z", x => x + Duration.FromHours(3));
            Assert.Equal("2019-10-02T15:14:18Z", result.Data.Test);
        }

        [Fact]
        public void MutationParsesLiteralWithOffset()
        {
            var result = Builder.ParseLiteral<string, string>(
                "2019-10-02T12:14:18+01:30", x => x + Duration.FromMinutes(3));
            Assert.Equal("2019-10-02T10:47:18Z", result.Data.Test);
        }

        [Fact]
        public void MutationParsesLiteralWithOffsetWithoutColons()
        {
            var result = Builder.ParseLiteral<string, string>(
                "2019-10-02T12:14:18+0130", x => x + Duration.FromMinutes(3));
            Assert.Equal("2019-10-02T10:47:18Z", result.Data.Test);
        }

        [Fact]
        public void MutationParsesLiteralWithOffsetWithoutMinutes()
        {
            var result = Builder.ParseLiteral<string, string>(
                "2019-10-02T12:14:18+01", x => x + Duration.FromMinutes(3));
            Assert.Equal("2019-10-02T11:17:18Z", result.Data.Test);
        }

        [Fact]
        public void MutationParsesLiteralWithNegativeOffset()
        {
            var result = Builder.ParseLiteral<string, string>(
                "2019-10-02T12:14:18-01:30", x => x + Duration.FromMinutes(3));
            Assert.Equal("2019-10-02T13:47:18Z", result.Data.Test);
        }

        [Fact]
        public void MutationParsesLiteralWithNegativeOffsetWithoutColons()
        {
            var result = Builder.ParseLiteral<string, string>(
                "2019-10-02T12:14:18-0130", x => x + Duration.FromMinutes(3));
            Assert.Equal("2019-10-02T13:47:18Z", result.Data.Test);
        }

        [Fact]
        public void MutationParsesLiteralWithNegativeOffsetWithoutMinutes()
        {
            var result = Builder.ParseLiteral<string, string>(
                "2019-10-02T12:14:18-01", x => x + Duration.FromMinutes(3));
            Assert.Equal("2019-10-02T13:17:18Z", result.Data.Test);
        }

        [Fact]
        public void MutationDoesntParseMalformedLiteral()
        {
            var result = Builder.ParseLiteral<string, string>(
                "malformed", x => x + Duration.FromHours(3));
            Assert.Null(result.Data);
            Assert.NotEmpty(result.Errors);
        }

        [Fact]
        public void MutationDoesntTryToParseDateLiteral()
        {
            var result = Builder.ParseLiteral<string, string>(
                "2019-10-02", x => x + Duration.FromHours(3));
            Assert.Null(result.Data);
            Assert.NotEmpty(result.Errors);
        }

        [Fact]
        public void MutationDoesntTryToParseTimeLiteral()
        {

            var result = Builder.ParseLiteral<string, string>(
                "15:14:18", x => x + Duration.FromHours(3));
            Assert.Null(result.Data);
            Assert.NotEmpty(result.Errors);
        }

        [Fact]
        public void MutationParsesInput()
        {
            var result = Builder.ParseInput<string, string>(
                "2019-10-02T12:14:18Z", x => x + Duration.FromHours(3));
            Assert.Equal("2019-10-02T15:14:18Z", result.Data.Test);
        }

        [Fact]
        public void MutationParsesInputWithOffset()
        {
            var result = Builder.ParseInput<string, string>(
                "2019-10-02T12:14:18+01:30", x => x + Duration.FromMinutes(3));
            Assert.Equal("2019-10-02T10:47:18Z", result.Data.Test);
        }

        [Fact]
        public void MutationParsesInputWithOffsetWithoutColons()
        {
            var result = Builder.ParseInput<string, string>(
                "2019-10-02T12:14:18+0130", x => x + Duration.FromMinutes(3));
            Assert.Equal("2019-10-02T10:47:18Z", result.Data.Test);
        }

        [Fact]
        public void MutationParsesInputWithOffsetWithoutMinutes()
        {
            var result = Builder.ParseInput<string, string>(
                "2019-10-02T12:14:18+01", x => x + Duration.FromMinutes(3));
            Assert.Equal("2019-10-02T11:17:18Z", result.Data.Test);
        }

        [Fact]
        public void MutationParsesInputWithNegativeOffset()
        {
            var result = Builder.ParseInput<string, string>(
                "2019-10-02T12:14:18-01:30", x => x + Duration.FromMinutes(3));
            Assert.Equal("2019-10-02T13:47:18Z", result.Data.Test);
        }

        [Fact]
        public void MutationParsesInputWithNegativeOffsetWithoutColons()
        {
            var result = Builder.ParseInput<string, string>(
                "2019-10-02T12:14:18-0130", x => x + Duration.FromMinutes(3));
            Assert.Equal("2019-10-02T13:47:18Z", result.Data.Test);
        }

        [Fact]
        public void MutationParsesInputWithNegativeOffsetWithoutMinutes()
        {
            var result = Builder.ParseInput<string, string>(
                "2019-10-02T12:14:18-01", x => x + Duration.FromMinutes(3));
            Assert.Equal("2019-10-02T13:17:18Z", result.Data.Test);
        }

        [Fact]
        public void MutationDoesntParseMalformedInput()
        {
            var result = Builder.ParseInput<string, string>(
                "malformed", x => x + Duration.FromHours(3));
            Assert.Null(result.Data);
            Assert.NotEmpty(result.Errors);
        }

        [Fact]
        public void MutationDoesntTryToParseDateInput()
        {
            var result = Builder.ParseInput<string, string>(
                "2019-10-02", x => x + Duration.FromHours(3));
            Assert.Null(result.Data);
            Assert.NotEmpty(result.Errors);
        }

        [Fact]
        public void MutationDoesntTryToParseTimeInput()
        {

            var result = Builder.ParseInput<string, string>(
                "15:14:18", x => x + Duration.FromHours(3));
            Assert.Null(result.Data);
            Assert.NotEmpty(result.Errors);
        }
    }
}
