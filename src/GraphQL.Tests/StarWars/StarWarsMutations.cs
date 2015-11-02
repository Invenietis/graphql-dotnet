using GraphQL.Types;
using System.Linq;

namespace GraphQL.Tests
{
    public class StarWarsMutations : ObjectGraphType
    {
        public StarWarsMutations()
        {
            Name = "Mutations";
            // Command, CommandResult, Handler, CommandName
            Mutation<IntroduceShipInput, IntroduceShipPayload, IntroduceShipHandler>( "introduceShip" );
        }
    }
}