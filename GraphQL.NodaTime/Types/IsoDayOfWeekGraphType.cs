using System;
using GraphQL.Language.AST;
using GraphQL.Types;
using NodaTime;
using NodaTime.Text;

namespace GraphQL.NodaTime
{
    public class IsoDayOfWeekGraphType : ScalarGraphType
    {
        public IsoDayOfWeekGraphType()
        {
            Name = "IsoDayOfWeek";
            Description = "Represents a day of the week according to ISO-8601 (Monday = 1, Sunday = 7).";
        }

        public override object Serialize(object value)
        {
            if (value is int intValue && intValue >= 1 && intValue <= 7)
                return value;
            if (value is IsoDayOfWeek dowValue)
                return (int)dowValue;
            return null;
        }

        public override object ParseValue(object value)
        {
            if (!(value is int intValue && intValue >= 1 && intValue <= 7))
                return null;
            return (IsoDayOfWeek)intValue;
        }

        public override object ParseLiteral(IValue value)
        {
            if (!(value is IntValue intValue && intValue.Value >= 1 && intValue.Value <= 7))
                return null;
            return (IsoDayOfWeek)intValue.Value;
        }
    }
}