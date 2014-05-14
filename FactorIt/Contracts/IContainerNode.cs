using System.Collections.Generic;

namespace FactorIt.Contracts
{
	public interface IContainerNode
	{
		IContainer Parent { get; }
		ISet<IContainer> Children { get; }

		void RegisterOnParent();
		void UnregisterFromParent();
	}
}