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
using Microsoft.Data.Entity;
using Microsoft.Extensions.Configuration;

namespace GraphQL.Relay.TodoMVC
{
    public class Startup
    {
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices( IServiceCollection services )
        {
            ConfigurationBuilder b = new Microsoft.Extensions.Configuration.ConfigurationBuilder();
            b.AddJsonFile( "serverconfig.json", false );
            Configuration = b.Build();
        }

        public static IConfigurationRoot Configuration { get; set; }

        public void Configure( IApplicationBuilder app )
        {

            // Add the platform handler to the request pipeline.
            app.UseStaticFiles();

            // This will creates the "current" user also known as "viewer".
            using( var db = new DB.TodoMVCDbContext() )
            {
                if( db.Users.Count() == 0 )
                {
                    db.Users.Add( new DB.User { Id = 0, UserName = "Anonymous" } );
                    db.SaveChanges();
                }
            }

            app.UseGraphQL( "/graphql", new TodoMVCSchema
            {
                DbContextFactory = () => new DB.TodoMVCDbContext()
            } );
        }
    }
}
