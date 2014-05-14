using System;

namespace FactorIt.Contracts
{
	public interface IBindingIntercept<TContract>
		where TContract : class
	{
		void Decorate([NotNull] Func<TContract, TContract> factory);

		void Decorate([NotNull] Func<IServiceLocator, TContract, TContract> factory);
	}
}