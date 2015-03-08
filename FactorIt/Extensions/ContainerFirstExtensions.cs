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
    public static class ContainerFirstExtensions
    {
        /// <summary>
        /// Returns the first registration matching the specified key and
        /// within the provided scope.
        /// </summary>
        /// <param name="container">The container to use as the entry to the hierarchy</param>
        /// <param name="key">The registration key to look for</param>
        /// <param name="scope">The scope in which the key will be looked for</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">No contract matching the provided key and within the specified scope can be found on this container.</exception>
        public static IRegistration First([NotNull] this IContainer container, [NotNull] RegistrationKey key, Scope scope)
        {
            if (scope.HasFlag(Scope.Local) && container.ContainsLocal(key))
            {
                return container.FirstLocal(key);
            }

            if (scope.HasFlag(Scope.Parent) && container.ContainsParent(key))
            {
                return container.FirstParent(key);
            }

            if (scope.HasFlag(Scope.Children) && container.ContainsChildren(key))
            {
                return container.FirstChildren(key);
            }

            throw new InvalidOperationException(string.Format(Container.Constants.ContractNotRegistered, key));
        }

        private static IRegistration FirstParent([NotNull] this IContainerNode container, [NotNull] RegistrationKey key)
        {
            if (container.Parent != null)
            {
                return container.Parent.ContainsLocal(key)
                    ? container.Parent.FirstLocal(key)
                    : container.Parent.FirstParent(key);
            }

            throw new InvalidOperationException(string.Format(Container.Constants.ContractNotRegistered, key));
        }

        private static IRegistration FirstLocal([NotNull] this IContainer container, [NotNull] RegistrationKey key)
        {
            IRegistration registration;
            if (container.Registrations.TryGetValue(key, out registration))
            {
                return registration;
            }

            throw new InvalidOperationException(string.Format(Container.Constants.ContractNotRegistered, key));
        }

        private static IRegistration FirstChildren([NotNull] this IContainerNode container, [NotNull] RegistrationKey key)
        {
            foreach (var c in container.Children)
            {
                return c.ContainsLocal(key)
                    ? c.FirstLocal(key)
                    : c.FirstChildren(key);
            }

            throw new InvalidOperationException(string.Format(Container.Constants.ContractNotRegistered, key));
        }
    }
}