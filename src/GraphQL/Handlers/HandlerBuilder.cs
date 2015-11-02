using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphQL.Handlers
{

    public class HandlerBuilder
    {
        public static HandlerBuilder Current { get; } = new HandlerBuilder();

        public void SetHandlerFactory( IMutationHandlerFactory handlerFactory )
        {
            Factory = handlerFactory;
        }

        public IMutationHandlerFactory Factory { get; private set; } = new DefaultHandlerFactory();
    }

}
