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
                context.Root.As<StarWarsData>().Droids.FirstOrDefault( x => x.Id == "3" )
            );
            Field<ListGraphType<HumanType>>( "humans", resolve: context => context.Root.As<StarWarsData>().Humans );
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
                    return context.Root.As<StarWarsData>().Humans.FirstOrDefault( x => x.Id == a );
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
                    return context.Root.As<StarWarsData>().Droids.FirstOrDefault( x => x.Id == a );
                }
            );
        }
    }
}
