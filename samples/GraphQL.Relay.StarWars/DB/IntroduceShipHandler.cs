using System;
using System.Collections.Generic;
using System.Linq;
using GraphQL.Handlers;
using GraphQL.Tests;
using GraphQL.Types;

namespace GraphQL.Relay.StarWars
{
    class IntroduceShipHandler : MutationHandler<IntroduceShipInput, IntroduceShipPayload>
    {
        readonly ResolveFieldContext _ctx;
        public IntroduceShipHandler( ResolveFieldContext ctx )
        {
            _ctx = ctx;
        }

        public override IntroduceShipPayload Handle( IntroduceShipInput command )
        {
            using( var db = _ctx.Schema.As<StarWarsRelaySchema>().OpenDatabase() )
            {
                var faction = db.Factions.FirstOrDefault( f  => f.FactionId == command.FactionId );
                var newShip = new IntroduceShipPayload
                {
                    Ship = new Ship
                    {
                        Id = Guid.NewGuid().ToString("N"),
                        ShipName = command.ShipName
                    },

                    Faction = faction,

                    ClientMutationId = command.ClientMutationId
                };

                faction.Ships.Add( newShip.Ship );

                db.SaveChanges();
                return newShip;
            }
        }
    }
}
