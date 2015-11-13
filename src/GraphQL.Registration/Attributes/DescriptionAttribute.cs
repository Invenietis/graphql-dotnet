using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphQL.Registration
{
    public class DescriptionAttribute : GraphPropertyAttribute
    {
        public string Description { get; set; }
        public DescriptionAttribute( string desc )
        {
            Description = desc;
        }
    }

}
