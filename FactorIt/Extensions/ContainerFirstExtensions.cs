using System;

using FactorIt.Extensions;
using FactorIt.Contracts;

namespace FactorIt.Extensions
{
	public static class ContainerFirstExtensions
	{
		public static IRegistration First([NotNull] this IContainer source, [NotNull] RegistrationKey key, Scope scope)
		{
			if (scope.HasFlag(Scope.Local) && source.ContainsLocal(key))
			{
				return source.FirstLocal(key);
			}

			if (scope.HasFlag(Scope.Parent) && source.ContainsParent(key))
			{
				return source.FirstParent(key);
			}

			if (scope.HasFlag(Scope.Children) && source.ContainsChildren(key))
			{
				return source.FirstChildren(key);
			}

			throw new InvalidOperationException(string.Format(Container.Constants.ContractNotRegistered, key));
		}

		private static IRegistration FirstParent([NotNull] this IContainer source, [NotNull] RegistrationKey key)
		{
			return source.Parent.SelectOrDefault(p => p.ContainsLocal(key) ? p.FirstLocal(key) : p.FirstParent(key));
		}

		private static IRegistration FirstLocal([NotNull] this IContainer source, [NotNull] RegistrationKey key)
		{
			return source.Registrations[key];
		}

		private static IRegistration FirstChildren([NotNull] this IContainer source, [NotNull] RegistrationKey key)
		{
			foreach (var c in source.Children)
			{
				if (c.ContainsLocal(key))
				{
					return c.FirstLocal(key);
				}

				return c.FirstChildren(key);
			}

			throw new InvalidOperationException(string.Format(Container.Constants.ContractNotRegistered, key));
		}
	}
}