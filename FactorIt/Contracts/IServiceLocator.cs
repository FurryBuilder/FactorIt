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
using JetBrains.Annotations;

namespace FactorIt.Contracts
{
    /// <summary>
    /// Provides high level query interactions on a container.
    /// </summary>
    public interface IServiceLocator
    {
        /// <summary>
        /// Returns wether the service locator can provides an instance of a
        /// contract matching the specified key and within the specified scope.
        /// </summary>
        /// <typeparam name="TContract">Type of the contract to provide</typeparam>
        /// <param name="key">Key associated to the contract</param>
        /// <param name="scope">The range that should be used when looking for the contract.</param>
        /// <returns></returns>
        bool CanResolve<TContract>([CanBeNull] string key, Scope scope)
            where TContract : class;

        /// <summary>
        /// Provides an instance of a contract matching the specified key and
        /// within the specified scope.
        /// </summary>
        /// <typeparam name="TContract">Type of the contract to provide</typeparam>
        /// <param name="key">Key associated to the contract</param>
        /// <param name="scope">The range that should be used when looking for the contract.</param>
        TContract Resolve<TContract>([CanBeNull] string key, Scope scope)
            where TContract : class;

        /// <summary>
        /// Schedule a callback to be executed once the service locator can
        /// provide an instance of a contract matching the specified key and
        /// within the specified scope.
        /// </summary>
        /// <typeparam name="TContract">Type of the contract to provide</typeparam>
        /// <param name="key">Key associated to the contract</param>
        /// <param name="scope">The range that should be used when looking for the contract.</param>
        /// <param name="callback">A action to execute requiring a specific service instance that is not yet available.</param>
        void Postpone<TContract>([CanBeNull] string key, Scope scope, [NotNull] Action<TContract> callback)
            where TContract : class;
    }
}