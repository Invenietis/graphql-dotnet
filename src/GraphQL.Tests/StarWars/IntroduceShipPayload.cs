using GraphQL.Types;

namespace GraphQL.Tests
{
    public class IntroduceShipPayload : OutputGraphType<IntroduceShipInput>
    {
        public IntroduceShipPayload()
        {
            Name = "IntroduceShipPayload";

            Field<NonNullGraphType<Ship>>( "ship" );
            Field<NonNullGraphType<Faction>>( "faction" );
        }

        public Ship Ship { get; set; }
        public Faction Faction { get; set; }
        public string ClientMutationId { get; set; }
    }
}