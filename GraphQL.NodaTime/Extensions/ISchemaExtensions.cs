using GraphQL.Types;

namespace GraphQL.NodaTime
{
    public static class ISchemaExtensions
    {
        public static ISchema UseNodaTime(this ISchema schema)
        {
            schema.RegisterTypes(new []
            { 
                typeof(InstantGraphType), 
                typeof(IsoDayOfWeekGraphType),
                typeof(LocalDateGraphType),
                typeof(LocalTimeGraphType),
            });
            return schema;
        }
    }
}
