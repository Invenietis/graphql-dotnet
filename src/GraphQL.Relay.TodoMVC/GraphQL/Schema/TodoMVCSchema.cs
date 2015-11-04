using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Types;
using GraphQL.Types.Relay;

namespace GraphQL.Relay.TodoMVC
{
    public class TodoMVCSchema : Schema
    {
        public TodoMVCSchema()
        {
            Query = new TodoMVCQueries();
            Mutation = new TodoMVCSchemaMutations();
        }
    }

    public sealed class TodoInterface : NodeInterface
    {
        public TodoInterface() : base( ( obj ) =>
        {
            if( obj is UserType ) return new UserType();
            if( obj is TodoType ) return new TodoType();
            return null;
        } )
        {
        }
    }

    public class TodoMVCQueries : ObjectGraphType
    {
        public TodoMVCQueries()
        {
            Name = "QueryRoot";
            Field<UserType>( "viewer", resolve: ctx =>
            {
                return DB.User.Current;
            } );
            Field<TodoInterface>( "node", "Fetches an object given its ID", arguments: new QueryArguments( new[]
            {
                new QueryArgument<NonNullGraphType<IdGraphType>>
                {
                    Name = "id",
                    Description = "The ID of an object"
                }
            } ),
            resolve: ctx =>
            {
                return DB.User.Current.Todos;
            } );
        }
    }

    public class UserType : ObjectGraphType
    {
        public UserType()
        {
            Name = "User";
            Field<NonNullGraphType<IdGraphType>>( "id" );
            Field<NonNullGraphType<StringGraphType>>( "userName" );
            Field<ListGraphType<ConnectionTodoType>>( "todos",
                arguments: new QueryArguments( Relay.GetConnectionFieldArguments() ),
                resolve: ctx =>
                {
                    //TODO: Connection
                    var user = ctx.Cast<DB.User>().Source;
                    return user.Todos;

                } );
            Interface<TodoInterface>();
        }
    }

    public class TodoType : ObjectGraphType
    {
        public TodoType() 
        {
            Name = "Todo";
            Field<NonNullGraphType<IdGraphType>>( "id" );
            Field<NonNullGraphType<StringGraphType>>( "text" );
            Field<NonNullGraphType<BooleanGraphType>>( "complete" );

            Interface<TodoInterface>();
        }

    }

    public class ConnectionTodoType : ConnectionType<EdgeType<TodoType>>
    {
        public ConnectionTodoType() : base( "Todo" )
        {
            Field<IntGraphType>( "totalCount", resolve: ctx =>
            {
                var connection = (Connection) ctx.Source;
                return connection.Edges.Count;
            } );
            Field<IntGraphType>( "completedCount", resolve: ctx =>
            {
                var connection = (Connection) ctx.Source;
                return connection.Edges.Count( x => ((DB.Todo)x.Node).Completed );
            } );
        }
    }
}
