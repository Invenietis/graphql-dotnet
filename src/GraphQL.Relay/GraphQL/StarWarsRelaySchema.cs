using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Tests;
using GraphQL.Types;

namespace GraphQL.Relay
{
    public class StarWarsRelaySchema : StarWarsSchema<ViewerType<StarWarsQuery>>
    {
        public StarWarsRelaySchema() : base()
        {
            //Mutation = null;
        }
    }
}
