using System;
using System.Globalization;
using GraphQL.Language.AST;
using GraphQL.Types;
using NodaTime;
using NodaTime.Text;

namespace GraphQL.NodaTime
{
    public class OffsetGraphType : ScalarGraphType
    {
        public OffsetGraphType()
        {
            Name = "Offset";
            Description = "An offset from UTC. A positive value means that the local time is ahead of UTC (e.g. for Europe); a negative value means that the local time is behind UTC (e.g. for America).";
        }


        public override object Serialize(object value)
        {
            if (value is string)
                return value;
            if (value is Offset offset)
                return OffsetPattern.GeneralInvariant
                    .WithCulture(CultureInfo.InvariantCulture)
                    .Format(offset);
            return value;
        }

        public override object ParseValue(object value)
        {
            if (!(value is string stringValue))
                return null;

            try
            {
                return OffsetPattern.GeneralInvariantWithZ
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
                var ret = OffsetPattern.GeneralInvariantWithZ
                    .WithCulture(CultureInfo.InvariantCulture)
                    .Parse(stringValue.Value).GetValueOrThrow();
                return ret;
            } 
            catch (Exception e) { return null; }
        }
    }
}