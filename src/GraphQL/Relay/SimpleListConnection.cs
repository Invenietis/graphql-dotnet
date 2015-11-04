using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Types;

namespace GraphQL.Relay
{

    public class SimpleListConnection<T>
    {
        private const string DUMMY_CURSOR_PREFIX = "simple-cursor";

        readonly IList<T> data;
        private static Connection<T> EmptyConnection { get; } = new Connection<T> { PageInfo = new PageInfo() };

        public SimpleListConnection( List<T> data )
        {
            this.data = data;
        }

        private List<Edge<T>> buildEdges()
        {
            List<Edge<T>> edges = new List<Edge<T>>();
            int ix = 0;
            foreach( var o in data )
            {
                edges.Add( new Edge<T>( o, new ConnectionCursor( CreateCursor( ix++ ) ) ) );
            }
            return edges;
        }


        public object Resolve( ResolveFieldContext environment )
        {
            var edges = buildEdges();

            int afterOffset = GetOffsetFromCursor(environment.Arguments["after"], -1);
            int begin = Math.Max(afterOffset, -1) + 1;
            int beforeOffset = GetOffsetFromCursor(environment.Arguments["before"], edges.Count);
            int end = Math.Min(beforeOffset, edges.Count);

            edges = edges.GetRange( begin, end );
            if( edges.Count == 0 )
            {
                return EmptyConnection;
            }


            var first = (int?)environment.Arguments["first"];
            var last = (int?)environment.Arguments["last"];

            ConnectionCursor firstPresliceCursor = edges[0].Cursor;
            ConnectionCursor lastPresliceCursor = edges[edges.Count - 1].Cursor;

            if( first.HasValue )
            {
                edges = edges.GetRange( 0, first.Value <= edges.Count ? first.Value : edges.Count );
            }
            if( last.HasValue )
            {
                edges = edges.GetRange( edges.Count - last.Value, edges.Count );
            }

            if( edges.Count == 0 )
            {
                return EmptyConnection;
            }

            var firstEdge = edges[0];
            var lastEdge = edges[edges.Count - 1];

            PageInfo pageInfo = new PageInfo();
            pageInfo.StartCursor = firstEdge.Cursor;
            pageInfo.EndCursor = lastEdge.Cursor;
            pageInfo.HasPreviousPage = !firstEdge.Cursor.Equals( firstPresliceCursor );
            pageInfo.HasNextPage = !lastEdge.Cursor.Equals( lastPresliceCursor );

            var connection = new Connection<T>
            {
                Edges = edges,
                PageInfo = pageInfo
            };
            return connection;
        }


        //public ConnectionCursor cursorForObjectInConnection( object o )
        //{
        //    int index = data.IndexOf(object);
        //    String cursor = CreateCursor(index);
        //    return new ConnectionCursor( cursor );
        //}


        private int GetOffsetFromCursor( object cursor, int defaultValue )
        {
            if( cursor == null ) return defaultValue;
            var s = Base64.FromBase64(cursor.ToString() );
            return int.Parse( s.Substring( DUMMY_CURSOR_PREFIX.Length ) );
        }

        private String CreateCursor( int offset )
        {
            return Base64.ToBase64( DUMMY_CURSOR_PREFIX + offset.ToString() );
        }
    }

}
