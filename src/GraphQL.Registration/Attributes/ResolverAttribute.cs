using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphQL.Registration
{
    public class ResolverAttribute : GraphPropertyAttribute
    {
        public Type ResolverType { get; set; }
        public ResolverAttribute( Type resolverType )
        {
            if( !typeof( IResolver ).IsAssignableFrom( resolverType ) )
                throw new ArgumentException( "The resolverType must implement IResolver...", nameof( resolverType ) );
            if( resolverType.IsAbstract )
                throw new ArgumentException( "The resolverType should be instanciable...", nameof( resolverType ) );

            ResolverType = resolverType;
        }
    }
}
