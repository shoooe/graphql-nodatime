using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NodaTime;
using Xunit;

namespace GraphQL.NodaTime.Tests
{
    using Builder = TestBuilder<OffsetGraphType, Offset>;

    public class OffsetGraphTypeTests
    {
        [Fact]
        public void QueryReturnsSerializedDataIgnoringMilliseconds()
        {
            var hours = 5;
            var minutes = 39;
            var seconds = 14;
            var milliseconds = 89;
            var result = Builder.Serialize<string>(
                Offset.FromMilliseconds(
                    milliseconds + 
                    seconds * 1000 + 
                    minutes * 60 * 1000 +
                    hours * 60 * 60 * 1000));
            Assert.Equal("+05:39:14", result.Data.Test);
        }

        [Fact]
        public void QueryReturnsSerializedDataWithZeroSeconds()
        {
            var result = Builder.Serialize<string>(
                Offset.FromHoursAndMinutes(5, 39));
            Assert.Equal("+05:39", result.Data.Test);
        }

        [Fact]
        public void QueryReturnsSerializedDataWithZeroSecondsAndZeroMinutes()
        {
            var result = Builder.Serialize<string>(Offset.FromHours(5));
            Assert.Equal("+05", result.Data.Test);
        }

        [Fact]
        public void QueryReturnsSerializedDataWithZeroOffset()
        {
            var result = Builder.Serialize<string>(Offset.Zero);
            Assert.Equal("+00", result.Data.Test);
        }

        [Fact]
        public void QueryReturnsSerializedDataWithNegative()
        {
            var result = Builder.Serialize<string>(-Offset.FromHoursAndMinutes(5, 39));
            Assert.Equal("-05:39", result.Data.Test);
        }

        [Fact]
        public void MutationParsesLiteralWithoutDecimals()
        {
            var result = Builder.ParseLiteral<string, string>("+09:34:01", x => x + Offset.FromHoursAndMinutes(0, 10));
            Assert.Equal("+09:44:01", result.Data.Test);
        }

        [Fact]
        public void MutationParsesLiteralWithoutSeconds()
        {
            var result = Builder.ParseLiteral<string, string>("+09:34", x => x + Offset.FromHoursAndMinutes(0, 10));
            Assert.Equal("+09:44", result.Data.Test);
        }


        [Fact]
        public void MutationParsesLiteralWithoutMinutes()
        {
            var result = Builder.ParseLiteral<string, string>("+09", x => x + Offset.FromHoursAndMinutes(0, 10));
            Assert.Equal("+09:10", result.Data.Test);
        }

        [Fact]
        public void MutationParsesLiteralWithZ()
        {
            var result = Builder.ParseLiteral<string, string>("Z", x => x + Offset.FromHoursAndMinutes(0, 10));
            Assert.Equal("+00:10", result.Data.Test);
        }

        [Fact]
        public void MutationParsesLiteralWithZero()
        {
            var result = Builder.ParseLiteral<string, string>("+00:00", x => x + Offset.FromHoursAndMinutes(0, 10));
            Assert.Equal("+00:10", result.Data.Test);
        }

        [Fact]
        public void MutationParsesLiteralWithNegativeZero()
        {
            var result = Builder.ParseLiteral<string, string>("-00", x => x + Offset.FromHoursAndMinutes(0, 10));
            Assert.Equal("+00:10", result.Data.Test);
        }

        [Fact]
        public void MutationParsesLiteralWithNegativeValues()
        {
            var result = Builder.ParseLiteral<string, string>("-12:00", x => x + Offset.FromHoursAndMinutes(0, 10));
            Assert.Equal("-11:50", result.Data.Test);
        }

        [Fact]
        public void MutationParsesLiteralAtTheLeftBoundary()
        {
            var result = Builder.ParseLiteral<string, string>("-18:00", x => x + Offset.FromHoursAndMinutes(0, 10));
            Assert.Equal("-17:50", result.Data.Test);
        }

        [Fact]
        public void MutationDoesntParseLiteralOverTheLeftBoundary()
        {
            var result = Builder.ParseLiteral<string, string>("-18:01", x => x + Offset.FromHoursAndMinutes(0, 10));
            Assert.Null(result.Data);
            Assert.NotEmpty(result.Errors);
        }

