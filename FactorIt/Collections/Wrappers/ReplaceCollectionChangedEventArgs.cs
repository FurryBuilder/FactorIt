using System.Collections.Specialized;

namespace FactorIt.Collections.Wrappers
{
	public class ReplaceCollectionChangedEventArgs : NotifyCollectionChangedEventArgs
	{
		public ReplaceCollectionChangedEventArgs(object newItem, object oldItem, int index)
			: base(NotifyCollectionChangedAction.Replace, newItem, oldItem, index)
		{ }
	}
}