using System;

using FactorIt.Extensions;
using FactorIt.Contracts;

namespace FactorIt.Extensions
{
	public static class ContainerRegisterExtensions
	{
		public static void Register([NotNull] this IContainer source, [NotNull] RegistrationKey key, [NotNull] IRegistration registration)
		{
			if (source.Registrations.ContainsKey(key))
			{
				throw new InvalidOperationException(string.Format(Container.Constants.ContractAlreadyRegistered, key));
			}

			source.Registrations.Add(key, registration);

			source.PostponedActions
				.FirstOrDefault(RegistrationKey.Comparer.Default, key)
				.Maybe(a => a.Invoke(registration.Value));
		}
	}
}