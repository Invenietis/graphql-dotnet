using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphQL.Types;

namespace GraphQL.Registration
{
    public class AnnotationGraphTypeHandler : IGraphTypeHandler
    {
        public Type BindGraphType( ItemMetadata item )
        {
            return item.GetAttribute<GraphTypeAttribute>()?.GraphType;
        }

        public bool IsNullable( ItemMetadata item )
        {
            return item.HasAttribute<NonNullAttribute>();
        }

        public string BindName( ItemMetadata item )
        {
            return item.GetAttribute<NameAttribute>()?.Name;
        }

        public string BindDescription( ItemMetadata item )
        {
            return item.GetAttribute<DescriptionAttribute>()?.Description;
        }

        public Type BindResolver( ItemMetadata item )
        {
            return item.GetAttribute<ResolverAttribute>()?.ResolverType;
        }

        public GraphType ResolveGraphType( Type type )
        {
            GraphTypeAttribute a = type.GetCustomAttributes().OfType<GraphTypeAttribute>().FirstOrDefault();
            if( a == null ) return null;

            GraphType graphType = Activator.CreateInstance( a.GraphType ) as GraphType;
            if( graphType == null ) throw new InvalidOperationException( "Only GraphType can be registered in the Schema." );

            graphType.Name = a.Name;
            graphType.Description = a.Description;
            return graphType;
        }

        public object BindDefaultValue( ItemMetadata item )
        {
            return item.GetAttribute<DefaultValueAttribute>()?.DefaultValue;
        }

        public bool? IsIdentifier( ItemMetadata item )
        {
            return item.HasAttribute<IdGraphAttribute>();
        }

        public string BindDeprecationReason( ItemMetadata item )
        {
            return item.GetAttribute<DeprecatedAttribute>()?.Reason;
        }
    }
}
