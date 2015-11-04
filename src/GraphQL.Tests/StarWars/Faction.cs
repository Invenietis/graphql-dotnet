using System.Collections.Generic;
using GraphQL.Types;

namespace GraphQL.Tests
{
    public class Faction : ObjectGraphType
    {
        public Faction()
        {
            Name = "Faction";
            Field<NonNullGraphType<IntGraphType>>( "factionId" );
            Field<StringGraphType>( "factionName" );
            Field<ListGraphType<Ship>>( "ships", resolve: context =>
            {
                var source = context.Source as Faction;
                return source.Ships;
            } );
        }

        public int FactionId { get; set; }

        public string FactionName { get; set; }

        public virtual ICollection<Ship> Ships { get; set; }
    }
}