using GraphQL.Types;

namespace GraphQL.Tests
{
    public class Ship : ObjectGraphType
    {
        public Ship()
        {
            Name = "ship";
            Field<StringGraphType>("id");
            Field<StringGraphType>("shipName");
        }

        public string Id { get; set; }
        public string ShipName { get; set; }
    }
}