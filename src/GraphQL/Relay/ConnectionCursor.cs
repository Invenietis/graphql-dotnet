using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQL.Relay
{

    public class ConnectionCursor
    {

        public string Value { get; set; }

        public ConnectionCursor( string value )
        {
            Value = value;
        }

        public override bool Equals( object o )
        {
            if( this == o ) return true;

            ConnectionCursor that = (ConnectionCursor) o;

            if( Value != null ? !Value.Equals( that.Value ) : that.Value != null ) return false;

            return true;
        }

        public override int GetHashCode()
        {
            return Value != null ? Value.GetHashCode() : 0;
        }

        public override string ToString()
        {
            return Value;
        }
    }

}
