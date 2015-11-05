using GraphQL.Types;

namespace GraphQL.Tests
{
    public class IntroduceShipInput : InputObjectGraphType
    {
        public IntroduceShipInput()
        {
            Name = "IntroduceShipInput";
            
            Field<NonNullGraphType<IntGraphType>>( "FactionId", "Id of the faction" );
            Field<NonNullGraphType<StringGraphType>>( "ShipName", "Name of the ship" );
        }

        public int FactionId { get; set; }

        public string ShipName { get; set; }

        public string ClientMutationId { get; set; }

    }
}