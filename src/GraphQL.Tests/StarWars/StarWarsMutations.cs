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
            Mutation<IntroduceShipInput, IntroduceShipPayload>( "introduceShip", ( input, ctx ) =>
            {
                var faction = new Faction
                {
                    FactionName = "Alliance to Restore the Republic"
                };

                var newShip = new IntroduceShipPayload
                {
                    Ship = new Ship
                    {
                        Id = "U2hpcDo5",
                        ShipName = input.ShipName
                    },

                    Faction = faction,

                    ClientMutationId = input.ClientMutationId
                };

                return newShip;
            } );
        }
    }
}