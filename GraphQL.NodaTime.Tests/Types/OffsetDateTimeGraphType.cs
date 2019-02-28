using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NodaTime;
using Xunit;

namespace GraphQL.NodaTime.Tests
{
    using Builder = TestBuilder<OffsetDateTimeGraphType, OffsetDateTime>;

    public class OffsetDateTimeGraphTypeTests
    {
        [Fact]
        public void QueryReturnsSerializedData()
        {
            var result = Builder.Serialize<string>(
                new OffsetDateTime(
                    new LocalDateTime(2019, 10, 2, 12, 14, 19, 23), 
                    Offset.FromHoursAndMinutes(1, 30)));
            Assert.Equal("2019-10-02T12:14:19.023+01:30", result.Data.Test);
        }

        [Fact]
        public void QueryReturnsSerializedDataWithNegativeOffset()
        {
            var result = Builder.Serialize<string>(
                new OffsetDateTime(
                    new LocalDateTime(2019, 10, 2, 12, 14, 19, 23), 
                    -Offset.FromHoursAndMinutes(1, 30)));
            Assert.Equal("2019-10-02T12:14:19.023-01:30", result.Data.Test);
        }

        [Fact]
        public void QueryReturnsSerializedDataWithoutOffset()
        {
            var result = Builder.Serialize<string>(
                new OffsetDateTime(
                    new LocalDateTime(2019, 10, 2, 12, 14, 19, 23), 
                    Offset.Zero));
            Assert.Equal("2019-10-02T12:14:19.023Z", result.Data.Test);
        }

        [Fact]
        public void QueryReturnsSerializedDataWithoutDecimals()
        {
            var result = Builder.Serialize<string>(
                new OffsetDateTime(
                    new LocalDateTime(2019, 10, 2, 12, 14, 19), 
                    Offset.FromHoursAndMinutes(1, 30)));
            Assert.Equal("2019-10-02T12:14:19+01:30", result.Data.Test);
        }

        [Fact]
        public void QueryReturnsSerializedDataWithoutMinutes()
        {
            var result = Builder.Serialize<string>(
                new OffsetDateTime(
                    new LocalDateTime(2019, 10, 2, 12, 14, 19), 
                    Offset.FromHours(1)));
            Assert.Equal("2019-10-02T12:14:19+01", result.Data.Test);
        }

        [Fact]
        public void QueryReturnsSerializedDataWithoutOffsetAndDecimals()
        {
            var result = Builder.Serialize<string>(
                new OffsetDateTime(
                    new LocalDateTime(2019, 10, 2, 12, 14, 19), 
                    Offset.Zero));
            Assert.Equal("2019-10-02T12:14:19Z", result.Data.Test);
        }

        [Fact]
        public void MutationParsesLiteral()
        {
            var result = Builder.ParseLiteral<string, string>(
                "2019-10-02T12:14:18+01:47", x => x + Duration.FromHours(3));
            Assert.Equal("2019-10-02T15:14:18+01:47", result.Data.Test);
        }

        [Fact]
        public void MutationParsesLiteralWithoutOffset()
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
            Assert.Equal("2019-10-02T12:17:18+01:30", result.Data.Test);
        }

        [Fact]
        public void MutationParsesLiteralWithOffsetWithoutColons()
        {
            var result = Builder.ParseLiteral<string, string>(
                "2019-10-02T12:14:18+0130", x => x + Duration.FromMinutes(3));
            Assert.Equal("2019-10-02T12:17:18+01:30", result.Data.Test);
        }

        [Fact]
        public void MutationParsesLiteralWithOffsetWithoutMinutes()
        {
            var result = Builder.ParseLiteral<string, string>(
                "2019-10-02T12:14:18+01", x => x + Duration.FromMinutes(3));
            Assert.Equal("2019-10-02T12:17:18+01", result.Data.Test);
        }

        [Fact]
        public void MutationParsesLiteralWithNegativeOffset()
        {
            var result = Builder.ParseLiteral<string, string>(
                "2019-10-02T12:14:18-01:30", x => x + Duration.FromMinutes(3));
            Assert.Equal("2019-10-02T12:17:18-01:30", result.Data.Test);
        }

        [Fact]
        public void MutationParsesLiteralWithNegativeOffsetWithoutColons()
        {
            var result = Builder.ParseLiteral<string, string>(
                "2019-10-02T12:14:18-0130", x => x + Duration.FromMinutes(3));
            Assert.Equal("2019-10-02T12:17:18-01:30", result.Data.Test);
        }

        [Fact]
        public void MutationParsesLiteralWithNegativeOffsetWithoutMinutes()
        {
            var result = Builder.ParseLiteral<string, string>(
                "2019-10-02T12:14:18-01", x => x + Duration.FromMinutes(3));
            Assert.Equal("2019-10-02T12:17:18-01", result.Data.Test);
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
                "2019-10-02T12:14:18+01:47", x => x + Duration.FromHours(3));
            Assert.Equal("2019-10-02T15:14:18+01:47", result.Data.Test);
        }

        [Fact]
        public void MutationParsesInputWithoutOffset()
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
            Assert.Equal("2019-10-02T12:17:18+01:30", result.Data.Test);
        }

        [Fact]
        public void MutationParsesInputWithOffsetWithoutColons()
        {
            var result = Builder.ParseInput<string, string>(
                "2019-10-02T12:14:18+0130", x => x + Duration.FromMinutes(3));
            Assert.Equal("2019-10-02T12:17:18+01:30", result.Data.Test);
        }

        [Fact]
        public void MutationParsesInputWithOffsetWithoutMinutes()
        {
            var result = Builder.ParseInput<string, string>(
                "2019-10-02T12:14:18+01", x => x + Duration.FromMinutes(3));
            Assert.Equal("2019-10-02T12:17:18+01", result.Data.Test);
        }

        [Fact]
        public void MutationParsesInputWithNegativeOffset()
        {
            var result = Builder.ParseInput<string, string>(
                "2019-10-02T12:14:18-01:30", x => x + Duration.FromMinutes(3));
            Assert.Equal("2019-10-02T12:17:18-01:30", result.Data.Test);
        }

        [Fact]
        public void MutationParsesInputWithNegativeOffsetWithoutColons()
        {
            var result = Builder.ParseInput<string, string>(
                "2019-10-02T12:14:18-0130", x => x + Duration.FromMinutes(3));
            Assert.Equal("2019-10-02T12:17:18-01:30", result.Data.Test);
        }

        [Fact]
        public void MutationParsesInputWithNegativeOffsetWithoutMinutes()
        {
            var result = Builder.ParseInput<string, string>(
                "2019-10-02T12:14:18-01", x => x + Duration.FromMinutes(3));
            Assert.Equal("2019-10-02T12:17:18-01", result.Data.Test);
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
