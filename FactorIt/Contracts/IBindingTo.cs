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

namespace FactorIt.Contracts
{
    /// <summary>
    /// Second step in the fluent binding flow. Associated a service factory to
    /// a specific contract.
    /// </summary>
    /// <typeparam name="TContract">Type of the bound contract.</typeparam>
    public interface IBindingTo<TContract>
        where TContract : class
    {
        /// <summary>
        /// Associate a service to an entry (contract and key pair) in the container.
        /// </summary>
        /// <typeparam name="TService">Type of the service to associate</typeparam>
        /// <param name="factory">Factory used when creating an instance of the service.</param>
        /// <returns>An object to handle the next step in the binding flow.</returns>
        IBindingIntercept<TContract> To<TService>([NotNull] Func<IServiceLocator, TService> factory)
            where TService : TContract;
    }
}