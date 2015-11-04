using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQL.Relay
{

    public class Connection
    {
        public Connection()
        {
            Edges = new List<Edge>();
        }

        public IList<Edge> Edges { get; set; }

        public PageInfo PageInfo { get; set; }
    }
}
