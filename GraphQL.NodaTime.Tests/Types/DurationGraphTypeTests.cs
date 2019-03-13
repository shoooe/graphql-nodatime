using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NodaTime;
using Xunit;

namespace GraphQL.NodaTime.Tests
{
    using Builder = TestBuilder<DurationGraphType, Duration>;

    public class DurationGraphTypeTests
    {
        [Fact]
        public void QueryReturnsSerializedDataWithDecimals()
        {
            var result = Builder.Serialize<string>(Duration.FromTimeSpan(new TimeSpan(123, 7, 53, 10, 19)));
            Assert.Equal("123:07:53:10.019", result.Data.Test);
        }

        [Fact]
        public void QueryReturnsSerializedDataWithNegativeValue()
        {
            var result = Builder.Serialize<string>(-Duration.FromTimeSpan(new TimeSpan(123, 7, 53, 10, 19)));
            Assert.Equal("-123:07:53:10.019", result.Data.Test);
        }

        [Fact]
        public void QueryReturnsSerializedDataWithoutDecimals()
        {
            var result = Builder.Serialize<string>(Duration.FromTimeSpan(new TimeSpan(123, 7, 53, 10)));
            Assert.Equal("123:07:53:10", result.Data.Test);
        }

        [Fact]
        public void QueryReturnsSerializedDataWithoutSeconds()
        {
            var result = Builder.Serialize<string>(Duration.FromTimeSpan(new TimeSpan(123, 7, 53, 0)));
            Assert.Equal("123:07:53:00", result.Data.Test);
        }

        [Fact]
        public void QueryReturnsSerializedDataWithoutMinutes()
        {
            var result = Builder.Serialize<string>(Duration.FromTimeSpan(new TimeSpan(123, 7, 0, 0)));
            Assert.Equal("123:07:00:00", result.Data.Test);
        }

        [Fact]
        public void QueryReturnsSerializedDataWithRoundtrip()
        {
            var result = Builder.Serialize<string>(Duration.FromTimeSpan(new TimeSpan(123, 26, 0, 70)));
            Assert.Equal("124:02:01:10", result.Data.Test);
        }

        [Fact]
        public void MutationParsesLiteralWithDecimals()
        {
            var result = Builder.ParseLiteral<string, string>("09:22:01:00.019", x => x + Duration.FromMinutes(10));
            Assert.Equal("9:22:11:00.019", result.Data.Test);
        }

        [Fact]
        public void MutationParsesLiteralWithoutDecimals()
        {
            var result = Builder.ParseLiteral<string, string>("09:22:01:00", x => x + Duration.FromMinutes(10));
            Assert.Equal("9:22:11:00", result.Data.Test);
        }

        [Fact]
        public void MutationParsesLiteralWithoutLeadingZero()
        {
            var result = Builder.ParseLiteral<string, string>("9:22:01:00", x => x + Duration.FromMinutes(10));
            Assert.Equal("9:22:11:00", result.Data.Test);
        }


        [Fact]
        public void MutationParsesLiteralWithNegativeValue()
        {
            var result = Builder.ParseLiteral<string, string>("-9:22:01:00", x => x - Duration.FromMinutes(10));
            Assert.Equal("-9:22:11:00", result.Data.Test);
        }

        [Fact]
        public void MutationDoesntParseLiteralWithPlusSign()
        {
            var result = Builder.ParseLiteral<string, string>("+09:22:01:00", x => x + Duration.FromMinutes(10));
            Assert.Null(result.Data);
            Assert.NotEmpty(result.Errors);
        }

        [Fact]
        public void MutationDoesntParseLiteralWithOverflownHours()
        {
            var result = Builder.ParseLiteral<string, string>("9:26:01:00", x => x + Duration.FromMinutes(10));
            Assert.Null(result.Data);
            Assert.NotEmpty(result.Errors);
        }

        [Fact]
        public void MutationParsesInputWithDecimals()
        {
            var result = Builder.ParseInput<string, string>("09:22:01:00.019", x => x + Duration.FromMinutes(10));
            Assert.Equal("9:22:11:00.019", result.Data.Test);
        }

        [Fact]
        public void MutationParsesInputWithoutDecimals()
        {
            var result = Builder.ParseInput<string, string>("09:22:01:00", x => x + Duration.FromMinutes(10));
            Assert.Equal("9:22:11:00", result.Data.Test);
        }

        [Fact]
        public void MutationParsesInputWithoutLeadingZero()
        {
            var result = Builder.ParseInput<string, string>("9:22:01:00", x => x + Duration.FromMinutes(10));
            Assert.Equal("9:22:11:00", result.Data.Test);
        }


        [Fact]
        public void MutationParsesInputWithNegativeValue()
        {
            var result = Builder.ParseInput<string, string>("-9:22:01:00", x => x - Duration.FromMinutes(10));
            Assert.Equal("-9:22:11:00", result.Data.Test);
        }

        [Fact]
        public void MutationDoesntParseInputWithPlusSign()
        {
            var result = Builder.ParseInput<string, string>("+09:22:01:00", x => x + Duration.FromMinutes(10));
            Assert.Null(result.Data);
            Assert.NotEmpty(result.Errors);
        }

        [Fact]
        public void MutationDoesntParseInputWithOverflownHours()
        {
            var result = Builder.ParseInput<string, string>("9:26:01:00", x => x + Duration.FromMinutes(10));
            Assert.Null(result.Data);
            Assert.NotEmpty(result.Errors);
        }
    }
}
