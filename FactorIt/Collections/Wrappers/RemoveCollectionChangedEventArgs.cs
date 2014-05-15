using System.Collections.Specialized;

namespace FactorIt.Collections.Wrappers
{
	internal class RemoveCollectionChangedEventArgs : NotifyCollectionChangedEventArgs
	{
		public RemoveCollectionChangedEventArgs(object item, int index)
			: base(NotifyCollectionChangedAction.Remove, item, index)
		{ }
	}
}