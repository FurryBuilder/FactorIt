namespace FactorIt.Contracts
{
	public interface IBindingRoot
	{
		IBindingTo<TContract> Bind<TContract>([CanBeNull] string key)
			where TContract : class;
	}
}