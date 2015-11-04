using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Http;
using GraphQL.Types;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace GraphQL.Relay.TodoMVC
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
            app.UseGraphQL( "/graphql", new TodoMVCSchema(), () => new DB.TodoMVCDbContext() );
        }
    }
}
