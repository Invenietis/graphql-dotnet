using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphQL.Registration
{
    [AttributeUsage( AttributeTargets.Property | AttributeTargets.Method, AllowMultiple = false, Inherited = false )]
    public abstract class GraphPropertyAttribute : Attribute, IGraphAttribute
    {
    }

}
