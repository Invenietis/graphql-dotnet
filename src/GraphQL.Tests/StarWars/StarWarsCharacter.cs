using System.Collections.Generic;
using System.Linq;

namespace GraphQL.Tests
{
    public abstract class StarWarsCharacter
    {
        public StarWarsCharacter()
        {
            Friends = new HashSet<StarWarsCharacter>();
            Appearances = new HashSet<StarWarsEpisod>();
        }

        public string Id { get; set; }
        public string Name { get; set; }

        public int[] AppearsIn
        {
            get { return Appearances.Select( x => x.Id ).ToArray(); }
        }

        public virtual ICollection<StarWarsEpisod> Appearances { get; set; }

        public virtual ICollection<StarWarsCharacter> Friends { get; set; }
    }

    public class Human : StarWarsCharacter
    {
        public string HomePlanet { get; set; }
    }

    public class Droid : StarWarsCharacter
    {
        public string PrimaryFunction { get; set; }
    }
}
