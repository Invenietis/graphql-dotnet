using GraphQL.Types;

namespace GraphQL.Tests
{
    public class IntroduceShipInput : InputObjectGraphType
    {
        public IntroduceShipInput()
        {
            Name = "IntroduceShipInput";
            
            Field<IntGraphType>( "FactionId", "Id of the faction" );
            Field<StringGraphType>( "ShipName", "Name of the ship" );
            Field<StringGraphType>( "ClientMutationId", "Client mutation id" );
        }

        public int FactionId { get; set; }

        public string ShipName { get; set; }

        public string ClientMutationId { get; set; }
    }
}