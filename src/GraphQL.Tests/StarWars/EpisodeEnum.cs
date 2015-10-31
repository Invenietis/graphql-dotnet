using GraphQL.Types;

namespace GraphQL.Tests
{

    public class EpisodeEnum : EnumerationGraphType
    {
        public EpisodeEnum()
        {
            Name = "Episode";
            Description = "One of the films in the Star Wars Trilogy.";
            AddValue( "NEWHOPE", "Released in 1977.",(int)StarWarsEpisodEnum.NEWHOPE );
            AddValue( "EMPIRE", "Released in 1980.", (int)StarWarsEpisodEnum.EMPIRE );
            AddValue( "JEDI", "Released in 1983.", (int)StarWarsEpisodEnum.JEDI );
        }
    }
}
