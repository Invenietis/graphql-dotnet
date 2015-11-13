using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphQL.Types;
using System.Reflection;

namespace GraphQL.Registration
{
    public class ConventionGraphTypeHandler : IGraphTypeHandler
    {
        /// <summary>
        /// By convention, the <see cref="GraphType"/> is an <see cref="ObjectGraphType"/>. 
        /// The name is the type name.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public GraphType ResolveGraphType( Type type )
        {
            GraphType graphType = Activator.CreateInstance( typeof( ObjectGraphType ) ) as GraphType;
            graphType.Name = type.Name;
            return graphType;
        }

        public Type BindResolver( ItemMetadata field )
        {
            return null;
        }

        public string BindName( ItemMetadata itemMetadata )
        {
            string name = itemMetadata.Name;
            return char.ToLowerInvariant( name[0] ) + name.Substring( 1 );
        }

        public string BindDescription( ItemMetadata itemMetadata )
        {
            return null;
        }

        public bool? IsIdentifier( ItemMetadata item )
        {
            return item.Name == "id";
        }

        public bool IsNullable( ItemMetadata item )
        {
            return !item.ItemType.IsPrimitive;
        }

        public Type BindGraphType( ItemMetadata item )
        {
            return FindGraphTypeByConvention( item.ItemType );
        }

        public object BindDefaultValue( ItemMetadata itemMetadata )
        {
            return null;
        }

        public string BindDeprecationReason( ItemMetadata item )
        {
            return null;
        }

        private Type FindGraphTypeByConvention( Type propertyType )
        {
            if( propertyType.IsGenericType )
            {
                Type innerType = TryListOfWhat( propertyType, typeof( IEnumerable<> ), typeof( ICollection<> ), typeof( IList<> ) );
                if( innerType != null )
                {
                    var graphType = FindGraphTypeByConvention( innerType );
                    return typeof( ListGraphType<> ).MakeGenericType( graphType );
                }

                var def = propertyType.GetGenericTypeDefinition();
                if( def == typeof( Nullable<> ) )
                {
                    return FindGraphTypeByConvention( propertyType.GetGenericArguments().Single() );
                }
            }
            if( typeof( int ) == propertyType || typeof( long ) == propertyType ) return typeof( IntGraphType );
            if( typeof( string ) == propertyType ) return typeof( StringGraphType );
            if( typeof( float ) == propertyType ) return typeof( FloatGraphType );
            if( typeof( bool ) == propertyType ) return typeof( BooleanGraphType );

            return typeof( ObjectGraphType );
        }

        /// <summary>
        /// Test if a type implements IList of T, and if so, determine T.
        /// </summary>
        internal static Type TryListOfWhat( Type type, params Type[] typesToCheck )
        {
            var interfaceTest = new Func<Type, Type, Type>((i,typeToCheck) => i.IsGenericType && i.GetGenericTypeDefinition() == typeToCheck ? i.GetGenericArguments().Single() : null);

            foreach( var typeToCheck in typesToCheck )
            {
                var innerType = interfaceTest( type, typeToCheck );
                if( innerType != null )
                {
                    return innerType;
                }

                foreach( var i in type.GetInterfaces() )
                {
                    innerType = interfaceTest( i, typeToCheck );
                    if( innerType != null )
                    {
                        return innerType;
                    }
                }

            }
            return null;
        }
    }
}
