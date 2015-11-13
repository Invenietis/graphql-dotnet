using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphQL.Types;

namespace GraphQL.Registration
{
    public interface IGraphTypeHandler
    {
        /// <summary>
        /// Resolves the given <see cref="Type"/> to a <see cref="GraphType"/>. If no resolution can be performed, returns null.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        GraphType ResolveGraphType( Type type );

        Type BindResolver( ItemMetadata item );

        string BindName( ItemMetadata item );

        string BindDescription( ItemMetadata item );

        bool IsNullable( ItemMetadata item );

        Type BindGraphType( ItemMetadata item );

        object BindDefaultValue( ItemMetadata item );

        bool? IsIdentifier( ItemMetadata item );
        string BindDeprecationReason( ItemMetadata item );
    }
}
