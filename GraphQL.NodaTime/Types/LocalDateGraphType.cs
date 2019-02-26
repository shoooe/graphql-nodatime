using System;
using System.Globalization;
using GraphQL.Language.AST;
using GraphQL.Types;
using NodaTime;
using NodaTime.Text;

namespace GraphQL.NodaTime
{
    public class LocalDateGraphType : ScalarGraphType
    {
        public LocalDateGraphType()
        {
            Name = "LocalDate";
        }

        public override object Serialize(object value)
        {
            if (value is string)
                return value;
            if (value is LocalDate)
                return (value as LocalDate?)?.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            return null;
        }

        public override object ParseValue(object value)
        {
            try
            {
                var ret = LocalDatePattern.Iso.Parse(value as string).GetValueOrThrow();
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
                var ret = LocalDatePattern.Iso.Parse(stringVal.Value).GetValueOrThrow();
                return ret;
            } 
            catch (Exception) { return null; }
        }
    }
}