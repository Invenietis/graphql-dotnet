using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using GraphQL.Http;
using GraphQL.Types;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.PlatformAbstractions;
using Newtonsoft.Json;

namespace GraphQL.SchemaBuilder
{
    public class Program
    {
        private readonly IApplicationEnvironment _appEnv;
        private readonly ILibraryManager _libMgr;

        public Program( ILibraryManager libraryManager, IApplicationEnvironment appEnv )
        {
            _libMgr = libraryManager;
            _appEnv = appEnv;
        }

        public async void Main( string[] args )
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("schemaBuilder.json", true)
                .AddCommandLine(args)
                .Build();


            string schemaTypeName = config["schemaTypeName"];
            if( String.IsNullOrEmpty( schemaTypeName ) )
            {
                Console.WriteLine( "SchemaTypeName must be provide: Assembly Qualified name (GraphQL.Tests.StarWarsSchema, GraphQL.Tests)." );
                return;
            }

            string schemaAssemblyName = config["schemaAssemblyName"];
            if( String.IsNullOrEmpty( schemaAssemblyName ) )
            {
                Console.WriteLine( "SchemaAssemblyName must be provide: Assembly Qualified name (GraphQL.Tests.StarWarsSchema, GraphQL.Tests)." );
                return;
            }

            string outputFilePath = config["output"];
            if( String.IsNullOrEmpty( outputFilePath ) )
            {
                outputFilePath = Path.Combine( _appEnv.ApplicationBasePath, "data", "schema.json" );
            }
            Type schemaType = Assembly.Load( schemaAssemblyName ).GetType( schemaTypeName );

            var schema = (Schema)Activator.CreateInstance(schemaType);
            var executer = new DocumentExecuter();
            var result = await executer.ExecuteAsync( schema, Introspection.SchemaIntrospection.SchemaMeta, Introspection.SchemaIntrospection.IntrospectionQuery, null );
            var writer = new DocumentWriter( Formatting.Indented );
            var json = writer.Write( result );

            string outputPath = Path.GetDirectoryName( outputFilePath );
            if( !Directory.Exists( outputPath ) ) Directory.CreateDirectory( outputPath );

            File.WriteAllText( outputFilePath, json );

            Console.WriteLine( "Schema created at: {0}", outputFilePath );
        }
    }
}
