namespace GraphQL.Tests
{
    public class StarWarsBasicMutationTests : QueryTestBase<StarWarsSchemaWithMutation>
    {

        [Test]
        public void can_introduce_ship_mutation()
        {

            var mutation = @"
                mutation AddBWingQuery($input: IntroduceShipInput!) {
                    introduceShip(input: $input) {
                        ship {
                            id
                            shipName
                        }
                        faction {
                            factionName
                        }
                        clientMutationId
                    }
                }";

            var inputs = new Inputs
                {
                    {
                        "input", new
                        {
                            shipName = "B-Wing",
                            factionId = 1,
                            clientMutationId = "abcde"
                        }
                    }
                };

            var expected = @"{
    ""introduceShip"": {
      ""ship"": {
        ""id"": ""U2hpcDo5"",
        ""shipName"": ""B-Wing""
      },
      ""faction"": {
        ""factionName"": ""Alliance to Restore the Republic""
      },
      ""clientMutationId"": ""abcde""
    }
  }";

            AssertQuerySuccess( mutation, expected, inputs );
        }
    }
}