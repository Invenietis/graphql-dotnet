using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Should;

namespace GraphQL.Tests.StarWars
{
    public class DatabaseIntializerTests
    {
        StarWarsData _data = new StarWarsData();



        private void InitializeDatabase()
        {
            Database.SetInitializer( new DropCreateDatabaseAlways<StarWarsData>() );
            _data.InitializeData();
            _data.SaveChanges();
        }

        [Test]
        public async Task initialize_database()
        {
            InitializeDatabase();

            int count = await _data.Humans.CountAsync();
            count.ShouldEqual( 2 );
            count = await _data.Droids.CountAsync();
            count.ShouldEqual( 2 );
        }

    }
}
