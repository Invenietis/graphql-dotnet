using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphQL.Types
{
    public class ViewerType<Q> : ObjectGraphType where Q : ObjectGraphType
    {
        public ViewerType()
        {
            Name = "Viewer";
            Field<Q>(
                "viewer",
                "Query root to workaround Relay issue described here: https://github.com/facebook/relay/issues/112",
                resolve: context =>
                {
                    return context.Source;
                    //return null;
                } );
        }
    }

}
