using GraphQL.Types;

namespace GraphQL.Tests
{
    public class Faction : ObjectGraphType
    {
        public Faction()
        {
            Name = "faction";
            Field<StringGraphType>("factionName");
        }

        public int FactionId { get; set; }

        public string FactionName { get; set; }
    }
}