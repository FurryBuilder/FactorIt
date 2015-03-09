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
using JetBrains.Annotations;

namespace FactorIt.Extensions
{
    public static class ContainerRegisterExtensions
    {
        /// <summary>
        /// Adds a registration to a container and bind it to a registration key.
        /// </summary>
        /// <param name="container">The container to update</param>
        /// <param name="key">The key to use for this registration</param>
        /// <param name="registration">The registration to store in the container</param>
        /// <exception cref="InvalidOperationException">The key is already present within the container.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="key" /> is null.</exception>
        /// <exception cref="NotSupportedException">The <see cref="T:System.Collections.Generic.IDictionary`2" /> is read-only.</exception>
        public static void Register(
            [NotNull] this IContainer container,
            [NotNull] RegistrationKey key,
            [NotNull] IRegistration registration)
        {
            if (container.Registrations.ContainsKey(key))
            {
                throw new InvalidOperationException(Container.Constants.ContractAlreadyRegistered.Format(key));
            }

            container.Registrations.Add(key, registration);

            container.PostponedActions
                .FirstOrDefault(RegistrationKey.Comparer.Default, key)
                .Maybe(a => a.Invoke(registration.Value));
        }
    }
}