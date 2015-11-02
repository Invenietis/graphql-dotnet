using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphQL.Types;

namespace GraphQL.Handlers
{
    public interface IMutationHandlerFactory
    {
        THandler CreateHandler<TInput, TPayload, THandler>( ResolveFieldContext ctx )
            where TInput : Types.GraphType
            where TPayload : class
            where THandler : MutationHandler<TInput, TPayload>;
    }

    class DefaultHandlerFactory : IMutationHandlerFactory
    {
        public THandler CreateHandler<TInput, TPayload, THandler>( ResolveFieldContext ctx )
            where TInput : GraphType
            where TPayload : class
            where THandler : MutationHandler<TInput, TPayload>
        {
            return (THandler)Activator.CreateInstance( typeof( THandler ), ctx );
        }
    }
}
