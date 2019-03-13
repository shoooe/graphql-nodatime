using System;
using System.Globalization;
using GraphQL.Language.AST;
using GraphQL.Types;
using NodaTime;
using NodaTime.Text;

namespace GraphQL.NodaTime
{
    public class DurationGraphType : ScalarGraphType
    {
        public DurationGraphType()
        {
            Name = "Duration";
            Description = "Represents a fixed (and calendar-independent) length of time.";
        }

        public override object Serialize(object value)
        {
            if (value is string)
                return value;
            if (value is Duration duration)
                return DurationPattern.Roundtrip
                    .WithCulture(CultureInfo.InvariantCulture)
                    .Format(duration);
            return null;
        }

        public override object ParseValue(object value)
        {
            if (!(value is string stringValue))
                return null;

            try
            {
                return DurationPattern.Roundtrip
                    .WithCulture(CultureInfo.InvariantCulture)
                    .Parse(stringValue).GetValueOrThrow();
            } 
            catch (Exception) { return null; }
        }

        public override object ParseLiteral(IValue value)
        {
            if (!(value is StringValue stringValue))
                return null;

            try
            {
                return DurationPattern.Roundtrip
                    .WithCulture(CultureInfo.InvariantCulture)
                    .Parse(stringValue.Value).GetValueOrThrow();
            } 
            catch (Exception) { return null; }
        }
    }
}