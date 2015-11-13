using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphQL.Types;

namespace GraphQL.Registration
{
    public class ItemMetadata
    {
        public string Name { get; set; }

        public Type ItemType { get; set; }
        public IReadOnlyCollection<IGraphAttribute> Attributes { get; set; }
        internal bool HasAttribute<T>() where T : IGraphAttribute
        {
            return Attributes.OfType<T>().Any();
        }

        internal T GetAttribute<T>() where T : IGraphAttribute
        {
            return Attributes.OfType<T>().FirstOrDefault();
        }

        internal IReadOnlyCollection<T> GetAttributes<T>()
        {
            return Attributes.OfType<T>().ToArray();
        }
    }

    public sealed class Argument : ItemMetadata
    {
    }

    public sealed class MethodMetadata : ItemMetadata
    {
        public IReadOnlyCollection<Argument> Arguments { get; set; }
    }

}
