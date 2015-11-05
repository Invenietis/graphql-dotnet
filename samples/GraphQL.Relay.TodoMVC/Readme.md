Relay
==
A Relay sample using the TodoMVC app.

Getting started
---

You need to have SqlServer installed somewhere. To put your connection string, simply updates the configuration in serverconfig.json.

Restores client packages:
```
npm install
```

Restores server packages needed to run the web sample:
```
dnu restore
```

This dnx command will generates the schema.json required by the plugin *babelRelayPlugin* (which you can found under the build folder).
By default, it creates the schema.json under the *data* folder.
```
dnx buildschema
```

To workaround the issue that there is two instances of React loaded in the web application, we need to remove the second.
Run the folowing command:
```
rimraf node_modules/react-relay/node_modules/react
```

Webpack the js and jsx into a bundle file.  Webpack will use the *babelRelayPlugin* and reads the schema.json previously generated.
```
webpack --progress
```

Run the website. 
```
dnx web
```

This will launch the server at localhost:5001.
Type http://localhost:5001/index.html in your favorite browser and enjoy !

The Viewer node
---

Since Relay does not supports arrays as a primary node, the sample implements the viewer pattern.
See: https://github.com/facebook/relay/issues/112

Server Side
---
A simple ASP.Net Middleware handles graphql requests:
```csharp
app.UseGraphQL( "/graphql" ); 
```

Entity Framework
---
This Relay sample uses EntityFramework over SqlServer.

Mutations
---
Works from SaltyDH on Mutations is included in this sample. (https://github.com/SaltyDH/graphql-dotnet).

GraphQL.SchemaBuilder
---
Dnx command to build schema.json.
You must provides the schema type name and the assembly name where to found your final Schema.
In the Relay sample, the command is:  "GraphQL.SchemaBuilder --schemaTypeName GraphQL.Relay.TodoMVC.TodoMVCSchema --schemaAssemblyName GraphQL.Relay.TodoMVC".
