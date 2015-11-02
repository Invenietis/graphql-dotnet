using System.Collections.Generic;
using GraphQL.Language;
using System.Threading;

namespace GraphQL.Types
{
    public class ResolveFieldContext<T>
    {
        public Field FieldAst { get; set; }

        public FieldType FieldDefinition { get; set; }

        public ObjectGraphType ParentType { get; set; }

        public Dictionary<string, object> Arguments { get; set; }

        public T Source { get; set; }

        public Schema Schema { get; set; }

        public CancellationToken CancellationToken { get; set; }
    }

    public class ResolveFieldContext : ResolveFieldContext<object>
    {
        public ResolveFieldContext<TDestination> Cast<TDestination>()
            where TDestination : class
        {
            return new ResolveFieldContext<TDestination>
            {
                Source = this.Source as TDestination,
                Arguments = this.Arguments,
                FieldAst = this.FieldAst,
                FieldDefinition = this.FieldDefinition,
                ParentType = this.ParentType,
                Schema = this.Schema
            };
        }
    }

}
