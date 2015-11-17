using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using GraphQL.Types;

namespace GraphQL.Registration
{
    public class GraphTypeDiscoverer
    {
        readonly IGraphTypeHandler[] _handlers;
        readonly List<Type> _types = new List<Type>();

        /// <summary>
        /// Creates the <see cref="DefaultGraphTypeDiscoverer"/> with the given handlers,
        /// in order: <see cref="AnnotationGraphTypeHandler"/>, then <see cref="ConventionGraphTypeHandler"/>.
        /// </summary>
        public GraphTypeDiscoverer() : this( new AnnotationGraphTypeHandler(), new ConventionGraphTypeHandler() )
        {
        }

        public GraphTypeDiscoverer( params IGraphTypeHandler[] handlers )
        {
            _handlers = handlers;
        }

        public virtual void Build( Schema schema, SchemaOptions options = null )
        {
            if( options == null ) options = SchemaOptions.DefaultOptions;

            // This custom resolver takes a Type (which is not a GraphType) and return a GraphType by resolving from handlers.
            // TODO: cache the resolution into a static, thread-safe, in-memory cache.
            schema.ResolveType = ( type ) =>
            {
                foreach( var handler in _handlers )
                {
                    var graphType = handler.ResolveGraphType( type );
                    if( graphType != null ) return graphType;
                }
                return null;
            };

            _types
                .Apply( type =>
                {
                    RegisterType( type, schema );
                } );

            schema.Query = (ObjectGraphType)schema.FindType( options.QueryRootTypeName );
        }

        public virtual void Register( Type t )
        {
            _types.Add( t );
        }

        protected virtual GraphType RegisterType( Type type, Schema schema )
        {
            var graphType = schema.FindType( type );
            type.GetMethods( BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly )
                .Where( m => !m.IsSpecialName )
                .Select( CreateMethod )
                .Where( FilterMethod )
                .Apply( m =>
                {
                    var ctx = RegisterMethod( graphType, m, schema );
                    graphType.AddField( ctx.BuildFieldType() );
                } );

            type
                .GetProperties()
                .Select( CreateProperty )
                .Where( FilterProperty )
                .Apply( p =>
                {
                    var ctx = RegisterItem( graphType, p, schema );
                    graphType.AddField( ctx.BuildFieldType() );
                } );

            var interfaces = type
                .GetInterfaces()
                .Select( t =>
                {
                    var item = new ItemMetadata
                    {
                        ItemType = t,
                        Name = t.Name,
                        Attributes = t.GetCustomAttributes().OfType<IGraphAttribute>().ToArray()
                    };
                    return item;
                } )
                .Where( item => FilterInterface( graphType, item, schema ) );

            var objectGraphType = graphType as ObjectGraphType;
            if( objectGraphType != null )
            {
                objectGraphType.Interfaces = interfaces.Select( t => t.GetType() );
            }

            var interfaceGraphType = graphType as InterfaceGraphType;
            if( interfaceGraphType != null )
            {
                // The resolver for the InterfaceGraphType is simply the resolver of the Schema...
                interfaceGraphType.ResolveType = ( o ) => schema.ResolveType( o.GetType() ) as ObjectGraphType;
            }

            return graphType;
        }

        protected virtual GraphItemResolutionContext RegisterMethod( GraphType graphType, MethodMetadata m, Schema schema )
        {
            var ctx = RegisterItem( graphType, m, schema );

            foreach( var arg in m.Arguments )
            {
                var argCtx = new GraphItemResolutionContext();

                foreach( var handler in _handlers )
                {
                    argCtx.Name = argCtx.Name ?? handler.BindName( arg );
                    argCtx.Description = argCtx.Description ?? handler.BindDescription( arg );
                    argCtx.DefaultValue = argCtx.DefaultValue ?? handler.BindDefaultValue( arg );
                    argCtx.GraphType = argCtx.GraphType ?? handler.ResolveFieldGraphType( arg );
                }

                QueryArgument argument = new QueryArgument( argCtx.BuildGraphType() )
                {
                    Name = argCtx.Name,
                    Description  = argCtx.Description,
                    DefaultValue = argCtx.DefaultValue
                };
                if( ctx.Arguments == null ) ctx.Arguments = new QueryArguments( new QueryArgument[] { argument } );
                else ctx.Arguments.Add( argument );
            }

            return ctx;
        }

        protected virtual GraphItemResolutionContext RegisterItem( GraphType graphType, ItemMetadata item, Schema schema )
        {
            var ctx = new GraphItemResolutionContext();

            foreach( var handler in _handlers )
            {
                ctx.IsIdentifier = ctx.IsIdentifier ?? handler.IsIdentifier( item );
                ctx.IsNullable = ctx.IsNullable ?? handler.IsNullable( item );
                ctx.GraphType = ctx.GraphType ?? handler.ResolveFieldGraphType( item );
                ctx.Name = ctx.Name ?? handler.BindName( item );
                ctx.Description = ctx.Description ?? handler.BindDescription( item );
                ctx.ResolverType = ctx.ResolverType ?? handler.BindResolver( item );
                ctx.DefaultValue = ctx.DefaultValue ?? handler.BindDefaultValue( item );
                ctx.DeprecationReason = ctx.DeprecationReason ?? handler.BindDeprecationReason( item );
            };

            return ctx;
        }

        protected virtual bool FilterInterface( GraphType graphType, ItemMetadata item, Schema schema )
        {
            bool isGraphTypeInterface = false;
            foreach( var handler in _handlers )
            {
                isGraphTypeInterface |= handler.IsGraphTypeInterface( item );
            }
            return isGraphTypeInterface;
        }

        protected virtual MethodMetadata CreateMethod( MethodInfo methodInfo )
        {
            MethodMetadata m = new MethodMetadata
            {
                Name = methodInfo.Name,
                ItemType = methodInfo.ReturnType,
                Arguments = methodInfo.GetParameters().Where( FilterMethodArgument ).Select( CreateMethodArgument ).ToArray(),
                Attributes = methodInfo.GetCustomAttributes().OfType<IGraphAttribute>().ToArray()
            };
            return m;
        }

        protected virtual Argument CreateMethodArgument( ParameterInfo parameterInfo )
        {
            Argument arg = new Argument
            {
                Name = parameterInfo.Name,
                ItemType = parameterInfo.ParameterType,
                Attributes = parameterInfo.GetCustomAttributes().OfType<IGraphAttribute>().ToArray()
            };
            return arg;
        }

        protected virtual ItemMetadata CreateProperty( PropertyInfo propertyInfo )
        {
            var item = new ItemMetadata
            {
                ItemType = propertyInfo.PropertyType,
                Name = propertyInfo.Name,
                Attributes = propertyInfo.GetCustomAttributes().OfType<IGraphAttribute>().ToArray()
            };
            return item;
        }

        protected virtual bool FilterMethod( MethodMetadata methodInfo )
        {
            return true;
        }

        protected virtual bool FilterProperty( ItemMetadata item )
        {
            return !item.HasAttribute<IgnoreAttribute>();
        }

        protected virtual bool FilterMethodArgument( ParameterInfo p )
        {
            return true;
        }

    }

}
