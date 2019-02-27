using System;
using System.Globalization;
using GraphQL.Language.AST;
using GraphQL.Types;
using NodaTime;
using NodaTime.Text;

namespace GraphQL.NodaTime
{
    public class LocalTimeGraphType : ScalarGraphType
    {
        public LocalTimeGraphType()
        {
            Name = "LocalTime";
        }

        public override object Serialize(object value)
        {
            if (value is string)
                return value;
            if (value is LocalTime)
                return (value as LocalTime?)?.ToString("r", CultureInfo.InvariantCulture);
            return null;
        }

        public override object ParseValue(object value)
        {
            try
            {
                var ret = LocalTimePattern.ExtendedIso.Parse(value as string).GetValueOrThrow();
                return ret;
            } 
            catch (Exception) { return null; }
        }

        public override object ParseLiteral(IValue value)
        {
            try
            {
                if (!(value is StringValue))
                    return null;
                var stringVal = value as StringValue;
                var ret = LocalTimePattern.ExtendedIso.Parse(stringVal.Value).GetValueOrThrow();
                return ret;
            } 
            catch (Exception) { return null; }
        }
    }
}