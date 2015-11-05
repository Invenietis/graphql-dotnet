using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Types;
using GraphQL.Relay.Types;
using Microsoft.Data.Entity;

namespace GraphQL.Relay.TodoMVC
{
    public class TodoMVCSchemaMutations : ObjectGraphType
    {
        public TodoMVCSchemaMutations()
        {
            Name = "Mutations";

            Mutation<AddTodoInput, AddTodoPayload>( "addTodo", resolve: ( input, ctx ) =>
            {
                using( var db = ctx.Schema.As<TodoMVCSchema>().OpenDatabase() )
                {
                    var currentUser = DB.User.Current;
                    var entity = db.Todos.Add( new DB.Todo
                    {
                        Text = input.Text,
                        Completed = false,
                        UserId = currentUser.Id
                    } );
                    db.SaveChanges();

                    currentUser.Todos.Add( entity.Entity );
                    return new AddTodoPayload
                    {
                        TodoEdge = new Edge<DB.Todo>( entity.Entity, new ConnectionCursor( "" ) ),
                        Viewer = currentUser,
                        ClientMutationId = input.ClientMutationId
                    };
                }
            } );

            Mutation<ChangeTodoStatusInput, ChangeTodoStatusPayload>( "changeTodoStatus", ( input, ctx ) =>
            {
                using( var db = ctx.Schema.As<TodoMVCSchema>().OpenDatabase() )
                {
                    int todoId;
                    if( int.TryParse( ResolvedGlobalId.FromGlobalId( input.Id ).Id, out todoId ) )
                    {
                        var currentUser = DB.User.Current;
                        var result = new ChangeTodoStatusPayload
                        {
                            ClientMutationId = input.ClientMutationId,
                            Viewer = db.Users.Include( x => x.Todos ).SingleOrDefault( u => u.Id == DB.User.Current.Id )
                        };

                        result.Todo = result.Viewer.Todos.SingleOrDefault( x => x.Id == todoId );
                        if( result.Todo != null )
                        {
                            result.Todo.Completed = input.Complete;
                            db.SaveChanges();
                        }
                        return result;
                    }
                    throw new ArgumentException( "Todo not found." );
                }
            } );
            Mutation<MarkAllTodosInput, MarkAllTodosPayload>( "markAllTodos", ( input, ctx ) =>
             {
                 using( var db = ctx.Schema.As<TodoMVCSchema>().OpenDatabase() )
                 {
                     var result = new MarkAllTodosPayload
                     {
                         ClientMutationId = input.ClientMutationId
                     };
                     result.Viewer = db.Users.Include( x => x.Todos ).SingleOrDefault( u => u.Id == DB.User.Current.Id );

                     // TODO: uses update from XX output clause
                     foreach( var todo in result.Viewer.Todos )
                     {
                         if( todo.Completed != input.Complete )
                         {
                             todo.Completed = input.Complete;
                             result.ChangedTodos.Add( todo );
                         }
                     }
                     db.SaveChanges();
                     return result;
                 }
             } );
            Mutation<RemoveCompletedTodosInput, RemoveCompletedTodosPayload>( "removeCompletedTodos", ( input, ctx ) =>
            {
                using( var db = ctx.Schema.As<TodoMVCSchema>().OpenDatabase() )
                {
                    var result = new RemoveCompletedTodosPayload
                    {
                        ClientMutationId = input.ClientMutationId
                    };
                    result.Viewer = db.Users.Include( x => x.Todos ).SingleOrDefault( u => u.Id == DB.User.Current.Id );

                    // TODO: uses delete from XX output clause
                    result.DeletedTodoIds = result.Viewer.Todos.Where( t => t.Completed ).Select( t => ResolvedGlobalId.ToGlobalId( "Todo", t.Id.ToString() ) ).ToList();
                    db.RemoveRange( result.Viewer.Todos.Where( t => t.Completed ) );
                    db.SaveChanges();
                    return result;
                }
            } );
            Mutation<RemoveTodoInput, RemoveTodoPayload>( "removeTodo", ( input, ctx ) =>
            {
                var gId = ResolvedGlobalId.FromGlobalId(input.Id);
                int todoId;
                if( Int32.TryParse( gId.Id, out todoId ) )
                {
                    using( var db = ctx.Schema.As<TodoMVCSchema>().OpenDatabase() )
                    {
                        var result = new RemoveTodoPayload
                        {
                            ClientMutationId = input.ClientMutationId
                        };
                        result.Viewer = db.Users.Include( x => x.Todos ).SingleOrDefault( u => u.Id == DB.User.Current.Id );
                        var todo = result.Viewer.Todos.SingleOrDefault( t => t.Id == todoId );
                        if( todo != null )
                        {
                            db.Remove( todo );
                            db.SaveChanges();
                            result.DeletedTodoId = ResolvedGlobalId.ToGlobalId( gId.Type, todoId.ToString() );
                            return result;
                        }
                    }
                }
                throw new ArgumentException( "Todo not found for current viewer" );
            } );
            Mutation<RenameTodoInput, RenameTodoPayload>( "renameTodo", ( input, ctx ) =>
            {
                var gId = ResolvedGlobalId.FromGlobalId(input.Id);
                int todoId;
                if( int.TryParse( gId.Id, out todoId ) )
                {
                    using( var db = ctx.Schema.As<TodoMVCSchema>().OpenDatabase() )
                    {
                        var result = new RenameTodoPayload
                        {
                            ClientMutationId = input.ClientMutationId
                        };
                        result.Viewer = db.Users.Include( x => x.Todos ).SingleOrDefault( u => u.Id == DB.User.Current.Id );
                        result.Todo = result.Viewer.Todos.SingleOrDefault( t => t.Id == todoId );
                        if( result.Todo != null )
                        {
                            result.Todo.Text = input.Text;
                            db.SaveChanges();
                        }
                        return result;
                    }
                }
                throw new ArgumentException( "Todo not found for current viewer" );
            } );
        }
    }

