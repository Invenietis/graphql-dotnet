using GraphQL.Types;

namespace GraphQL.Tests
{
    public class IntroduceShipPayload : OutputGraphType<IntroduceShipInput>
    {
        public IntroduceShipPayload()
        {
            Name = "IntroduceShipPayload";

            Field<NonNullGraphType<Ship>>( "newShipEdge" );
            Field<NonNullGraphType<Faction>>( "faction" );
            Field<StringGraphType>( "clientMutationId" );
        }

        public Ship Ship { get; set; }
        public Faction Faction { get; set; }
        public string ClientMutationId { get; set; }
    }
}