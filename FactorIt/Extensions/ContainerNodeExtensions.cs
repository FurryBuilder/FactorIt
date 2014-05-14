using FactorIt.Extensions;
using FactorIt.Contracts;

namespace FactorIt.Extensions
{
	public static class ContainerNodeExtensions
	{
		internal static IContainer GetRootNode([NotNull] this IContainer source)
		{
			return source.Parent.SelectOrDefault(p => p.GetRootNode(), source);
		}
	}
}