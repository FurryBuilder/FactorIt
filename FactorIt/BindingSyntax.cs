using System;

using FactorIt.Contracts;

namespace FactorIt
{
	public class BindingSyntax<TContract> : IBindingTo<TContract>, IBindingIntercept<TContract>
		where TContract : class
	{
		private readonly IServiceLocator _serviceLocator;
		private readonly Action<IRegistration> _register;

		private readonly Registration<TContract> _registration;

		public BindingSyntax([NotNull] IServiceLocator serviceLocator, [NotNull] Action<IRegistration> register)
		{
			_serviceLocator = serviceLocator;
			_register = register;

			_registration = new Registration<TContract>();
		}

		public IBindingIntercept<TContract> To<TService>([NotNull] Func<IServiceLocator, TService> factory)
			where TService : TContract
		{
			_registration.Source(() => factory.Invoke(_serviceLocator));

			_register.Invoke(_registration);

			return this;
		}

		public void Decorate([NotNull] Func<TContract, TContract> decorator)
		{
			_registration.Decorate(decorator);
		}

		public void Decorate([NotNull] Func<IServiceLocator, TContract, TContract> factory)
		{
			_registration.Decorate(c => factory.Invoke(_serviceLocator, c));
		}
	}
}