using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Tests;
using GraphQL.Types;

namespace GraphQL.Relay.StarWars
{
    public class StarWarsRelaySchema : StarWarsSchema
    {
        public StarWarsRelaySchema() : base()
        {
            Mutation = new StarWarsMutations();
        }

        public Func<DB.StarWarsDbContext> DbContextFactory { get; internal set; }

        public DB.StarWarsDbContext OpenDatabase()
        {
            return DbContextFactory();
        }
    }
}
