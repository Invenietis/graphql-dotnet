using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Http;
using GraphQL.Types;
using Newtonsoft.Json;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;

namespace GraphQL.Relay
{
    public class GraphQLMiddleware
    {
        private readonly RequestDelegate _next;
        readonly Schema _schema;
        readonly IDocumentExecuter _executer;
        readonly IDocumentWriter _writer;

        public GraphQLMiddleware( RequestDelegate next, Schema schema )
        {
            _next = next;
            _schema = schema;
            _executer = new DocumentExecuter();
            _writer = new DocumentWriter( Formatting.Indented );
        }

        public async Task Invoke( HttpContext httpContext )
        {
            var query = GetQuery( httpContext );
            var result = await _executer.ExecuteAsync( _schema, null, query.Query, null, query.Variables, httpContext.RequestAborted );

            var resultJson = _writer.Write( result );
            await httpContext.Response.WriteAsync( resultJson, httpContext.RequestAborted );
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
        public static IApplicationBuilder UseGraphQL( this IApplicationBuilder builder, PathString routePrefix, Schema schema )
        {
            if( string.IsNullOrWhiteSpace( routePrefix ) ) throw new ArgumentNullException( nameof( routePrefix ) );

            return builder.Map( routePrefix, ( app ) =>
            {
                app.UseMiddleware<GraphQLMiddleware>( schema );
            } );
        }
    }
}