        [Fact]
        public void MutationParsesLiteralAtTheRightBoundary()
        {
            var result = Builder.ParseLiteral<string, string>("+18:00", x => x - Offset.FromHoursAndMinutes(0, 10));
            Assert.Equal("+17:50", result.Data.Test);
        }

        [Fact]
        public void MutationDoesntParseLiteralOverTheRightBoundary()
        {
            var result = Builder.ParseLiteral<string, string>("+18:01", x => x - Offset.FromHoursAndMinutes(0, 10));
            Assert.Null(result.Data);
            Assert.NotEmpty(result.Errors);
        }

        [Fact]
        public void MutationDoesntParseMalformedLiteral()
        {
            var result = Builder.ParseLiteral<string, string>("malformed", x => x + Offset.FromHoursAndMinutes(0, 10));
            Assert.Null(result.Data);
            Assert.NotEmpty(result.Errors);
        }

        [Fact]
        public void MutationParsesInputWithoutDecimals()
        {
            var result = Builder.ParseInput<string, string>("+09:34:01", x => x + Offset.FromHoursAndMinutes(0, 10));
            Assert.Equal("+09:44:01", result.Data.Test);
        }

        [Fact]
        public void MutationParsesInputWithoutSeconds()
        {
            var result = Builder.ParseInput<string, string>("+09:34", x => x + Offset.FromHoursAndMinutes(0, 10));
            Assert.Equal("+09:44", result.Data.Test);
        }


        [Fact]
        public void MutationParsesInputWithoutMinutes()
        {
            var result = Builder.ParseInput<string, string>("+09", x => x + Offset.FromHoursAndMinutes(0, 10));
            Assert.Equal("+09:10", result.Data.Test);
        }

        [Fact]
        public void MutationParsesInputWithZ()
        {
            var result = Builder.ParseInput<string, string>("Z", x => x + Offset.FromHoursAndMinutes(0, 10));
            Assert.Equal("+00:10", result.Data.Test);
        }

        [Fact]
        public void MutationParsesInputWithZero()
        {
            var result = Builder.ParseInput<string, string>("+00:00", x => x + Offset.FromHoursAndMinutes(0, 10));
            Assert.Equal("+00:10", result.Data.Test);
        }

        [Fact]
        public void MutationParsesInputWithNegativeZero()
        {
            var result = Builder.ParseInput<string, string>("-00", x => x + Offset.FromHoursAndMinutes(0, 10));
            Assert.Equal("+00:10", result.Data.Test);
        }

        [Fact]
        public void MutationParsesInputWithNegativeValues()
        {
            var result = Builder.ParseInput<string, string>("-12:00", x => x + Offset.FromHoursAndMinutes(0, 10));
            Assert.Equal("-11:50", result.Data.Test);
        }

        [Fact]
        public void MutationParsesInputAtTheLeftBoundary()
        {
            var result = Builder.ParseInput<string, string>("-18:00", x => x + Offset.FromHoursAndMinutes(0, 10));
            Assert.Equal("-17:50", result.Data.Test);
        }

        [Fact]
        public void MutationDoesntParseInputOverTheLeftBoundary()
        {
            var result = Builder.ParseInput<string, string>("-18:01", x => x + Offset.FromHoursAndMinutes(0, 10));
            Assert.Null(result.Data);
            Assert.NotEmpty(result.Errors);
        }

        [Fact]
        public void MutationParsesInputAtTheRightBoundary()
        {
            var result = Builder.ParseInput<string, string>("+18:00", x => x - Offset.FromHoursAndMinutes(0, 10));
            Assert.Equal("+17:50", result.Data.Test);
        }

        [Fact]
        public void MutationDoesntParseInputOverTheRightBoundary()
        {
            var result = Builder.ParseInput<string, string>("+18:01", x => x - Offset.FromHoursAndMinutes(0, 10));
            Assert.Null(result.Data);
            Assert.NotEmpty(result.Errors);
        }

        [Fact]
        public void MutationDoesntParseMalformedInput()
        {
            var result = Builder.ParseInput<string, string>("malformed", x => x + Offset.FromHoursAndMinutes(0, 10));
            Assert.Null(result.Data);
            Assert.NotEmpty(result.Errors);
        }
    }
}
