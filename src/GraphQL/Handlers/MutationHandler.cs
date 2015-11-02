using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphQL.Handlers
{
    public abstract class MutationHandler<T, TResult>
    {
        public abstract TResult Handle( T mutation );
    }
}
