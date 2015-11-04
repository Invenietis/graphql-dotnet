using Newtonsoft.Json;

namespace GraphQL.Types
{
    public class OutputGraphType<TInput> : ObjectGraphType where TInput : InputObjectGraphType
    {
        public OutputGraphType()
        {
            Field<NonNullGraphType<StringGraphType>>( "clientMutationId" );
        }

        public override string CollectTypes( TypeCollectionContext context )
        {
            context.AddType( typeof( TInput ).Name, context.ResolveType( typeof( TInput ) ) );
            return base.CollectTypes( context );
        }
    }

}
