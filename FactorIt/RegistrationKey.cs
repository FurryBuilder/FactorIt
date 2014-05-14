using System;

using FactorIt.Patterns;

namespace FactorIt
{
	/// <summary>
	/// Represent a registration in a dictionary based on a type of contract
	/// and a sub-set key.
	/// </summary>
	public class RegistrationKey : KeyBase<RegistrationKey>
	{
		public class Comparer : KeyComparerBase<Comparer>
		{
			public override bool Equals([CanBeNull] RegistrationKey x, [CanBeNull] RegistrationKey y)
			{
				return
					string.Equals(x.Key, y.Key) &&
					x.Type == y.Type;
			}

			public override int GetHashCode([CanBeNull] RegistrationKey obj)
			{
				unchecked
				{
					return
						((obj.Key != null ? obj.Key.GetHashCode() : 0) * 397) ^
						obj.Type.GetHashCode();
				}
			}
		}

		public string Key { get; private set; }
		public Type Type { get; private set; }

		private RegistrationKey([CanBeNull] string key, [NotNull] Type type)
		{
			Key = key;
			Type = type;
		}

		public static RegistrationKey From<T>([CanBeNull] string key)
		{
			return new RegistrationKey(key, typeof(T));
		}

		protected override string GetStringValues()
		{
			return
				"Key=\"" + Key + "\", " +
				"Type=\"" + Type + "\"";
		}
	}
}