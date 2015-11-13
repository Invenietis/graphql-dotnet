using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphQL.Registration
{
    public class DeprecatedAttribute : GraphPropertyAttribute
    {
        public string Reason { get; set; }
        public DeprecatedAttribute( string reason, bool isDeprecated = true )
        {
            Reason = reason;
        }
    }

}
