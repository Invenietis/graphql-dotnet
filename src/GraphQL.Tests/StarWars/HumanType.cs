using GraphQL.Types;

namespace GraphQL.Tests
{
    public class HumanType : ObjectGraphType
    {
        public HumanType()
        {
            Name = "Human";

            Field<NonNullGraphType<StringGraphType>>( "id", "The id of the human." );
            Field<StringGraphType>( "name", "The name of the human." );
            Field<ListGraphType<CharacterInterface>>(
                "friends",
                resolve: context =>
                {
                    var source =  context.Source as StarWarsCharacter;
                    //context.Root.As<StarWarsData>().GetFriends( context.Source as StarWarsCharacter );
                    return source.Friends;
                }
            );
            Field<ListGraphType<EpisodeEnum>>( "appearsIn", "Which movie they appear in." );
            Field<StringGraphType>( "homePlanet", "The home planet of the human." );

            Interface<CharacterInterface>();
        }
    }
    
}
