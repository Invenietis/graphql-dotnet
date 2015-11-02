using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphQL.Types
{
    public abstract class GraphType
    {
        private readonly List<FieldType> _fields = new List<FieldType>();

        public string Name { get; set; }

        public string Description { get; set; }

        public IEnumerable<FieldType> Fields
        {
            get { return _fields; }
            private set
            {
                _fields.Clear();
                _fields.AddRange( value );
            }
        }

        public void Field<TType>(
            string name,
            string description = null,
            QueryArguments arguments = null,
            Func<ResolveFieldContext, object> resolve = null )
            where TType : GraphType
        {
            if( _fields.Exists( x => x.Name == name ) )
            {
                throw new ArgumentOutOfRangeException( "name", "A field with that name is already registered." );
            }

            _fields.Add( new FieldType
            {
                Name = name,
                Type = typeof( TType ),
                Arguments = arguments,
                Resolve = resolve
            } );
        }

        public void Mutation<TInput, TPayload>( string name, Func<TInput, ResolveFieldContext, TPayload> resolve )
            where TInput : GraphType
            where TPayload : class
        {
            var arguments = new QueryArguments(new[]
            {
                new QueryArgument<NonNullGraphType<StringGraphType>>
                {
                    Name = "input",
                    Description = "input"
                }
            });

            Field<TInput>( name, null, arguments, ctx => resolve( ctx.Arguments.First().Value as TInput, ctx ) );
        }

        public void Mutation<TInput, TPayload, THandler>( string name ) 
            where TInput : GraphType
            where TPayload : class
            where THandler : Handlers.MutationHandler<TInput, TPayload>
        {
            Mutation<TInput, TPayload>( name, ( input, ctx ) =>
           {
               THandler handler = Handlers.HandlerBuilder.Current.Factory.CreateHandler<TInput, TPayload, THandler>( ctx );
               return handler.Handle( input );
           } );
        }


        public virtual string CollectTypes( TypeCollectionContext context )
        {
            return Name;
        }
    }

    /// <summary>
    /// This sucks, find a better way
    /// </summary>
    public class TypeCollectionContext
    {
        public TypeCollectionContext(
            Func<Type, GraphType> resolver,
            Action<string, GraphType> addType )
        {
            ResolveType = resolver;
            AddType = addType;
        }

        public Func<Type, GraphType> ResolveType { get; private set; }
        public Action<string, GraphType> AddType { get; private set; }
    }
}
