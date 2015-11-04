using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Types;
using GraphQL.Relay.Types;

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
                    var currentUser = DB.User.Current;
                    int todoId = int.Parse( ResolvedGlobalId.FromGlobalId( input.Id ).Id);
                    var todo = db.Todos.SingleOrDefault( x => x.Id == todoId );
                    if( todo != null )
                    {
                        todo.Completed = input.Complete;
                        db.SaveChanges();
                    }
                    return new ChangeTodoStatusPayload
                    {
                        Todo = todo,
                        Viewer = todo.User,
                        ClientMutationId = input.ClientMutationId
                    };
                }
            } );
            Mutation<MarkAllTodosInput, MarkAllTodosPayload>( "markAllTodos", ( input, ctx ) =>
             {
                 return null;
             } );
            Mutation<RemoveCompletedTodosInput, RemoveCompletedTodosPayload>( "removeCompletedTodos", ( input, ctx ) =>
            {
                return null;
            } );
            Mutation<RemoveTodoInput, RemoveTodoPayload>( "removeTodo", ( input, ctx ) =>
            {
                return null;
            } );
            Mutation<RenameTodoInput, RenameTodoPayload>( "renameTodo", ( input, ctx ) =>
            {
                return null;
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
        }
    }

    public class RemoveCompletedTodosInput : InputObjectGraphType
    {
        public RemoveCompletedTodosInput()
        {
            Name = "RemoveCompletedTodosInput";
        }
    }

    public class RemoveCompletedTodosPayload : OutputGraphType<RemoveCompletedTodosInput>
    {
        public RemoveCompletedTodosPayload()
        {
            Name = "RemoveCompletedTodosPayload";
            Field<ListGraphType<StringGraphType>>( "deletedTodoIds" );
            Field<UserType>( "viewer", resolve: ctx => DB.User.Current );
        }
    }

    public class RemoveTodoInput : InputObjectGraphType
    {
        public RemoveTodoInput()
        {
            Name = "RemoveTodoInput";
            Field<NonNullGraphType<IdGraphType>>( "id" );
        }
    }

    public class RemoveTodoPayload : OutputGraphType<RemoveTodoInput>
    {
        public RemoveTodoPayload()
        {
            Name = "RemoveTodoPayload";
            Field<NonNullGraphType<IdGraphType>>( "deletedTodoId" );
            Field<UserType>( "viewer", resolve: ctx => DB.User.Current );
        }
    }

    public class RenameTodoInput : InputObjectGraphType
    {
        public RenameTodoInput()
        {
            Name = "RenameTodoInput";
            Field<NonNullGraphType<IdGraphType>>( "id" );
            Field<NonNullGraphType<StringGraphType>>( "text" );
        }
    }
    public class RenameTodoPayload : OutputGraphType<RenameTodoInput>
    {
        public RenameTodoPayload()
        {
            Name = "RenameTodoPayload";
            Field<TodoType>( "todo" );
            Field<UserType>( "viewer", resolve: ctx => DB.User.Current );
        }
    }
}
