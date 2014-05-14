using System;

using FactorIt.Extensions;
using FactorIt.Contracts;

namespace FactorIt.Extensions
{
	public static class ContainerPostponeExtensions
	{
		public static void Postpone([NotNull] this IContainer source, [NotNull] RegistrationKey key, Scope scope, [NotNull] Action<object> callback)
		{
			if (source.Contains(key, scope))
			{
				callback.Invoke(source.First(key, scope).Value);
				return;
			}

			if (scope.HasFlag(Scope.Local))
			{
				source.PostponeLocal(key, callback);
			}

			if (scope.HasFlag(Scope.Parent))
			{
				source.PostponeParent(key, callback);
			}

			if (scope.HasFlag(Scope.Children))
			{
				source.PostponeChildren(key, callback);
			}
		}

		private static void PostponeParent([NotNull] this IContainer source, [NotNull] RegistrationKey key, [NotNull] Action<object> callback)
		{
			source.Parent.Maybe(p => p.PostponeLocal(key, callback));
		}

		private static void PostponeLocal([NotNull] this IContainer source, [NotNull] RegistrationKey key, [NotNull] Action<object> callback)
		{
			source.GetPostponingHandler(key).Postpone(callback);
		}

		private static PostponedAction GetPostponingHandler([NotNull] this IContainer source, [NotNull] RegistrationKey key)
		{
			PostponedAction postponedAction;
			if (!source.PostponedActions.TryGetValue(key, out postponedAction))
			{
				postponedAction = new PostponedAction(() => source.PrunePostponedActionsFromHierarchy(key));
				source.PostponedActions.Add(key, postponedAction);
			}

			return postponedAction;
		}

		private static void PostponeChildren([NotNull] this IContainer source, [NotNull] RegistrationKey key, [NotNull] Action<object> callback)
		{
			foreach (var child in source.Children)
			{
				child.PostponeLocal(key, callback);

				child.PostponeChildren(key, callback);
			}
		}

		private static void PrunePostponedActionsFromHierarchy([NotNull] this IContainer source, [NotNull] RegistrationKey key)
		{
			source.GetRootNode().RemovePostponedActionsFromChildren(key);
		}

		private static void RemovePostponedActionsFromChildren([NotNull] this IContainer node, [NotNull] RegistrationKey key)
		{
			PostponedAction action;
			if (node.PostponedActions.TryGetValue(key, out action))
			{
				node.PostponedActions.Remove(key);
			}

			foreach (var child in node.Children)
			{
				child.RemovePostponedActionsFromChildren(key);
			}
		}
	}
}