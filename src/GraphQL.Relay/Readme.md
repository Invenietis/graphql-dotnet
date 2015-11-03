Relay
==
A Relay sample using the tests StarWarsSchema.

To compile it simple uses:
```
npm install
```

Restores packages needed to run the web sample:
```
dnu restore
```

This dnx command will generates the schema.json required by the plugin *babelRelayPlugin* (which you can found under the build folder).
By default, it creates the schema.json under the *data* folder.
```
dnx buildschema
```

Runs the website. 
```
dnx web
```

```
webpack --progress
```

This will launch the server at localhost:5000.

The Viewer node
==

Since Relay does not supports arrays as a primary node, the sample implements the viewer pattern.
See: https://github.com/facebook/relay/issues/112

* A new GraphType *ViewerType&lt;Q&gt;* has been introduced in GraphQL.Types. 
* In tests project GraphQL.Tests, *StarWarsSchema&lt;QRoot&gt;* has been introduced. QRoot is an ObjectGraphType that wraps the basic StarWarsQuery. 

Finaly, a simple ASP.Net Middleware handle requests :
```csharp
app.UseGraphQL( "/graphql" ); 
```

Entity Framework
==
StarWarsData from test project is modified to fit EntityFramework Code First format.
This Relay sample uses EntityFramework over SqlServer.

Mutations
==
Works from SaltyDH on Mutations is integrated in this sample. (https://github.com/SaltyDH/graphql-dotnet).
Mutations has then been disabled, since it seems to miss something for Relay.

GraphQL.SchemaBuilder
==
Dnx command to build schema.json.
You must provides the schema type name and the assembly name where to found your final Schema.
In the Relay sample, the command is:  "GraphQL.SchemaBuilder --schemaTypeName GraphQL.Relay.StarWarsRelaySchema --schemaAssemblyName GraphQL.Relay".
