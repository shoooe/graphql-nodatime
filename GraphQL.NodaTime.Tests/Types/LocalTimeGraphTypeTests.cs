using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NodaTime;
using Xunit;

namespace GraphQL.NodaTime.Tests
{
    using Builder = TestBuilder<LocalTimeGraphType, LocalTime>;

    public class LocalTimeGraphTypeTests
    {
        [Fact]
        public void QueryReturnsSerializedDataWithDecimals()
        {
            var result = Builder.Serialize<string>(
                LocalTime.FromHourMinuteSecondMillisecondTick(7, 53, 10, 9, 19));
            Assert.Equal("07:53:10.0090019", result.Data.Test);
        }

        [Fact]
        public void QueryReturnsSerializedDataWithoutDecimals()
        {
            var result = Builder.Serialize<string>(
                LocalTime.FromHourMinuteSecondMillisecondTick(7, 53, 10, 0, 0));
            Assert.Equal("07:53:10", result.Data.Test);
        }

        [Fact]
        public void QueryReturnsSerializedDataWithoutDecimalsAndZeroSeconds()
        {
            var result = Builder.Serialize<string>(
                LocalTime.FromHourMinuteSecondMillisecondTick(7, 53, 0, 0, 0));
            Assert.Equal("07:53:00", result.Data.Test);
        }

        [Fact]
        public void MutationParsesLiteralWithDecimals()
        {
            var result = Builder.ParseLiteral<string, string>("09:34:01.2340087", x => x + Period.FromMinutes(3));
            Assert.Equal("09:37:01.2340087", result.Data.Test);
        }

        [Fact]
        public void MutationParsesLiteralWithoutDecimals()
        {
            var result = Builder.ParseLiteral<string, string>("09:34:01", x => x + Period.FromMinutes(3));
            Assert.Equal("09:37:01", result.Data.Test);
        }

        [Fact]
        public void MutationDoesntParseMalformedLiteral()
        {
            var result = Builder.ParseLiteral<string, string>("malformed", x => x + Period.FromMinutes(3));
            Assert.Null(result.Data);
            Assert.NotEmpty(result.Errors);
        }

        [Fact]
        public void MutationDoesntParseLiteralWithoutSeconds()
        {
            var result = Builder.ParseLiteral<string, string>("09:34", x => x + Period.FromMinutes(3));
            Assert.Null(result.Data);
            Assert.NotEmpty(result.Errors);
        }

        [Fact]
        public void MutationParsesInputWithDecimals()
        {
            var result = Builder.ParseInput<string, string>("13:04:43.0010876", x => x + Period.FromMinutes(3));
            Assert.Equal("13:07:43.0010876", result.Data.Test);
        }

        [Fact]
        public void MutationParsesInputWithoutDecimals()
        {
            var result = Builder.ParseInput<string, string>("13:04:43", x => x + Period.FromMinutes(3));
            Assert.Equal("13:07:43", result.Data.Test);
        }

        [Fact]
        public void MutationDoesntParseMalformedInput()
        {
            var result = Builder.ParseInput<string, string>("malformed", x => x + Period.FromMinutes(3));
            Assert.Null(result.Data);
            Assert.NotEmpty(result.Errors);
        }

        [Fact]
        public void MutationDoesntParseInputWithoutSeconds()
        {
            var result = Builder.ParseInput<string, string>("09:34", x => x + Period.FromMinutes(3));
            Assert.Null(result.Data);
            Assert.NotEmpty(result.Errors);
        }
    }
}
