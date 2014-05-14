using System.Collections.Specialized;

namespace FactorIt.Collections.Wrappers
{
	public class ResetCollectionChangedEventArgs : NotifyCollectionChangedEventArgs
	{
		public ResetCollectionChangedEventArgs()
			: base(NotifyCollectionChangedAction.Reset)
		{ }
	}
}