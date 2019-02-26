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
            return null;
        }

        public override object ParseValue(object value)
        {
            return (IsoDayOfWeek)((int)value);
        }

        public override object ParseLiteral(IValue value)
        {
            if (!(value is IntValue))
                return null;
            var intVal = value as IntValue;
            return (IsoDayOfWeek)((int)intVal.Value);
        }
    }
}