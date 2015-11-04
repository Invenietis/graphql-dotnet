using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Types;

namespace GraphQL.Relay.Types
{

    public abstract class ConnectionType<TType> : ObjectGraphType where TType : GraphType
    {
        public ConnectionType( string name )
        {
            //connectionType( String name, GraphQLObjectType edgeType, List < GraphQLFieldDefinition > connectionFields )
            Name = name + "Connection";
            Description = "A connection to a list of items.";
            Field<ListGraphType<TType>>( "edges" );
            Field<NonNullGraphType<PageInfoType>>( "pageInfo" );
        }

        public override string CollectTypes( TypeCollectionContext context )
        {
            context.AddType( "PageInfoType", context.ResolveType( typeof( PageInfoType ) ) );
            return base.CollectTypes( context );
        }
    }

}
