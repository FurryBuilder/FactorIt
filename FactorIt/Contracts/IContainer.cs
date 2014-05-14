using System.Collections.Generic;

namespace FactorIt.Contracts
{
	public interface IContainer : IContainerNode
	{
		IDictionary<RegistrationKey, IRegistration> Registrations { get; }
		IDictionary<RegistrationKey, PostponedAction> PostponedActions { get; }
	}
}