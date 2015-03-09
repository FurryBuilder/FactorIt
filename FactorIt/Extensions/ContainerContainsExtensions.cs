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

using System.Linq;
using FactorIt.Contracts;
using FluffIt;
using JetBrains.Annotations;

namespace FactorIt.Extensions
{
    public static class ContainerContainsExtensions
    {
        public static bool Contains([NotNull] this IContainer container, [NotNull] RegistrationKey key, Scope scope)
        {
            return
                (scope.HasFlag(Scope.Local) && container.ContainsLocal(key)) ||
                (scope.HasFlag(Scope.Parent) && container.ContainsParent(key)) ||
                (scope.HasFlag(Scope.Children) && container.ContainsChildren(key));
        }

        internal static bool ContainsParent([NotNull] this IContainer container, [NotNull] RegistrationKey key)
        {
            return container.Parent.SelectOrDefault(p => p.ContainsLocal(key) || p.ContainsParent(key));
        }

        internal static bool ContainsLocal([NotNull] this IContainer container, [NotNull] RegistrationKey key)
        {
            return container.Registrations.ContainsKey(key);
        }

        internal static bool ContainsChildren([NotNull] this IContainer container, [NotNull] RegistrationKey key)
        {
            return container.Children.Any(c => c.ContainsLocal(key) || c.ContainsChildren(key));
        }
    }
}