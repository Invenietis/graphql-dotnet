using GraphQL.Types;
using System.Linq;
using System.Data.Entity;

namespace GraphQL.Tests
{
    public class StarWarsQuery : ObjectGraphType
    {
        public StarWarsQuery()
        {
            Name = "Query";

            Field<CharacterInterface>( "hero", resolve: context =>
                context.Cast<StarWarsData>().Source.Droids.FirstOrDefault( x => x.Id == "3" )
            );
            Field<ListGraphType<HumanType>>( "humans", resolve: context => context.Cast<StarWarsData>().Source.Humans );
            Field<ListGraphType<DroidType>>( "droids", resolve: context => context.Cast<StarWarsData>().Source.Droids );
            Field<HumanType>(
                "human",
                arguments: new QueryArguments(
                    new[]
                    {
                        new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "id", Description = "id of the human" }
                    } ),
                resolve: context =>
                {
                    string a = (string)context.Arguments["id"];
                    return context.Cast<StarWarsData>().Source.Humans.FirstOrDefault( x => x.Id == a );
                }
            );
            Field<DroidType>(
                "droid",
                arguments: new QueryArguments(
                    new[]
                    {
                        new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "id", Description = "id of the droid" }
                    } ),
                resolve: context =>
                {
                    string a = (string)context.Arguments["id"];
                    return context.Cast<StarWarsData>().Source.Droids.FirstOrDefault( x => x.Id == a );
                }
            );

            Field<ListGraphType<Faction>>( "factions", resolve: context => context.Cast<StarWarsData>().Source.Factions );
            Field<ListGraphType<Ship>>( "ships", resolve: context => context.Cast<StarWarsData>().Source.Ships );
        }
    }
}
