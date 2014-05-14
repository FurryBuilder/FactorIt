namespace FactorIt.Patterns
{
	public abstract class DefaultProvider<T>
		where T : new()
	{
		public static readonly T Default = new T();
	}
}