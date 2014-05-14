using System;

namespace FactorIt.Contracts
{
	public interface IServiceLocator
	{
		bool CanResolve<TContract>([CanBeNull] string key, Scope scope)
			where TContract : class;
		TContract Resolve<TContract>([CanBeNull] string key, Scope scope)
			where TContract : class;
		void Postpone<TContract>([CanBeNull] string key, Scope scope, [NotNull] Action<TContract> callback)
			where TContract : class;
	}
}