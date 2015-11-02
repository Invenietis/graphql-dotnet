using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Should;

namespace GraphQL.Tests
{
    public class StarWarsBasicViewerQueryTests : QueryTestBase<StarWarsSchema<ActorType<StarWarsQuery>>>
    {
        StarWarsData _data = new StarWarsData();

        public StarWarsBasicViewerQueryTests()
        {
        }

        [Test]
        public void query_same_root_field_using_alias()
        {
            var query = @"
            query SomeDroids {
                viewer {
                  r2d2: droid(id: ""3"") {
                    name
                  }

                  c3po: droid(id: ""4"") {
                    name
                  }
               }
            }
            ";

            var expected = @"{
                viewer: {
                  'r2d2': {
                    name: 'R2-D2'
                  },
                  'c3po': {
                    name: 'C-3PO'
                  }
                }
            }";

            AssertQuerySuccess( query, expected, root: _data );
        }
    }
}
