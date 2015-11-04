//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using GraphQL.Types;

//namespace GraphQL.Relay.Types
//{
//    public class NodeType<TInterface> : ObjectGraphType where TInterface : InterfaceGraphType
//    {
//        public NodeType( Func<ResolveFieldContext, object> nodeDataFetcher )
//        {
//            Name = "Node";
//            Field<TInterface>(
//                "node",
//                "Fetches an object given its ID",
//                arguments: new QueryArguments( new[]
//                {
//                        new QueryArgument<NonNullGraphType<IdGraphType>>
//                        {
//                            Name = "id",
//                            Description = "The ID of an object"
//                        }
//                } ),
//                resolve: nodeDataFetcher );
//        }
//    }
//}
