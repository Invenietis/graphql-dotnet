using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQL.Relay
{
    public class Edge<T>
    {
        public Edge( T node, ConnectionCursor cursor )
        {
            Node = node;
            Cursor = cursor;
        }

        public T Node { get; set; }
        public ConnectionCursor Cursor { get; set; }
    }

}
