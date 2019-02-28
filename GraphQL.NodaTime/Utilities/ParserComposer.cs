using System;
using System.Collections.Generic;
using System.Linq;
using NodaTime.Text;

namespace GraphQL.NodaTime
{
    public static class ParserComposer
    {
        public static object FirstNonThrowing<T>(IEnumerable<Func<string, T>> parsers, string value)
        {
            if (parsers.Count() == 0)
                return null;

            foreach (var nextParser in parsers)
            {
                try
                {
                    return nextParser(value);
                }
                catch (Exception) { continue; }
            }

            return null;
        }
    }
}