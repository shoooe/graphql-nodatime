using System;
using System.Globalization;
using GraphQL.Language.AST;
using GraphQL.Types;
using NodaTime;
using NodaTime.Text;

namespace GraphQL.NodaTime
{
    public class LocalDateTimeGraphType : ScalarGraphType
    {
        public LocalDateTimeGraphType()
        {
            Name = "LocalDateTime";
            Description = "A date and time in a particular calendar system.";
        }

        public override object Serialize(object value)
        {
            if (value is string)
                return value;
            if (value is LocalDateTime localDateTime)
                return LocalDateTimePattern.ExtendedIso
                    .WithCulture(CultureInfo.InvariantCulture)
                    .Format(localDateTime);
            return null;
        }

        private static object FromString(string stringValue)
        {
            return ParserComposer.FirstNonThrowing(new Func<string, LocalDateTime>[]
            {
                str => LocalDateTimePattern.ExtendedIso
                    .WithCulture(CultureInfo.InvariantCulture)
                    .Parse(str).GetValueOrThrow(),
            }, stringValue);
        }

        private static object FromDateTime(DateTime dateTime)
        {
            try { return LocalDateTime.FromDateTime(dateTime); }
            catch (Exception) { return null; }
        }

        public override object ParseValue(object value)
        {
            if (value is string stringValue)
            {
                return FromString(stringValue);
            }

            if (value is DateTime dateTimeValue)
            {
                return FromDateTime(dateTimeValue);
            }

            return null;
        }

        public override object ParseLiteral(IValue value)
        {
            if (value is StringValue stringValue)
            {
                return FromString(stringValue.Value);
            }

            if (value is DateTimeValue dateTimeValue)
            {
                return FromDateTime(dateTimeValue.Value);
            }

            return null;
        }
    }
}