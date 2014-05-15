//////////////////////////////////////////////////////////////////////////////////
//
// The MIT License (MIT)
//
// Copyright (c) 2014 Furry Builder
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//
//////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;

namespace FactorIt.Extensions
{
	internal static class ObjectExtensions
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