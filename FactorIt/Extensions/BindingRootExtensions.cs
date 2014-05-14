using FactorIt.Contracts;

namespace FactorIt.Extensions
{
	public static class BindingRootExtensions
	{
		public static IBindingTo<TContract> Bind<TContract>([NotNull] this IBindingRoot source)
			where TContract : class
		{
			return source.Bind<TContract>(null);
		}
	}
}