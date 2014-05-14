using System;

namespace FactorIt.Contracts
{
	public interface IBindingTo<TContract>
		where TContract : class
	{
		IBindingIntercept<TContract> To<TService>([NotNull] Func<IServiceLocator, TService> factory)
			where TService : TContract;
	}
}