using System;
using GraphQL.Types;

namespace GraphQL.NodaTime.Tests
{
    public static class SchemaBuilder<GraphType, UnderlyingType>
        where GraphType : GraphQL.Types.GraphType
    {
        public class QueryGraphType : ObjectGraphType
        {
            public QueryGraphType(UnderlyingType queried)
            {
                Name = "Query";

                Field<GraphType>()
                    .Name("test")
                    .Resolve(x => queried);
            }
        }

        public class MutationGraphType : ObjectGraphType
        {
            public MutationGraphType(Func<UnderlyingType, UnderlyingType> mutation)
            {
                Name = "Mutation";

                Field<GraphType>()
                    .Name("test")
                    .Argument<GraphType>("arg", "")
                    .Resolve(context => 
                    {
                        var arg = context.GetArgument<UnderlyingType>("arg");
                        return mutation(arg);
                    });
            }
        }

        public class SchemaGraphType : GraphQL.Types.Schema
        {
            public SchemaGraphType(UnderlyingType queried, Func<UnderlyingType, UnderlyingType> mutation)
            {
                Query = new QueryGraphType(queried);
                Mutation = new MutationGraphType(mutation);
            }
        }

        public static ISchema Build(UnderlyingType queried, Func<UnderlyingType, UnderlyingType> mutation)
        {
            return new SchemaGraphType(queried, mutation);
        }
    }
}