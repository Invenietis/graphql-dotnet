using System;
using GraphQL.Types;

namespace GraphQL.Tests
{
    public class StarWarsSchema<QRoot> : Schema where QRoot : ObjectGraphType
    {
        public StarWarsSchema()
        {
            Query = Activator.CreateInstance<QRoot>();
            //Mutation = new StarWarsMutations();
        }
    }

    public class StarWarsSchema : StarWarsSchema<StarWarsQuery> { }
}
