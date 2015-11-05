using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphQL.Types;

namespace GraphQL.Relay.TodoMVC
{
    public class Relay
    {

        public static IEnumerable<QueryArgument> GetConnectionFieldArguments()
        {
            yield return new QueryArgument<StringGraphType> { Name = "before" };
            yield return new QueryArgument<StringGraphType> { Name = "after" };
            yield return new QueryArgument<IntGraphType> { Name = "first" };
            yield return new QueryArgument<IntGraphType> { Name = "last" };
        }

    }
}
