using System.Linq;

using FactorIt.Extensions;
using FactorIt.Contracts;

namespace FactorIt.Extensions
{
	public static class ContainerContainsExtensions
	{
		public static bool Contains([NotNull] this IContainer source, [NotNull] RegistrationKey key, Scope scope)
		{
			return
				(scope.HasFlag(Scope.Local) && source.ContainsLocal(key)) ||
				(scope.HasFlag(Scope.Parent) && source.ContainsParent(key)) ||
				(scope.HasFlag(Scope.Children) && source.ContainsChildren(key));
		}

		internal static bool ContainsParent([NotNull] this IContainer source, [NotNull] RegistrationKey key)
		{
			return source.Parent.SelectOrDefault(p => p.ContainsLocal(key) || p.ContainsParent(key));
		}

		internal static bool ContainsLocal([NotNull] this IContainer source, [NotNull] RegistrationKey key)
		{
			return source.Registrations.ContainsKey(key);
		}

		internal static bool ContainsChildren([NotNull] this IContainer source, [NotNull] RegistrationKey key)
		{
			return source.Children.Any(c =>
			{
				if (c.ContainsLocal(key))
				{
					return true;
				}

				return c.ContainsChildren(key);
			});
		}
	}
}