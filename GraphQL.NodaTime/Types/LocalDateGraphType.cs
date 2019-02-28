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
            if (value is LocalDate localDateValue)
                return LocalDatePattern.Iso
                    .WithCulture(CultureInfo.InvariantCulture)
                    .Format(localDateValue);
            return value;
        }

        public override object ParseValue(object value)
        {
            if (!(value is string stringValue))
                return null;

            try
            {
                var ret = LocalDatePattern.Iso
                    .WithCulture(CultureInfo.InvariantCulture)
                    .Parse(stringValue).GetValueOrThrow();
                return ret;
            } 
            catch (Exception) { return null; }
        }

        public override object ParseLiteral(IValue value)
        {
            if (!(value is StringValue stringValue))
                return null;

            try
            {
                var ret = LocalDatePattern.Iso
                    .WithCulture(CultureInfo.InvariantCulture)
                    .Parse(stringValue.Value).GetValueOrThrow();
                return ret;
            } 
            catch (Exception) { return null; }
        }
    }
}