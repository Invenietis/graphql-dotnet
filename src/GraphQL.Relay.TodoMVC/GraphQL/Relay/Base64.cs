using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphQL.Relay
{
    public class Base64
    {
        public static string FromBase64( string s )
        {
            return Encoding.UTF8.GetString( Convert.FromBase64String( s ) );
        }

        public static string ToBase64( string s )
        {
            return Convert.ToBase64String( Encoding.UTF8.GetBytes( s ) );
        }
    }
}
