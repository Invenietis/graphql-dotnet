using System;
using System.Linq.Expressions;
using GraphQL.Types;

namespace GraphQL.Tests
{
    public class DroidType : ObjectGraphType
    {
        public DroidType()
        {
            Name = "Droid";
            Description = "A mechanical creature in the Star Wars universe.";

            Field<NonNullGraphType<StringGraphType>>( "id", "The id of the droid." );

            Field<StringGraphType>( "name", "The name of the droid." );
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
            Field<StringGraphType>( "primaryFunction", "The primary function of the droid." );

            Interface<CharacterInterface>();
        }
    }
}
