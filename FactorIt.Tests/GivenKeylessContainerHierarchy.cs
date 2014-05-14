using FactorIt.Contracts;
using FactorIt.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FactorIt.Tests
{
	[TestClass]
	public class GivenKeylessContainerHierarchy
	{
		private interface IStubService
		{ }

		private interface IStubService2
		{ }

		private class StubService : IStubService
		{
			public IServiceLocator ServiceLocator { get; private set; }

			public StubService(IServiceLocator serviceLocator)
			{
				ServiceLocator = serviceLocator;
			}
		}

		private class StubService2 : IStubService2
		{ }

		[TestMethod]
		public void WhenRegisterLocally_ThenCanResolveLocally()
		{
			var parentSut = Container.CreateRoot();
			var sut = Container.CreateWithParent(parentSut);
			var childrenSut = Container.CreateWithParent(sut);

			sut.Bind<IStubService>(null).To<StubService>(l => new StubService(l));

			var canLocal = sut.CanResolve<IStubService>(null, Scope.Local);
			var canParent = sut.CanResolve<IStubService>(null, Scope.Parent);
			var canChildren = sut.CanResolve<IStubService>(null, Scope.Children);

			Assert.IsTrue(canLocal);
			Assert.IsFalse(canParent);
			Assert.IsFalse(canChildren);
		}

		[TestMethod]
		public void WhenRegisterOnParent_ThenCanResolveFromParent()
		{
			var parentSut = Container.CreateRoot();
			var sut = Container.CreateWithParent(parentSut);
			var childrenSut = Container.CreateWithParent(sut);

			parentSut.Bind<IStubService>(null).To<StubService>(l => new StubService(l));

			var canLocal = sut.CanResolve<IStubService>(null, Scope.Local);
			var canParent = sut.CanResolve<IStubService>(null, Scope.Parent);
			var canChildren = sut.CanResolve<IStubService>(null, Scope.Children);

			Assert.IsFalse(canLocal);
			Assert.IsTrue(canParent);
			Assert.IsFalse(canChildren);
		}

		[TestMethod]
		public void WhenRegisterOnChild_ThenCanResolveFromChild()
		{
			var parentSut = Container.CreateRoot();
			var sut = Container.CreateWithParent(parentSut);
			var childrenSut = Container.CreateWithParent(sut);

			childrenSut.Bind<IStubService>(null).To<StubService>(l => new StubService(l));

			var canLocal = sut.CanResolve<IStubService>(null, Scope.Local);
			var canParent = sut.CanResolve<IStubService>(null, Scope.Parent);
			var canChildren = sut.CanResolve<IStubService>(null, Scope.Children);

			Assert.IsFalse(canLocal);
			Assert.IsFalse(canParent);
			Assert.IsTrue(canChildren);
		}

		[TestMethod]
		public void WhenRegisterLocallyAndParent_ThenCanResolveLocallyAndParent()
		{
			var parentSut = Container.CreateRoot();
			var sut = Container.CreateWithParent(parentSut);
			var childrenSut = Container.CreateWithParent(sut);

			parentSut.Bind<IStubService>(null).To<StubService>(l => new StubService(l));
			sut.Bind<IStubService>(null).To<StubService>(l => new StubService(l));

			var canLocal = sut.CanResolve<IStubService>(null, Scope.Local);
			var canParent = sut.CanResolve<IStubService>(null, Scope.Parent);
			var canChildren = sut.CanResolve<IStubService>(null, Scope.Children);

			Assert.IsTrue(canLocal);
			Assert.IsTrue(canParent);
			Assert.IsFalse(canChildren);
		}

		[TestMethod]
		public void WhenRegisterLocallyAndChild_ThenCanResolveLocallyAndChild()
		{
			var parentSut = Container.CreateRoot();
			var sut = Container.CreateWithParent(parentSut);
			var childrenSut = Container.CreateWithParent(sut);

			sut.Bind<IStubService>(null).To<StubService>(l => new StubService(l));
			childrenSut.Bind<IStubService>(null).To<StubService>(l => new StubService(l));

			var canLocal = sut.CanResolve<IStubService>(null, Scope.Local);
			var canParent = sut.CanResolve<IStubService>(null, Scope.Parent);
			var canChildren = sut.CanResolve<IStubService>(null, Scope.Children);

			Assert.IsTrue(canLocal);
			Assert.IsFalse(canParent);
			Assert.IsTrue(canChildren);
		}

		[TestMethod]
		public void WhenRegisterParentAndChild_ThenCanResolveParentAndChild()
		{
			var parentSut = Container.CreateRoot();
			var sut = Container.CreateWithParent(parentSut);
			var childrenSut = Container.CreateWithParent(sut);

			parentSut.Bind<IStubService>(null).To<StubService>(l => new StubService(l));
			childrenSut.Bind<IStubService>(null).To<StubService>(l => new StubService(l));

			var canLocal = sut.CanResolve<IStubService>(null, Scope.Local);
			var canParent = sut.CanResolve<IStubService>(null, Scope.Parent);
			var canChildren = sut.CanResolve<IStubService>(null, Scope.Children);

			Assert.IsFalse(canLocal);
			Assert.IsTrue(canParent);
			Assert.IsTrue(canChildren);
		}

		[TestMethod]
		public void WhenRegisterTwoParentDeep_ThenCanResolveParent()
		{
			var parentSutDeep = Container.CreateRoot();
			var parentSut = Container.CreateWithParent(parentSutDeep);
			var sut = Container.CreateWithParent(parentSut);
			var childrenSut = Container.CreateWithParent(sut);

			parentSutDeep.Bind<IStubService>(null).To<StubService>(l => new StubService(l));

			var canLocal = sut.CanResolve<IStubService>(null, Scope.Local);
			var canParent = sut.CanResolve<IStubService>(null, Scope.Parent);
			var canChildren = sut.CanResolve<IStubService>(null, Scope.Children);

			Assert.IsFalse(canLocal);
			Assert.IsTrue(canParent);
			Assert.IsFalse(canChildren);
		}

		[TestMethod]
		public void WhenRegisterTwoChildDeep_ThenCanResolveChild()
		{
			var parentSut = Container.CreateRoot();
			var sut = Container.CreateWithParent(parentSut);
			var childrenSut = Container.CreateWithParent(sut);
			var childrenSutDeep = Container.CreateWithParent(childrenSut);

			childrenSutDeep.Bind<IStubService>(null).To<StubService>(l => new StubService(l));

			var canLocal = sut.CanResolve<IStubService>(null, Scope.Local);
			var canParent = sut.CanResolve<IStubService>(null, Scope.Parent);
			var canChildren = sut.CanResolve<IStubService>(null, Scope.Children);

			Assert.IsFalse(canLocal);
			Assert.IsFalse(canParent);
			Assert.IsTrue(canChildren);
		}

		[TestMethod]
		public void WhenRegisterLocally_ThenResolveLocally()
		{
			var parentSut = Container.CreateRoot();
			var sut = Container.CreateWithParent(parentSut);
			var childrenSut = Container.CreateWithParent(sut);

			sut.Bind<IStubService>(null).To<StubService>(l => new StubService(l));

			var r = sut.Resolve<IStubService>(null, Scope.Local);

			Assert.AreSame(sut, r.As<StubService>().ServiceLocator);
		}

		[TestMethod]
		public void WhenRegisterOnParent_ThenResolveFromParent()
		{
			var parentSut = Container.CreateRoot();
			var sut = Container.CreateWithParent(parentSut);
			var childrenSut = Container.CreateWithParent(sut);

			parentSut.Bind<IStubService>(null).To<StubService>(l => new StubService(l));

			var r = sut.Resolve<IStubService>(null, Scope.Parent);

			Assert.AreSame(parentSut, r.As<StubService>().ServiceLocator);
		}

		[TestMethod]
		public void WhenRegisterOnChild_ThenResolveFromChild()
		{
			var parentSut = Container.CreateRoot();
			var sut = Container.CreateWithParent(parentSut);
			var childrenSut = Container.CreateWithParent(sut);

			childrenSut.Bind<IStubService>(null).To<StubService>(l => new StubService(l));

			var r = sut.Resolve<IStubService>(null, Scope.Children);

			Assert.AreSame(childrenSut, r.As<StubService>().ServiceLocator);
		}

		[TestMethod]
		public void WhenRegisterLocallyAndParent_ThenResolveLocally()
		{
			var parentSut = Container.CreateRoot();
			var sut = Container.CreateWithParent(parentSut);
			var childrenSut = Container.CreateWithParent(sut);

			parentSut.Bind<IStubService>(null).To<StubService>(l => new StubService(l));
			sut.Bind<IStubService>(null).To<StubService>(l => new StubService(l));

			var r = sut.Resolve<IStubService>(null, Scope.All);

			Assert.AreSame(sut, r.As<StubService>().ServiceLocator);
		}

		[TestMethod]
		public void WhenRegisterLocallyAndChild_ThenResolveLocally()
		{
			var parentSut = Container.CreateRoot();
			var sut = Container.CreateWithParent(parentSut);
			var childrenSut = Container.CreateWithParent(sut);

			sut.Bind<IStubService>(null).To<StubService>(l => new StubService(l));
			childrenSut.Bind<IStubService>(null).To<StubService>(l => new StubService(l));

			var r = sut.Resolve<IStubService>(null, Scope.All);

			Assert.AreSame(sut, r.As<StubService>().ServiceLocator);
		}

		[TestMethod]
		public void WhenRegisterParentAndChild_ThenResolveParent()
		{
			var parentSut = Container.CreateRoot();
			var sut = Container.CreateWithParent(parentSut);
			var childrenSut = Container.CreateWithParent(sut);

			parentSut.Bind<IStubService>(null).To<StubService>(l => new StubService(l));
			childrenSut.Bind<IStubService>(null).To<StubService>(l => new StubService(l));

			var r = sut.Resolve<IStubService>(null, Scope.All);

			Assert.AreSame(parentSut, r.As<StubService>().ServiceLocator);
		}

		[TestMethod]
		public void WhenRegisterParentAndChild_ThenResolveChild()
		{
			var parentSut = Container.CreateRoot();
			var sut = Container.CreateWithParent(parentSut);
			var childrenSut = Container.CreateWithParent(sut);

			parentSut.Bind<IStubService>(null).To<StubService>(l => new StubService(l));
			childrenSut.Bind<IStubService>(null).To<StubService>(l => new StubService(l));

			var r = sut.Resolve<IStubService>(null, Scope.Downward);

			Assert.AreSame(childrenSut, r.As<StubService>().ServiceLocator);
		}

		[TestMethod]
		public void WhenRegisterTwoParentDeep_ThenResolveParentDeep()
		{
			var parentSutDeep = Container.CreateRoot();
			var parentSut = Container.CreateWithParent(parentSutDeep);
			var sut = Container.CreateWithParent(parentSut);
			var childrenSut = Container.CreateWithParent(sut);

			parentSutDeep.Bind<IStubService>(null).To<StubService>(l => new StubService(l));

			var r = sut.Resolve<IStubService>(null, Scope.Parent);

			Assert.AreSame(parentSutDeep, r.As<StubService>().ServiceLocator);
		}

		[TestMethod]
		public void WhenRegisterTwoChildDeep_ThenResolveChildDeep()
		{
			var parentSut = Container.CreateRoot();
			var sut = Container.CreateWithParent(parentSut);
			var childrenSut = Container.CreateWithParent(sut);
			var childrenSutDeep = Container.CreateWithParent(childrenSut);

			childrenSutDeep.Bind<IStubService>(null).To<StubService>(l => new StubService(l));

			var r = sut.Resolve<IStubService>(null, Scope.Children);

			Assert.AreSame(childrenSutDeep, r.As<StubService>().ServiceLocator);
		}

		[TestMethod]
		public void WhenPostponeLocallyAndRegisterLocally_ThenExecute()
		{
			var parentSut = Container.CreateRoot();
			var sut = Container.CreateWithParent(parentSut);
			var childrenSut = Container.CreateWithParent(sut);

			var isCalled = false;

			sut.Postpone<IStubService>(null, Scope.Local, c => isCalled = true);

			sut.Bind<IStubService>(null).To<StubService>(l => new StubService(l));

			Assert.IsTrue(isCalled);
		}

		[TestMethod]
		public void WhenPostponeOnParentAndRegisterLocally_ThenSkip()
		{
			var parentSut = Container.CreateRoot();
			var sut = Container.CreateWithParent(parentSut);
			var childrenSut = Container.CreateWithParent(sut);

			var isCalled = false;

			sut.Postpone<IStubService>(null, Scope.Parent, c => isCalled = true);

			sut.Bind<IStubService>(null).To<StubService>(l => new StubService(l));

			Assert.IsFalse(isCalled);
		}

		[TestMethod]
		public void WhenPostponeOnChildAndRegisterLocally_ThenSkip()
		{
			var parentSut = Container.CreateRoot();
			var sut = Container.CreateWithParent(parentSut);
			var childrenSut = Container.CreateWithParent(sut);

			var isCalled = false;

			sut.Postpone<IStubService>(null, Scope.Children, c => isCalled = true);

			sut.Bind<IStubService>(null).To<StubService>(l => new StubService(l));

			Assert.IsFalse(isCalled);
		}

		[TestMethod]
		public void WhenPostponeLocallyAndRegisterOnParent_ThenSkip()
		{
			var parentSut = Container.CreateRoot();
			var sut = Container.CreateWithParent(parentSut);
			var childrenSut = Container.CreateWithParent(sut);

			var isCalled = false;

			sut.Postpone<IStubService>(null, Scope.Local, c => isCalled = true);

			parentSut.Bind<IStubService>(null).To<StubService>(l => new StubService(l));

			Assert.IsFalse(isCalled);
		}

		[TestMethod]
		public void WhenPostponeOnParentAndRegisterOnParent_ThenExecute()
		{
			var parentSut = Container.CreateRoot();
			var sut = Container.CreateWithParent(parentSut);
			var childrenSut = Container.CreateWithParent(sut);

			var isCalled = false;

			sut.Postpone<IStubService>(null, Scope.Parent, c => isCalled = true);

			parentSut.Bind<IStubService>(null).To<StubService>(l => new StubService(l));

			Assert.IsTrue(isCalled);
		}

		[TestMethod]
		public void WhenPostponeOnChildAndRegisterOnParent_ThenSkip()
		{
			var parentSut = Container.CreateRoot();
			var sut = Container.CreateWithParent(parentSut);
			var childrenSut = Container.CreateWithParent(sut);

			var isCalled = false;

			sut.Postpone<IStubService>(null, Scope.Children, c => isCalled = true);

			parentSut.Bind<IStubService>(null).To<StubService>(l => new StubService(l));

			Assert.IsFalse(isCalled);
		}

		[TestMethod]
		public void WhenPostponeLocallyAndRegisterOnChild_ThenSkip()
		{
			var parentSut = Container.CreateRoot();
			var sut = Container.CreateWithParent(parentSut);
			var childrenSut = Container.CreateWithParent(sut);

			var isCalled = false;

			sut.Postpone<IStubService>(null, Scope.Local, c => isCalled = true);

			childrenSut.Bind<IStubService>(null).To<StubService>(l => new StubService(l));

			Assert.IsFalse(isCalled);
		}

		[TestMethod]
		public void WhenPostponeOnParentAndRegisterOnChild_ThenSkip()
		{
			var parentSut = Container.CreateRoot();
			var sut = Container.CreateWithParent(parentSut);
			var childrenSut = Container.CreateWithParent(sut);

			var isCalled = false;

			sut.Postpone<IStubService>(null, Scope.Parent, c => isCalled = true);

			childrenSut.Bind<IStubService>(null).To<StubService>(l => new StubService(l));

			Assert.IsFalse(isCalled);
		}

		[TestMethod]
		public void WhenPostponeOnChildAndRegisterOnChild_ThenExecute()
		{
			var parentSut = Container.CreateRoot();
			var sut = Container.CreateWithParent(parentSut);
			var childrenSut = Container.CreateWithParent(sut);

			var isCalled = false;

			sut.Postpone<IStubService>(null, Scope.Children, c => isCalled = true);

			childrenSut.Bind<IStubService>(null).To<StubService>(l => new StubService(l));

			Assert.IsTrue(isCalled);
		}

		[TestMethod]
		public void WhenPostponeOnParentAndLocallyAndRegisterLocally_ThenExecuteLocally()
		{
			var parentSut = Container.CreateRoot();
			var sut = Container.CreateWithParent(parentSut);
			var childrenSut = Container.CreateWithParent(sut);

			var isParentCalled = false;
			var isLocalCalled = false;

			parentSut.Postpone<IStubService>(null, Scope.Local, c => isParentCalled = true);
			sut.Postpone<IStubService>(null, Scope.Local, c => isLocalCalled = true);

			sut.Bind<IStubService>(null).To<StubService>(l => new StubService(l));

			Assert.IsFalse(isParentCalled);
			Assert.IsTrue(isLocalCalled);
		}

		[TestMethod]
		public void WhenPostponeOnParentAndLocallyUsingChildrenAndRegisterLocally_ThenExecuteOnParentAndLocally()
		{
			var parentSut = Container.CreateRoot();
			var sut = Container.CreateWithParent(parentSut);
			var childrenSut = Container.CreateWithParent(sut);

			var isParentCalled = false;
			var isLocalCalled = false;

			parentSut.Postpone<IStubService>(null, Scope.Children, c => isParentCalled = true);
			sut.Postpone<IStubService>(null, Scope.Local, c => isLocalCalled = true);

			sut.Bind<IStubService>(null).To<StubService>(l => new StubService(l));

			Assert.IsTrue(isParentCalled);
			Assert.IsTrue(isLocalCalled);
		}
	}
}