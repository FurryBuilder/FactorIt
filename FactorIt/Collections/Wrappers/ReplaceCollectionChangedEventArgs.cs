using System.Collections.Specialized;

namespace FactorIt.Collections.Wrappers
{
	internal class ReplaceCollectionChangedEventArgs : NotifyCollectionChangedEventArgs
	{
		public ReplaceCollectionChangedEventArgs(object newItem, object oldItem, int index)
			: base(NotifyCollectionChangedAction.Replace, newItem, oldItem, index)
		{ }
	}
}