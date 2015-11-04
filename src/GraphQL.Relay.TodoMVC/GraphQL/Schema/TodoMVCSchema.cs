using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Types;
using GraphQL.Relay.Types;

namespace GraphQL.Relay.TodoMVC
{
    public class TodoMVCSchema : Schema
    {
        public TodoMVCSchema()
        {
            Query = new TodoMVCQueries();
            Mutation = new TodoMVCSchemaMutations();
        }

        public  Func<DB.TodoMVCDbContext> DbContextFactory { get; set; }

        public DB.TodoMVCDbContext OpenDatabase()
        {
            return DbContextFactory();
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
            Field<TodoInterface>(
                "node",
                "Fetches an object given its ID",
                arguments: new QueryArguments( new[]
                {
                    new QueryArgument<NonNullGraphType<IdGraphType>>
                    {
                        Name = "id",
                        Description = "The ID of an object"
                    }
                } ),
                resolve: ctx =>
                {
                    object id = ctx.Arguments["id"];
                    return null;
                }
            );
        }
    }

    public class UserType : ObjectGraphType
    {
        public UserType()
        {
            Name = "User";

            // Eeach type that implements NodeInterface must returns an opaque GlobalID as a Base64 string.
            Field<NonNullGraphType<IdGraphType>>( "id", resolve: ctx =>
            {
                DB.User user = (DB.User) ctx.Source;
                return ResolvedGlobalId.ToGlobalId( this.Name, user.Id.ToString() );
            } );
            Field<NonNullGraphType<StringGraphType>>( "userName" );
            Field<ConnectionTodoType>( "todos",
                arguments: new QueryArguments( Relay.GetConnectionFieldArguments() ),
                resolve: ctx =>
                {
                    using( var db = ctx.Schema.As<TodoMVCSchema>().OpenDatabase() )
                    {
                        var userId = ctx.Source.As<DB.User>().Id;
                        var listConnection = new SimpleListConnection<DB.Todo>( db.Todos.Where( x => x.UserId == userId ).ToList() );

                        return listConnection.Resolve( ctx );
                    }

                } );
            Interface<TodoInterface>();
        }
    }

    public class TodoType : ObjectGraphType
    {
        public TodoType()
        {
            Name = "Todo";
            // Eeach type that implements NodeInterface must returns an opaque GlobalID as a Base64 string.
            Field<NonNullGraphType<IdGraphType>>( "id", resolve: ctx => 
            {
                DB.Todo todo = (DB.Todo) ctx.Source;
                return ResolvedGlobalId.ToGlobalId( this.Name, todo.Id.ToString() );
            } );
            Field<NonNullGraphType<StringGraphType>>( "text" );
            Field<NonNullGraphType<BooleanGraphType>>( "complete", resolve: ctx => ctx.Source.As<DB.Todo>().Completed );

            Interface<TodoInterface>();
        }

    }

    public class ConnectionTodoType : ConnectionType<EdgeType<TodoType>>
    {
        public ConnectionTodoType() : base( "Todo" )
        {
            Field<IntGraphType>( "totalCount", resolve: ctx =>
            {
                var connection = (Connection<DB.Todo>) ctx.Source;
                return connection.Edges.Count;
            } );
            Field<IntGraphType>( "completedCount", resolve: ctx =>
            {
                var connection = (Connection<DB.Todo>) ctx.Source;
                return connection.Edges.Count( x => ((DB.Todo)x.Node).Completed );
            } );
        }
    }
}
