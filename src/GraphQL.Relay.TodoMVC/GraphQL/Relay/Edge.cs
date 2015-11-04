using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQL.Relay
{
    public class Edge
    {
        public Edge( object node, ConnectionCursor cursor )
        {
            Node = node;
            Cursor = cursor;
        }

        public object Node { get; set; }
        public ConnectionCursor Cursor { get; set; }
    }

}
