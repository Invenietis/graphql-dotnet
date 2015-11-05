using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQL.Relay
{

    public class Connection<T>
    {
        public Connection()
        {
            Edges = new List<Edge<T>>();
        }

        public IList<Edge<T>> Edges { get; set; }

        public PageInfo PageInfo { get; set; }
    }
}
