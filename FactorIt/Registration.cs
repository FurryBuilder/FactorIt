using System;

using FactorIt.Extensions;
using FactorIt.Contracts;

namespace FactorIt
{
	public class Registration<TContract> : IRegistration
		where TContract : class
	{
		private readonly Lazy<object> _registration;

		private Func<TContract> _factory;
		private Func<TContract, TContract> _decorator;

		public Registration()
		{
			_registration = new Lazy<object>(() => _decorator.SelectOrDefault(d => d.Invoke(_factory.Invoke()), _factory.Invoke()));
		}

		public void Source([NotNull] Func<TContract> factory)
		{
			_factory = factory;
		}

		public void Decorate([NotNull] Func<TContract, TContract> decorator)
		{
			_decorator = decorator;
		}

		public object Value { get { return _registration.Value; } }
		public bool IsValueCreated { get { return _registration.IsValueCreated; } }
	}
}