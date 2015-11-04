using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Types;

namespace GraphQL.Relay.Types
{
    public sealed class EdgeType<TType> : ObjectGraphType where TType : GraphType
    {
        public EdgeType()
        {
            //( String name, GraphQLOutputType nodeType, GraphQLInterfaceType nodeInterface, List<GraphQLFieldDefinition> edgeFields )
            Name =   "Edge";
            Description = "An edge in a connection.";
            Field<TType>( "node", "The item at the end of the edge" );
            Field<NonNullGraphType<StringGraphType>>( "cursor" );
        }

        public override string CollectTypes( TypeCollectionContext context )
        {
            return context.ResolveType( typeof( TType ) ).Name + this.Name;
        }
    }

}
