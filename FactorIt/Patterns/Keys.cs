using System;
using System.Collections.Generic;

namespace FactorIt.Patterns
{
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2237:MarkISerializableTypesWithSerializable", Justification = "Not using built it serialization to stay cross-platform friendly")]
	public class RequireEqualityComparerException<TSelf> : Exception
	{
		public static class Constants
		{
			public const string UseEqualityComparer = "An IEqualityComparer implementation is available for the type: {0} and must be used instead.";
		}

		public RequireEqualityComparerException()
			: base(string.Format(Constants.UseEqualityComparer, typeof(TSelf).FullName))
		{ }
	}

	public abstract class KeyBase<TKey>
	{
		public abstract class KeyComparerBase<TComparer> : DefaultProvider<TComparer>, IEqualityComparer<TKey>
			where TComparer : new()
		{
			public abstract bool Equals([NotNull] TKey x, [NotNull] TKey y);

			public abstract int GetHashCode([NotNull] TKey obj);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations", Justification = "The Key pattern requires the use of an IEqualityComparer. Equals should never be called directly")]
		public override bool Equals([NotNull] object obj)
		{
			throw new RequireEqualityComparerException<TKey>();
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations", Justification = "The Key patterns requires the use of an IEqualityComparer. GetHashCode should never be called directly")]
		public override int GetHashCode()
		{
			throw new RequireEqualityComparerException<TKey>();
		}

		protected abstract string GetStringValues();

		public override string ToString()
		{
			return GetStringValues();
		}
	}
}