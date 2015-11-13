using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphQL.Registration
{
    [AttributeUsage( AttributeTargets.Parameter )]
    public class DefaultValueAttribute : Attribute, IGraphAttribute
    {
        public object DefaultValue { get; set; }
    }
}
