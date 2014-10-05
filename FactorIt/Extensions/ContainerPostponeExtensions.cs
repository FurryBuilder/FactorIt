//////////////////////////////////////////////////////////////////////////////////
//
// The MIT License (MIT)
//
// Copyright (c) 2014 Furry Builder
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//
//////////////////////////////////////////////////////////////////////////////////

using System;

using FactorIt.Contracts;
using FluffIt;

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