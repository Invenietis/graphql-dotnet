using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.DependencyInjection;

namespace GraphQL.Relay.StarWars
{
    public class Startup
    {
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices( IServiceCollection services )
        {
        }

        public void Configure( IApplicationBuilder app )
        {
            // Add the platform handler to the request pipeline.
            app.UseStaticFiles();

            app.UseGraphQL( "/graphql", new GraphQL.Relay.StarWars.StarWarsRelaySchema
            {
                DbContextFactory = () => new DB.StarWarsDbContext()
            } );
        }
    }
}
