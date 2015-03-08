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

namespace FactorIt.Contracts
{
    /// <summary>
    /// Provides high level binding operations on an associated container. First
    /// step in the container's fluent binding flow.
    /// </summary>
    public interface IBindingRoot
    {
        /// <summary>
        /// Provides a way to associate a specified contract and key to a service factory.
        /// </summary>
        /// <typeparam name="TContract">The type of the contract to bind</typeparam>
        /// <param name="key">A grouping identifier to differenciate different services using the same bound contract.</param>
        /// <returns>An object to handle the next step in the binding flow.</returns>
        IBindingTo<TContract> Bind<TContract>([CanBeNull] string key)
            where TContract : class;
    }
}