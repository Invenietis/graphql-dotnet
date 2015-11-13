using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphQL.Types;

namespace GraphQL.Registration
{
    public class GraphItemResolutionContext
    {
        /// <summary>
        /// Gets or sets the field name.
        /// This property must have been set before building the field. 
        /// Otherwiser, an <see cref="InvalidOperationException"/> is thrown.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the field
        /// Can be null.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the type of a resolver that should be capable
        /// of resolving values of the property.
        /// Can be null.
        /// </summary>
        public Type ResolverType { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Type"/> of a graph type.
        /// </summary>
        public Type GraphType { get; set; }

        public object DefaultValue { get; internal set; }
        /// <summary>
        /// Gets ors sets wether the field should be nullable or not.
        /// It can be undetermined (null) during the resolution.
        /// </summary>
        public bool? IsNullable { get; set; }

        /// <summary>
        /// Gets or sets wether the field is an identifier or not.
        /// It can be undetermined (null) during the resolution.
        /// </summary>
        public bool? IsIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the query arguments
        /// </summary>
        public QueryArguments Arguments { get; set; }

        public string DeprecationReason { get; set; }

        public virtual FieldType BuildFieldType()
        {
            if( string.IsNullOrEmpty( Name ) )
                throw new InvalidOperationException( "Before building the FieldType, make sure the Name of the field is set on this resolution context." );

            var fieldType = new FieldType();
            fieldType.Name = Name;
            fieldType.Type = BuildGraphType();
            fieldType.Description = Description;
            fieldType.Resolve = CreateResolver();
            fieldType.DefaultValue = DefaultValue;
            fieldType.Arguments = Arguments;
            fieldType.DeprecationReason = DeprecationReason;
            return fieldType;
        }

        internal Type BuildGraphType()
        {
            if( IsIdentifier.HasValue && IsIdentifier.Value ) return typeof( IdGraphType );
            if( IsNullable.HasValue && IsNullable.Value ) return GraphType;

            return typeof( NonNullGraphType<> ).MakeGenericType( GraphType );
        }

        internal Func<ResolveFieldContext, object> CreateResolver()
        {
            if( ResolverType == null ) return null;

            IResolver resolver = (IResolver) Activator.CreateInstance( ResolverType );
            return resolver.Resolve;
        }
    }
}
