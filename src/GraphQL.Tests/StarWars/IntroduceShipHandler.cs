﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphQL.Handlers;
using GraphQL.Types;

namespace GraphQL.Tests
{
    class IntroduceShipHandler : MutationHandler<IntroduceShipInput, IntroduceShipPayload>
    {
        readonly StarWarsData _data;
        public IntroduceShipHandler( ResolveFieldContext ctx )
        {
            _data = ctx.Source.As<StarWarsData>();
        }
         
        public override IntroduceShipPayload Handle( IntroduceShipInput command )
        {
            //var data = ctx.Cast<StarWarsData>().Source;
            foreach( var s in _data.Ships.ToArray() ) _data.Ships.Remove( s );

            var faction = _data.Factions.FirstOrDefault( f  => f.FactionId == command.FactionId );
            var newShip = new IntroduceShipPayload
            {
                Ship = new Ship
                {
                    Id = "U2hpcDo5",
                    ShipName = command.ShipName
                },

                Faction = faction,
                
                ClientMutationId = command.ClientMutationId
            };

            _data.Ships.Add( newShip.Ship );

            _data.SaveChanges();
            return newShip;
        }
    }
}
