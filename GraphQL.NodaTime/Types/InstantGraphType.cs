using System;
using System.Globalization;
using GraphQL.Language.AST;
using GraphQL.Types;
using NodaTime;
using NodaTime.Text;

namespace GraphQL.NodaTime
{
    public class InstantGraphType : ScalarGraphType
    {
        public InstantGraphType()
        {
            Name = "Instant";
            Description = "Represents an instant on the global timeline.";
        }

        public override object Serialize(object value)
        {
            if (value is string)
                return value;
            if (value is DateTime dateTime)
                return dateTime.ToString("o", CultureInfo.InvariantCulture);
            if (value is Instant instant)
                return InstantPattern.ExtendedIso
                    .WithCulture(CultureInfo.InvariantCulture)
                    .Format(instant);
            return null;
        }

        private static object FromString(string stringValue)
        {
            return ParserComposer.FirstNonThrowing(new Func<string, Instant>[]
            {
                str => OffsetDateTimePattern.ExtendedIso
                    .WithCulture(CultureInfo.InvariantCulture)
                    .Parse(str).GetValueOrThrow().ToInstant(),
                str => OffsetDateTimePattern
                    .CreateWithInvariantCulture("yyyy'-'MM'-'dd'T'HH':'mm':'sso<+HHmm>")
                    .Parse(str).GetValueOrThrow().ToInstant(),
                str => InstantPattern.ExtendedIso
                    .WithCulture(CultureInfo.InvariantCulture)
                    .Parse(str).GetValueOrThrow(),
            }, stringValue);
        }

        private static object FromDateTimeUtc(DateTime dateTime)
        {
            try { return Instant.FromDateTimeUtc(dateTime); }
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
                return FromDateTimeUtc(dateTimeValue);
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
                return FromDateTimeUtc(dateTimeValue.Value);
            }

            return null;
        }
    }
}