    public class AddTodoInput : InputObjectGraphType
    {
        public AddTodoInput()
        {
            Name = "AddTodoInput";
            Field<NonNullGraphType<StringGraphType>>( "text" );
        }

        public string Text { get; set; }

        public string ClientMutationId { get; set; }
    }

    public class AddTodoPayload : OutputGraphType<AddTodoInput>
    {
        public AddTodoPayload()
        {
            Name = "AddTodoPayload";

            Field<UserType>( "viewer" );
            Field<EdgeType<TodoType>>( "todoEdge" );
        }

        public DB.User Viewer { get; set; }

        public Edge<DB.Todo> TodoEdge { get; set; }
        public string ClientMutationId { get; internal set; }
    }

    public class ChangeTodoStatusInput : InputObjectGraphType
    {
        public ChangeTodoStatusInput()
        {
            Name = "ChangeTodoStatusInput";
            Field<NonNullGraphType<IdGraphType>>( "id" );
            Field<NonNullGraphType<BooleanGraphType>>( "complete" );
        }

        public string Id { get; set; }

        public bool Complete { get; set; }
        public string ClientMutationId { get; set; }
    }

    public class ChangeTodoStatusPayload : OutputGraphType<ChangeTodoStatusInput>
    {
        public ChangeTodoStatusPayload()
        {
            Name = "ChangeTodoStatusPayload";
            Field<TodoType>( "todo" );
            Field<UserType>( "viewer" );
        }

        public DB.User Viewer { get; set; }

        public DB.Todo Todo { get; set; }
        public string ClientMutationId { get; set; }
    }


    public class MarkAllTodosInput : InputObjectGraphType
    {
        public MarkAllTodosInput()
        {
            Name = "MarkAllTodosInput";
            Field<NonNullGraphType<BooleanGraphType>>( "complete" );
        }

        public string ClientMutationId { get; set; }
        public bool Complete { get; set; }

    }
    public class MarkAllTodosPayload : OutputGraphType<MarkAllTodosInput>
    {
        public MarkAllTodosPayload()
        {
            Name = "MarkAllTodosPayload";
            Field<ListGraphType<TodoType>>( "changedTodos", resolve: ctx =>
            {
                //Map source = (Map) environment.getSource();
                //return todoSchema.getTodos( (List<String>)source.get( "todIds" ) );
                return null;
            } );
            Field<UserType>( "viewer", resolve: ctx => DB.User.Current );

            ChangedTodos = new List<DB.Todo>();
        }

        public IList<DB.Todo> ChangedTodos { get; set; }
        public DB.User Viewer { get; set; }
        public string ClientMutationId { get; set; }
    }

    public class RemoveCompletedTodosInput : InputObjectGraphType
    {
        public RemoveCompletedTodosInput()
        {
            Name = "RemoveCompletedTodosInput";
        }
        public string ClientMutationId { get; set; }
    }

    public class RemoveCompletedTodosPayload : OutputGraphType<RemoveCompletedTodosInput>
    {
        public RemoveCompletedTodosPayload()
        {
            Name = "RemoveCompletedTodosPayload";
            Field<ListGraphType<StringGraphType>>( "deletedTodoIds" );
            Field<UserType>( "viewer", resolve: ctx => DB.User.Current );
        }
        public IList<string> DeletedTodoIds { get; set; }
        public DB.User Viewer { get; set; }
        public string ClientMutationId { get; set; }
    }

    public class RemoveTodoInput : InputObjectGraphType
    {
        public RemoveTodoInput()
        {
            Name = "RemoveTodoInput";
            Field<NonNullGraphType<IdGraphType>>( "id" );
        }

        public string Id { get; set; }
        public string ClientMutationId { get; set; }
    }

    public class RemoveTodoPayload : OutputGraphType<RemoveTodoInput>
    {
        public RemoveTodoPayload()
        {
            Name = "RemoveTodoPayload";
            Field<NonNullGraphType<IdGraphType>>( "deletedTodoId" );
            Field<UserType>( "viewer", resolve: ctx => DB.User.Current );
        }

        public string DeletedTodoId { get; set; }

        public DB.User Viewer { get; set; }
        public string ClientMutationId { get; set; }
    }

    public class RenameTodoInput : InputObjectGraphType
    {
        public RenameTodoInput()
        {
            Name = "RenameTodoInput";
            Field<NonNullGraphType<IdGraphType>>( "id" );
            Field<NonNullGraphType<StringGraphType>>( "text" );
        }

        public string Id { get; set; }

        public string Text { get; set; }
        public string ClientMutationId { get; set; }
    }
    public class RenameTodoPayload : OutputGraphType<RenameTodoInput>
    {
        public RenameTodoPayload()
        {
            Name = "RenameTodoPayload";
            Field<TodoType>( "todo" );
            Field<UserType>( "viewer", resolve: ctx => DB.User.Current );
        }

        public DB.Todo Todo { get; set; }

        public DB.User Viewer { get; set; }
        public string ClientMutationId { get; set; }
    }
}
