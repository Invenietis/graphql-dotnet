using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphQL.Types;

namespace GraphQL.Registration
{
    public class QueryArgumentAttribute : GraphPropertyAttribute
    {
        public string ArgumentName { get; set; }

        public Type ArgumentType { get; set; }

        public string Description { get; set; }

        public QueryArgumentAttribute( string argument, Type argumentType )
        {
            ArgumentName = argument;
            ArgumentType = argumentType;
        }
    }

}
