using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphQL.Types;

namespace GraphQL.Registration
{
    [AttributeUsage( AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Property, AllowMultiple = true, Inherited = false )]
    public class GraphTypeAttribute : Attribute, IGraphAttribute
    {
        public Type GraphType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public GraphTypeAttribute( Type graphType )
        {
            if( !typeof( GraphType ).IsAssignableFrom( graphType ) )
                throw new ArgumentException( "The type must be a GraphType", nameof( graphType ) );

            GraphType = graphType;
        }
    }
}
