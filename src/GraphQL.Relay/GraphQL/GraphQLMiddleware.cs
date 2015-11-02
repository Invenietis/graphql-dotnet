using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Http;
using GraphQL.Tests;
using GraphQL.Types;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Newtonsoft.Json;

namespace GraphQL
{
    public class GraphQLMiddleware
    {
        private readonly RequestDelegate _next;
        readonly Schema _schema;
        readonly IDocumentExecuter _executer;
        readonly IDocumentWriter _writer;
        public GraphQLMiddleware( RequestDelegate next )
        {
            _next = next;
            _schema = new StarWarsSchema<ActorType<StarWarsQuery>>();
            _executer = new DocumentExecuter();
            _writer = new DocumentWriter( Formatting.Indented );
        }

        public async Task Invoke( HttpContext httpContext )
        {
            using( var data = new StarWarsData() )
            {
                var query = GetQuery( httpContext );
                var result = await _executer.ExecuteAsync( _schema, data, query.Query, null, query.Variables, httpContext.RequestAborted );

                var resultJson = _writer.Write( result );
                await httpContext.Response.WriteAsync( resultJson, httpContext.RequestAborted );
            }
        }

        private GraphQLQuery GetQuery( HttpContext httpContext )
        {
            using( var sr = new StreamReader( httpContext.Request.Body ) )
            {
                return JsonConvert.DeserializeObject<GraphQLQuery>( sr.ReadToEnd() );
            }
        }

        public class GraphQLQuery
        {
            public string Query { get; set; }
            public Inputs Variables { get; set; }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class GraphQLExtensions
    {
        public static IApplicationBuilder UseGraphQL( this IApplicationBuilder builder, PathString routePrefix )
        {
            if( string.IsNullOrWhiteSpace( routePrefix ) ) throw new ArgumentNullException( nameof( routePrefix ) );

            return builder.Map( routePrefix, ( app ) =>
            {
                app.UseMiddleware<GraphQLMiddleware>();
            } );
        }
    }
}