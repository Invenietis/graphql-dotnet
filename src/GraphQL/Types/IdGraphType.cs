using System;

namespace GraphQL.Types
{
    public class IdGraphType : ScalarGraphType
    {
        public IdGraphType()
        {
            Name = "ID";
        }

        public override object Coerce(object value)
        {
            return value.ToString();
        }
    }
}
