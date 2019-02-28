using System;
using GraphQL.Types;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NodaTime;

namespace GraphQL.NodaTime.Tests
{
    public class Input<InputType>
    {
        public InputType Arg { get; set; } 
    }

    public static class TestBuilder<GraphType, UnderlyingType>
        where GraphType : GraphQL.Types.GraphType
    {
        public static QueryResult<ResultType> Serialize<ResultType>(UnderlyingType value)
        {
            var schema = SchemaBuilder<GraphType, UnderlyingType>.Build(value, x => x);
            var json = schema.Execute(options =>
            {
                options.Query = "query { test }";
                options.Schema = schema;
            });
            return JsonConvert.DeserializeObject<QueryResult<ResultType>>(json);
        }

        public static QueryResult<ResultType> ParseLiteral<LiteralType, ResultType>(LiteralType literal, Func<UnderlyingType, UnderlyingType> transform)
        {
            var schema = SchemaBuilder<GraphType, UnderlyingType>.Build(
                default(UnderlyingType),
                instant => transform(instant)
            );
            var jsonLiteral = JsonConvert.SerializeObject(literal);
            var json = schema.Execute(options =>
            {
                options.Query = $"mutation {{ test(arg: {jsonLiteral}) }}";
                options.Schema = schema;
            });
            return JsonConvert.DeserializeObject<QueryResult<ResultType>>(json);
        }

        public static QueryResult<ResultType> ParseInput<InputType, ResultType>(InputType value, Func<UnderlyingType, UnderlyingType> transform)
        {
            var schema = SchemaBuilder<GraphType, UnderlyingType>.Build(
                default(UnderlyingType),
                instant => transform(instant)
            );
            var input = new Input<InputType> { Arg = value };
            var graphTypeName = schema.Mutation.GetField("test").Arguments[0].Type.GraphQLName();
            var json = schema.Execute(options =>
            {
                options.Query = $"mutation($arg: {graphTypeName}!) {{ test(arg: $arg) }}";
                options.Schema = schema;
                options.Inputs = JsonConvert.SerializeObject(
                    input,
                    new JsonSerializerSettings 
                    { 
                        ContractResolver = new CamelCasePropertyNamesContractResolver() 
                    }).ToInputs();
            });
            return JsonConvert.DeserializeObject<QueryResult<ResultType>>(json);
        }
    }
}