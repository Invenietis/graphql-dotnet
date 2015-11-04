using Newtonsoft.Json;

namespace GraphQL.Types
{
    public class InputObjectGraphType : GraphType
    {
        public InputObjectGraphType()
        {
            Field<NonNullGraphType<StringGraphType>>( "clientMutationId" );
        }

        public object Coerce(object input)
        {
            var source = JsonConvert.SerializeObject(input);
            var result = JsonConvert.DeserializeObject(source, GetType());
            return result;
        }
    }
}
