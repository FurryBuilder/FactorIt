using System.Collections.Generic;
using System.Linq;

namespace FactorIt.Extensions
{
	internal static class EnumerableExtensions
	{
		public static TValue FirstOrDefault<TKey, TValue>([NotNull] this IEnumerable<KeyValuePair<TKey, TValue>> source, [NotNull] IEqualityComparer<TKey> comparer, [CanBeNull] TKey needle)
		{
			return source
				.Where(kv => comparer.Equals(kv.Key, needle))
				.Select(kv => kv.Value)
				.FirstOrDefault();
		}
	}
}