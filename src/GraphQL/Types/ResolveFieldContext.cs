using System.Collections.Generic;
using GraphQL.Language;
using System.Threading;

namespace GraphQL.Types
{
    public class ResolveFieldContext
    {
        public Field FieldAst { get; set; }

        public FieldType FieldDefinition { get; set; }

        public ObjectGraphType ParentType { get; set; }

        public Dictionary<string, object> Arguments { get; set; }

        public object Source { get; set; }

        public object Root { get; set; }

        public Schema Schema { get; set; }

        public CancellationToken CancellationToken { get; set; }
    }

    public class ResolveFieldContext<T> : ResolveFieldContext
    {
    }

}
