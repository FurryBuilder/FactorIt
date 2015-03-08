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
using JetBrains.Annotations;

namespace FactorIt
{
    /// <summary>
    /// Handles contract binding and interception.
    /// </summary>
    /// <typeparam name="TContract">The type of contract to bind and intercept.</typeparam>
    public class BindingSyntax<TContract> : IBindingTo<TContract>, IBindingIntercept<TContract>
        where TContract : class
    {
        private readonly Action<IRegistration> _register;
        private readonly Registration<TContract> _registration;
        private readonly IServiceLocator _serviceLocator;

        internal BindingSyntax([NotNull] IServiceLocator serviceLocator, [NotNull] Action<IRegistration> register)
        {
            _serviceLocator = serviceLocator;
            _register = register;

            _registration = new Registration<TContract>();
        }

        public void Decorate([NotNull] Func<TContract, TContract> decorator)
        {
            _registration.Decorate(decorator);
        }

        public void Decorate([NotNull] Func<IServiceLocator, TContract, TContract> factory)
        {
            _registration.Decorate(c => factory.Invoke(_serviceLocator, c));
        }

        public IBindingIntercept<TContract> To<TService>([NotNull] Func<IServiceLocator, TService> factory)
            where TService : TContract
        {
            _registration.Source(() => factory.Invoke(_serviceLocator));

            _register.Invoke(_registration);

            return this;
        }
    }
}