using GraphQL.Types;

namespace GraphQL.Tests
{
    public class IntroduceShipPayload : ObjectGraphType
    {
        public IntroduceShipPayload()
        {
            Name = "IntroduceShipPayload";

            Field<Ship>("ship");
            Field<Faction>("faction");
            Field<StringGraphType>("clientMutationId");
        }

        public Ship Ship { get; set; }
        public Faction Faction { get; set; }
        public string ClientMutationId { get; set; }
    }
}