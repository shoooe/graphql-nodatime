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
            Description = "Represents a date within the calendar, with no reference to a particular time zone or time of day.";
        }

        public override object Serialize(object value)
        {
            if (value is string)
                return value;
            if (value is LocalDate)
                return (value as LocalDate?)?.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            return value;
        }

        public override object ParseValue(object value)
        {
            if (!(value is string stringValue))
                throw new FormatException();

            try
            {
                var ret = LocalDatePattern.Iso.Parse(stringValue).GetValueOrThrow();
                return ret;
            } 
            catch (Exception e) { throw new FormatException(null, e); }
        }

        public override object ParseLiteral(IValue value)
        {
            if (!(value is StringValue stringValue))
                throw new FormatException();

            try
            {
                var ret = LocalDatePattern.Iso.Parse(stringValue.Value).GetValueOrThrow();
                return ret;
            } 
            catch (Exception e) { throw new FormatException(null, e); }
        }
    }
}