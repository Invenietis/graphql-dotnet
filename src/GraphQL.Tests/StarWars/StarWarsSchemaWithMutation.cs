using System;
using GraphQL.Types;

namespace GraphQL.Tests
{
    public class StarWarsSchemaWithMutation : StarWarsSchema
    {
        public StarWarsSchemaWithMutation()
        {
            Query = new StarWarsQuery();
            Mutation = new StarWarsMutations();
        }
    }
}
