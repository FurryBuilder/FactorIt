﻿//////////////////////////////////////////////////////////////////////////////////
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
using System.Collections.Generic;
using System.Diagnostics;
using FactorIt.Contracts;
using FactorIt.Extensions;
using FluffIt;
using JetBrains.Annotations;

namespace FactorIt
{
    /// <summary>
    /// The main container class. Handles the life time of services and dispatches
    /// registration and service location events.
    /// </summary>
    public sealed class Container : IContainer, IBindingRoot, IServiceLocator, IDisposable
    {
        public static class Constants
        {
            public const string IncompatibleServiceLocator =
                "Trying to create a child container from an incompatible IServiceLocator. Please use an IServiceLocator coming from this IoC container library.";

            public const string ContractAlreadyRegistered = "Contract with key \"{0}\" already registered";
            public const string ContractNotRegistered = "Contract with key \"{0}\" not registered";

            public const string FinalizerCalled =
                "An instance of scope has been finalized without being disposed. It's services and children Scopes will not be disposed.";
        }

        private readonly HashSet<IContainer> _children =
            new HashSet<IContainer>();

        private readonly IContainer _parent;

        private readonly Dictionary<RegistrationKey, PostponedAction> _postponedActions =
            new Dictionary<RegistrationKey, PostponedAction>(RegistrationKey.Comparer.Default);

        private readonly Dictionary<RegistrationKey, IRegistration> _registrations =
            new Dictionary<RegistrationKey, IRegistration>(RegistrationKey.Comparer.Default);

        private Container([CanBeNull] IContainer parent = null)
        {
            _parent = parent;

            this.As<IContainerNode>().RegisterOnParent();
        }

        /// <summary>
        /// Creates a new instance of a root dependency injection container.
        /// </summary>
        /// <returns>A new empty container</returns>
        public static Container CreateRoot()
        {
            return new Container();
        }

        /// <summary>
        /// Create a new instance of a dependency injection container bound to
        /// a specified parent service locator.
        /// </summary>
        /// <param name="serviceLocator">Its parent container's service locator.</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">The <paramref name="serviceLocator"/> instance type must be a <see cref="Container"/> type.</exception>
        public static Container CreateWithParent([NotNull] IServiceLocator serviceLocator)
        {
            var parentContainer = serviceLocator as IContainer;

            if (parentContainer == null)
            {
                throw new InvalidOperationException(Constants.IncompatibleServiceLocator);
            }

            return new Container(parentContainer);
        }

        #region IContainerNode interface

        IContainer IContainerNode.Parent
        {
            get { return _parent; }
        }

        ISet<IContainer> IContainerNode.Children
        {
            get { return _children; }
        }

        void IContainerNode.RegisterOnParent()
        {
            _parent.Maybe(p => p.Children.Add(this));
        }

        void IContainerNode.UnregisterFromParent()
        {
            _parent.Maybe(p => p.Children.Remove(this));
        }

        #endregion

        #region IContainer interface

        IDictionary<RegistrationKey, IRegistration> IContainer.Registrations
        {
            get { return _registrations; }
        }

        IDictionary<RegistrationKey, PostponedAction> IContainer.PostponedActions
        {
            get { return _postponedActions; }
        }

        #endregion

        #region IBindingRoot

        public IBindingTo<TContract> Bind<TContract>([CanBeNull] string key)
            where TContract : class
        {
            return new BindingSyntax<TContract>(this, r => this.Register(RegistrationKey.From<TContract>(key), r));
        }

        #endregion

        #region IServiceLocator

        public bool CanResolve<TContract>([CanBeNull] string key, Scope scope)
            where TContract : class
        {
            return this.Contains(RegistrationKey.From<TContract>(key), scope);
        }

        public TContract Resolve<TContract>([CanBeNull] string key, Scope scope)
            where TContract : class
        {
            return this.First(RegistrationKey.From<TContract>(key), scope).Value as TContract;
        }

        public void Postpone<TContract>([CanBeNull] string key, Scope scope, [NotNull] Action<TContract> callback)
            where TContract : class
        {
            this.Postpone(RegistrationKey.From<TContract>(key), scope, o => callback.Invoke((TContract) o));
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                ContainerDisposeExtensions.Dispose(this);
            }
            else
            {
                Debug.WriteLine(Constants.FinalizerCalled);
            }
        }

        #endregion
    }
}