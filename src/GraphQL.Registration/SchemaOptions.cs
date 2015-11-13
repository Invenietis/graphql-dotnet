using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphQL.Registration
{
    public class SchemaOptions
    {
        public string QueryRootTypeName { get; set; }

        public string MutationRootTypeName { get; set; }

        public static SchemaOptions DefaultOptions = new SchemaOptions
        {
            QueryRootTypeName = "Query",
            MutationRootTypeName = "Mutation"
        };
    }

}
