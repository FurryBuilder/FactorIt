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
    /// Third step in the fluent binding flow. Customize the binding properties.
    /// </summary>
    /// <typeparam name="TContract"></typeparam>
    public interface IBindingIntercept<TContract>
        where TContract : class
    {
        /// <summary>
        /// Adds an interception point to the resolving process of a contract.
        /// </summary>
        /// <param name="factory">Factory that decorates a provided instance</param>
        void Decorate([NotNull] Func<TContract, TContract> factory);

        /// <summary>
        /// Adds an interception point to the resolving process of a contract.
        /// </summary>
        /// <param name="factory">Factory that decorates a provided instance</param>
        void Decorate([NotNull] Func<IServiceLocator, TContract, TContract> factory);
    }
}