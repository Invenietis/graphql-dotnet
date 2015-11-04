using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Types;

namespace GraphQL.Types.Relay
{

    public abstract class NodeInterface : InterfaceGraphType
    {
        public NodeInterface( Func<object, ObjectGraphType> resolver )
        {
            Name = "Node";
            Description = "An object with an ID";
            Field<NonNullGraphType<IdGraphType>>( "id" );
            ResolveType = resolver;
        }
    }
}
