using System;
using GraphQL.Types;

namespace GraphQL.Tests
{
    public class StarWarsSchema<QRoot> : Schema where QRoot : ObjectGraphType 
    {
        public StarWarsSchema()
        {
            Query = (QRoot)ResolveType( typeof( QRoot ) );
            Mutation = new StarWarsMutations();
        }
    }

    public class StarWarsSchema : StarWarsSchema<StarWarsQuery> { }
}
