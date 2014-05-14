using System.Collections.Specialized;

namespace FactorIt.Collections.Wrappers
{
	public class AddCollectionChangedEventArgs : NotifyCollectionChangedEventArgs
	{
		public AddCollectionChangedEventArgs(object item, int index)
			: base(NotifyCollectionChangedAction.Add, item, index)
		{ }
	}
}