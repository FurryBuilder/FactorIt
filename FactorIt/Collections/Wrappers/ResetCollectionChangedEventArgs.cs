using System.Collections.Specialized;

namespace FactorIt.Collections.Wrappers
{
	internal class ResetCollectionChangedEventArgs : NotifyCollectionChangedEventArgs
	{
		public ResetCollectionChangedEventArgs()
			: base(NotifyCollectionChangedAction.Reset)
		{ }
	}
}