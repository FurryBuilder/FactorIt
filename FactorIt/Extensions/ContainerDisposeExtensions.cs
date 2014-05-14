using System;
using System.Linq;

using FactorIt.Extensions;
using FactorIt.Contracts;

namespace FactorIt.Extensions
{
	public static class ContainerDisposeExtensions
	{
		// TODO: Use a special ServiceLocator instance to track all Resolve call to build a dependency tree that will be used to dispose services.
		public static void Dispose([NotNull] this IContainer source)
		{
			var disposables = source.Registrations
				.Select(registration => registration.Value.SelectOrDefault(v => v.IsValueCreated ? v.Value : null))
				.OfType<IDisposable>();

			foreach (var disposable in disposables)
			{
				disposable.Dispose();
			}
			
			source.Registrations.Clear();
			source.PostponedActions.Clear();

			source.UnregisterFromParent();

			foreach (var child in source.Children)
			{
				child.Dispose();
			}
		}
	}
}