using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Http;
using GraphQL.Tests;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace GraphQL.Relay
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

            IHostingEnvironment host = app.ApplicationServices.GetRequiredService<IHostingEnvironment>();

            var schema = new StarWarsSchema<ActorType<StarWarsQuery>>();

            var executer = new DocumentExecuter();

            executer.ExecuteAsync( schema, Introspection.SchemaIntrospection.SchemaMeta, Introspection.SchemaIntrospection.IntrospectionQuery, null  )
               .ContinueWith( x =>
               {
                   var writer = new DocumentWriter( Formatting.Indented );
                   File.WriteAllText( Path.Combine( host.WebRootPath, "data", "schema.json" ), writer.Write( x.Result ) );
               } );

            app.UseGraphQL( "/graphql" );
        }
    }
}
