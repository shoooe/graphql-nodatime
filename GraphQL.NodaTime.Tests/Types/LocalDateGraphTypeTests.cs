using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NodaTime;
using Xunit;

namespace GraphQL.NodaTime.Tests
{
    using Builder = TestBuilder<LocalDateGraphType, LocalDate>;

    public class LocalDateGraphTypeTests
    {
        [Fact]
        public void QueryReturnsSerializedData()
        {
            var result = Builder.Serialize<string>(
                LocalDate.FromWeekYearWeekAndDay(2019, 42, IsoDayOfWeek.Friday));
            Assert.Equal("2019-10-18", result.Data.Test);
        }

        [Fact]
        public void MutationParsesLiteral()
        {
            var result = Builder.ParseLiteral<string, string>("2019-07-12", x => x + Period.FromDays(3));
            Assert.Equal("2019-07-15", result.Data.Test);
        }

        [Fact]
        public void MutationDoesntParseMalformedLiteral()
        {
            var result = Builder.ParseLiteral<string, string>("malformed", x => x + Period.FromDays(3));
            Assert.Null(result.Data);
            Assert.NotEmpty(result.Errors);
        }

        [Fact]
        public void MutationDoesntParseInstantLiteral()
        {
            var result = Builder.ParseLiteral<string, string>("2019-10-02T15:14:18Z", x => x + Period.FromDays(3));
            Assert.Null(result.Data);
            Assert.NotEmpty(result.Errors);
        }

        [Fact]
        public void MutationParsesInput()
        {
            var result = Builder.ParseInput<string, string>("2019-12-03", x => x + Period.FromDays(3));
            Assert.Equal("2019-12-06", result.Data.Test);
        }

        [Fact]
        public void MutationDoesntParseMalformedInput()
        {
            var result = Builder.ParseInput<string, string>("malformed", x => x + Period.FromDays(3));
            Assert.Null(result.Data);
            Assert.NotEmpty(result.Errors);
        }

        [Fact]
        public void MutationDoesntParseInstantInput()
        {
            var result = Builder.ParseInput<string, string>("2019-10-02T15:14:18Z", x => x + Period.FromDays(3));
            Assert.Null(result.Data);
            Assert.NotEmpty(result.Errors);
        }
    }
}
