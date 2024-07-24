using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace MiP.ObjectDump.Reflection
{
    public class ObjectReferenceEqualityComparer : IEqualityComparer<object>
    {
        /// <inheritdoc />
        bool IEqualityComparer<object>.Equals(object x, object y)
        {
            return ReferenceEquals(x, y);
        }

        /// <inheritdoc />
        public int GetHashCode(object obj)
        {
            return RuntimeHelpers.GetHashCode(obj);
        }
    }
}