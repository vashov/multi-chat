using System.Collections.Generic;
using System.Linq;

namespace MultiChat.Shared.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> values)
        {
            return values == null || !values.Any();
        }

        public static bool HasAny<T>(this IEnumerable<T> values) => !values.IsNullOrEmpty();
    }
}
