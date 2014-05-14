using System;
using System.Collections.Generic;

namespace FactorIt
{
	public class PostponedAction
	{
		private readonly Action _cleanup;

		private readonly List<Action<object>> _actions = new List<Action<object>>();

		public PostponedAction([NotNull] Action cleanup)
		{
			_cleanup = cleanup;
		}

		public void Postpone([NotNull] Action<object> callback)
		{
			_actions.Add(callback);
		}

		public void Invoke([NotNull] object value)
		{
			foreach (var a in _actions)
			{
				a.Invoke(value);
			}

			_cleanup.Invoke();
		}
	}
}