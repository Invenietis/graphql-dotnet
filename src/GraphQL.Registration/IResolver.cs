using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphQL.Types;

namespace GraphQL.Registration
{
    public interface IResolver
    {
        object Resolve( ResolveFieldContext ctx );
    }
}
