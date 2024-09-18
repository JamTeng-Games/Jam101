using System;
using System.Collections.Generic;

namespace J.Core
{

    public class TypeComparer : IComparer<Type>
    {
        public int Compare(Type x, Type y)
        {
            // Handle null values.
            if (x == null)
                return (y == null) ? 0 : -1;
            if (y == null)
                return 1;

            // Compare the FullName of each type.
            return string.Compare(x.FullName, y.FullName, StringComparison.Ordinal);
        }
    }

    public static class TypeEx
    {
    }

}