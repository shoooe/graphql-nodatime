using System;
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
        }

        public override object Serialize(object value)
        {
            if (value is string)
                return value;
            if (value is DateTime dateTime)
                return dateTime.ToString("o", System.Globalization.CultureInfo.InvariantCulture);
            if (value is Instant instant)
                return instant.ToString("g", System.Globalization.CultureInfo.InvariantCulture);
            return value;
        }

        public override object ParseValue(object value)
        {
            if (value is string stringValue)
            {
                try
                {
                    var ret = InstantPattern.ExtendedIso.Parse(stringValue).GetValueOrThrow();
                    return ret;
                } 
                catch (Exception) { return null; }
            }

            if (value is DateTime dateTimeValue)
            {
                var ret = Instant.FromDateTimeUtc(dateTimeValue);
                return ret;
            }

            return null;
        }

        public override object ParseLiteral(IValue value)
        {
            if (value is StringValue stringValue)
            {
                try
                {

                    var ret = InstantPattern.ExtendedIso.Parse(stringValue.Value).GetValueOrThrow();
                    return ret;
                }
                catch (Exception) { return null; }
            }

            if (value is DateTimeValue dateTimeValue)
            {
                try
                {

                    var ret = Instant.FromDateTimeUtc(dateTimeValue.Value);
                    return ret;
                }
                catch (Exception) { return null; }
            }

            return null;
        }
    }
}