using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQL.Relay
{
    public class ResolvedGlobalId
    {
        public ResolvedGlobalId( string type, string id )
        {
            this.Type = type;
            this.Id = id;
        }

        public string Type { get; set; }
        public string Id { get; set; }

        public static string ToGlobalId( string type, string id )
        {
            return Base64.ToBase64( type + ":" + id );
        }

        public static ResolvedGlobalId FromGlobalId( string globalId )
        {
            var split = Base64.FromBase64( globalId ).Split(new char[] { ':' }, 2);
            return new ResolvedGlobalId( split[0], split[1] );
        }
    }

}
