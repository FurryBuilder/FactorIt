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