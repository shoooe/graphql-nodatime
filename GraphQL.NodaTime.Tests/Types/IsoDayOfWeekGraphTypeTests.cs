using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NodaTime;
using Xunit;

namespace GraphQL.NodaTime.Tests
{
    using Builder = TestBuilder<IsoDayOfWeekGraphType, IsoDayOfWeek>;
    
    public class IsoDayOfWeekGraphTypeTests
    {
        private static IsoDayOfWeek Increment(IsoDayOfWeek day)
        {
            var intRepr = (int)day;
            var nextIntRepr = Math.Max(1, (intRepr + 1) % 8);
            return (IsoDayOfWeek)nextIntRepr;
        }

        [Fact]
        public void QueryReturnsMonday()
        {
            var result = Builder.Serialize<int>(IsoDayOfWeek.Monday);
            Assert.Equal(1, result.Data.Test);
        }

        [Fact]
        public void QueryReturnsSunday()
        {
            var result = Builder.Serialize<int>(IsoDayOfWeek.Sunday);
            Assert.Equal(7, result.Data.Test);
        }

        [Fact]
        public void QueryReturnsFriday()
        {
            var result = Builder.Serialize<int>(IsoDayOfWeek.Friday);
            Assert.Equal(5, result.Data.Test);
        }

        [Fact]
        public void MutationParsesLiteral()
        {
            var result = Builder.ParseLiteral<int, int>(3, Increment);
            Assert.Equal(4, result.Data.Test);
        }

        [Fact]
        public void MutationDoesntParseZeroLiteral()
        {
            var result = Builder.ParseLiteral<int, int>(0, Increment);
            Assert.Null(result.Data);
            Assert.NotEmpty(result.Errors);
        }

        [Fact]
        public void MutationDoesntParseNegativeLiteral()
        {
            var result = Builder.ParseLiteral<int, int>(-3, Increment);
            Assert.Null(result.Data);
            Assert.NotEmpty(result.Errors);
        }

        [Fact]
        public void MutationDoesntParseOutOfRangeLiteral()
        {
            var result = Builder.ParseLiteral<int, int>(8, Increment);
            Assert.Null(result.Data);
            Assert.NotEmpty(result.Errors);
        }

        [Fact]
        public void MutationParsesInput()
        {
            var result = Builder.ParseInput<int, int>(7, Increment);
            Assert.Equal(1, result.Data.Test);
        }

        [Fact]
        public void MutationDoesntParseZeroInput()
        {
            var result = Builder.ParseInput<int, int>(0, Increment);
            Assert.Null(result.Data);
            Assert.NotEmpty(result.Errors);
        }

        [Fact]
        public void MutationDoesntParseNegativeInput()
        {
            var result = Builder.ParseInput<int, int>(-3, Increment);
            Assert.Null(result.Data);
            Assert.NotEmpty(result.Errors);
        }

        [Fact]
        public void MutationDoesntParseOutOfRangeInput()
        {
            var result = Builder.ParseInput<int, int>(8, Increment);
            Assert.Null(result.Data);
            Assert.NotEmpty(result.Errors);
        }
    }
}
