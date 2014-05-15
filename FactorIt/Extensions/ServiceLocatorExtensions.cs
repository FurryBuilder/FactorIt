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

using FactorIt.Contracts;

namespace FactorIt.Extensions
{
	public static class ServiceLocatorExtensions
	{
		public static bool CanResolve<TContract>([NotNull] this IServiceLocator source)
			where TContract : class
		{
			return source.CanResolve<TContract>(null, Scope.Default);
		}

		public static bool CanResolve<TContract>([NotNull] this IServiceLocator source, [CanBeNull] string key)
			where TContract : class
		{
			return source.CanResolve<TContract>(key, Scope.Default);
		}

		public static bool CanResolve<TContract>([NotNull] this IServiceLocator source, Scope scope)
			where TContract : class
		{
			return source.CanResolve<TContract>(null, scope);
		}

		public static TContract Resolve<TContract>([NotNull] this IServiceLocator source)
			where TContract : class
		{
			return source.Resolve<TContract>(null, Scope.Default);
		}

		public static TContract Resolve<TContract>([NotNull] this IServiceLocator source, [CanBeNull] string key)
			where TContract : class
		{
			return source.Resolve<TContract>(key, Scope.Default);
		}

		public static TContract Resolve<TContract>([NotNull] this IServiceLocator source, Scope scope)
			where TContract : class
		{
			return source.Resolve<TContract>(null, scope);
		}

		public static TContract ResolveOrDefault<TContract>([NotNull] this IServiceLocator source)
			where TContract : class
		{
			return source.ResolveOrDefault(null, Scope.Default, () => default(TContract));
		}

		public static TContract ResolveOrDefault<TContract>([NotNull] this IServiceLocator source, [CanBeNull] string key)
			where TContract : class
		{
			return source.ResolveOrDefault(key, Scope.Default, () => default(TContract));
		}

		public static TContract ResolveOrDefault<TContract>([NotNull] this IServiceLocator source, Scope scope)
			where TContract : class
		{
			return source.ResolveOrDefault(null, scope, () => default(TContract));
		}

		public static TContract ResolveOrDefault<TContract>([NotNull] this IServiceLocator source, [NotNull] Func<TContract> defaultFactory)
			where TContract : class
		{
			return source.ResolveOrDefault(null, Scope.Default, defaultFactory);
		}

		public static TContract ResolveOrDefault<TContract>([NotNull] this IServiceLocator source, [CanBeNull] string key, [NotNull] Func<TContract> defaultFactory)
			where TContract : class
		{
			if (source == null)
			{
				return defaultFactory.Invoke();
			}

			return source.CanResolve<TContract>(key, Scope.Default)
				? source.Resolve<TContract>(key, Scope.Default)
				: defaultFactory.Invoke();
		}

		public static TContract ResolveOrDefault<TContract>([NotNull] this IServiceLocator source, Scope scope, [NotNull] Func<TContract> defaultFactory)
			where TContract : class
		{
			if (source == null)
			{
				return defaultFactory.Invoke();
			}

			return source.CanResolve<TContract>(null, scope)
				? source.Resolve<TContract>(null, scope)
				: defaultFactory.Invoke();
		}

		public static TContract ResolveOrDefault<TContract>([NotNull] this IServiceLocator source, [CanBeNull] string key, Scope scope, [NotNull] Func<TContract> defaultFactory)
			where TContract : class
		{
			if (source == null)
			{
				return defaultFactory.Invoke();
			}

			return source.CanResolve<TContract>(key, scope)
				? source.Resolve<TContract>(key, scope)
				: defaultFactory.Invoke();
		}

		public static void Postpone<TContract>([NotNull] this IServiceLocator source, [NotNull] Action<TContract> callback)
			where TContract : class
		{
			source.Postpone(null, Scope.Default, callback);
		}

		public static void Postpone<TContract>([NotNull] this IServiceLocator source, [CanBeNull] string key, [NotNull] Action<TContract> callback)
			where TContract : class
		{
			source.Postpone(key, Scope.Default, callback);
		}

		public static void Postpone<TContract>([NotNull] this IServiceLocator source, Scope scope, [NotNull] Action<TContract> callback)
			where TContract : class
		{
			source.Postpone(null, scope, callback);
		}
	}
}