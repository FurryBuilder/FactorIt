using System.Collections.Specialized;

namespace FactorIt.Collections.Wrappers
{
	internal class AddCollectionChangedEventArgs : NotifyCollectionChangedEventArgs
	{
		public AddCollectionChangedEventArgs(object item, int index)
			: base(NotifyCollectionChangedAction.Add, item, index)
		{ }
	}
}