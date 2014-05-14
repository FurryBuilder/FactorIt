using System;
using System.Collections.Generic;

namespace FactorIt.Extensions
{
	public static class ObjectExtensions
	{
		public static bool IsNullOrDefault<TSource>([CanBeNull] this TSource source, [CanBeNull] IEqualityComparer<TSource> valueTypeComparer = null)
		{
			return
				ReferenceEquals(source, null) ||
				valueTypeComparer == null
					? Equals(source, default(TSource))
					: valueTypeComparer.Equals(source, default(TSource));
		}

		public static TResult SelectOrDefault<TSource, TResult>([CanBeNull] this TSource source, [NotNull] Func<TSource, TResult> selector, [CanBeNull] IEqualityComparer<TSource> valueTypeComparer = null)
		{
			return source.IsNullOrDefault(valueTypeComparer)
				? default(TResult)
				: selector.Invoke(source);
		}

		public static TResult SelectOrDefault<TSource, TResult>(
			[CanBeNull] this TSource source,
			[NotNull] Func<TSource, TResult> selector,
			[NotNull] TResult defaultValue,
			[CanBeNull] IEqualityComparer<TSource> valueTypeComparer = null
		)
		{
			return source.IsNullOrDefault(valueTypeComparer)
				? defaultValue
				: selector.Invoke(source);
		}

		public static void Maybe<TSource>([CanBeNull] this TSource source, [NotNull] Action<TSource> maybe, [CanBeNull] IEqualityComparer<TSource> valueTypeComparer = null)
		{
			if (!source.IsNullOrDefault(valueTypeComparer))
			{
				maybe.Invoke(source);
			}
		}

		/// <summary>
		/// Cast an input value to a target type and returns the new value.
		/// Simiar to the "as" operator but works on value types.
		/// </summary>
		/// <typeparam name="TResult">The type to cast to</typeparam>
		/// <param name="source">The source value to cast</param>
		/// <returns>The value casted into the specified type or the default value for the destination type if the types are incompatibles</returns>
		public static TResult As<TResult>([CanBeNull] this object source)
		{
			return source is TResult
				? (TResult)source
				: default(TResult);
		}
	}
}