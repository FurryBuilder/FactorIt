﻿using System;
using System.Collections.Generic;

using FactorIt.Extensions;
using FactorIt.Contracts;

namespace FactorIt
{
	public sealed class Container : IContainer, IBindingRoot, IServiceLocator, IDisposable
	{
		public static class Constants
		{
			public const string IncompatibleServiceLocator = "Trying to create a child container from an incompatible IServiceLocator. Please use an IServiceLocator coming from this IoC container library.";
			public const string ContractAlreadyRegistered = "Contract with key \"{0}\" already registered";
			public const string ContractNotRegistered = "Contract with key \"{0}\" not registered";
			public const string FinalizerCalled = "An instance of scope has been finalized without being disposed. It's services and children Scopes will not be disposed.";
		}

		private readonly IContainer _parent;

		private readonly HashSet<IContainer> _children =
			new HashSet<IContainer>();

		private readonly Dictionary<RegistrationKey, IRegistration> _registrations =
			new Dictionary<RegistrationKey, IRegistration>(RegistrationKey.Comparer.Default);

		private readonly Dictionary<RegistrationKey, PostponedAction> _postponedActions =
			new Dictionary<RegistrationKey, PostponedAction>(RegistrationKey.Comparer.Default);
		
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

		public static Container CreateRoot()
		{
			return new Container();
		}

		public static Container CreateWithParent([NotNull] IServiceLocator serviceLocator)
		{
			var parentContainer = serviceLocator as IContainer;

			if (parentContainer == null)
			{
				throw new InvalidOperationException(Constants.IncompatibleServiceLocator);
			}

			return new Container(parentContainer);
		}

		private Container([CanBeNull] IContainer parent = null)
		{
			_parent = parent;

			this.As<IContainer>().RegisterOnParent();
		}

		public IBindingTo<TContract> Bind<TContract>([CanBeNull] string key)
			where TContract : class
		{
			return new BindingSyntax<TContract>(this, r => this.Register(RegistrationKey.From<TContract>(key), r));
		}

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
			this.Postpone(RegistrationKey.From<TContract>(key), scope, o => callback.Invoke((TContract)o));
		}

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
				System.Diagnostics.Debug.WriteLine(Constants.FinalizerCalled);
			}
		}
	}
}