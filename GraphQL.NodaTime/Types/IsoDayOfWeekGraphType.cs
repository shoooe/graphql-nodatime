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
        }

        public override object Serialize(object value)
        {
            if (value is int)
                return value;
            if (value is IsoDayOfWeek)
                return (int)value;
            return value;
        }

        public override object ParseValue(object value)
        {
            if (!(value is int intValue))
                throw new FormatException();
            return (IsoDayOfWeek)intValue;
        }

        public override object ParseLiteral(IValue value)
        {
            if (!(value is IntValue intValue))
                throw new FormatException();
            return (IsoDayOfWeek)intValue.Value;
        }
    }
}