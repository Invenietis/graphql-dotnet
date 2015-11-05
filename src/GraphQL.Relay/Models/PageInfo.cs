using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQL.Relay
{
    public class PageInfo
    {
        public ConnectionCursor StartCursor { get; set; }
        public ConnectionCursor EndCursor { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
    }
}
