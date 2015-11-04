﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Http;
using GraphQL.Types;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Newtonsoft.Json;
using Microsoft.Extensions.DependencyInjection;

namespace GraphQL.Relay
{
    public class GraphQLMiddleware<TContext> where TContext : IDisposable
    {
        private readonly RequestDelegate _next;
        readonly Schema _schema;
        readonly IDocumentExecuter _executer;
        readonly IDocumentWriter _writer;
        readonly Func<TContext> _context;

        public GraphQLMiddleware( RequestDelegate next, Schema schema, Func<TContext> context )
        {
            _next = next;
            _schema = schema;
            _executer = new DocumentExecuter();
            _writer = new DocumentWriter( Formatting.Indented );
            _context = context;
        }

        public async Task Invoke( HttpContext httpContext )
        {
            using( var dbContext = _context() )
            {
                var query = GetQuery( httpContext );
                var result = await _executer.ExecuteAsync( _schema, dbContext, query.Query, null, query.Variables, httpContext.RequestAborted );

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
        public static IApplicationBuilder UseGraphQL<TContext>( this IApplicationBuilder builder, PathString routePrefix, Schema schema, Func<TContext> context ) where TContext : IDisposable
        {
            if( string.IsNullOrWhiteSpace( routePrefix ) ) throw new ArgumentNullException( nameof( routePrefix ) );

            return builder.Map( routePrefix, ( app ) =>
            {
                app.UseMiddleware<GraphQLMiddleware<TContext>>( schema, context );
            } );
        }
    }
}