using FactorIt.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FactorIt.Tests
{
	[TestClass]
	public class GivenKeyedContainer
	{
		private interface IStubService
		{ }

		private class StubService : IStubService
		{ }

		private class StubDecorator : IStubService
		{
			public IStubService Source { get; private set; }

			public StubDecorator(IStubService source)
			{
				Source = source;
			}
		}

		[TestMethod]
		public void WhenCanResolve_ThenAllow()
		{
			var sut = Container.CreateRoot();

			sut.Bind<IStubService>("test").To<StubService>(l => new StubService());

			var r = sut.CanResolve<IStubService>("test", Scope.Default);

			Assert.IsTrue(r);
		}

		[TestMethod]
		public void WhenCanResolveKeyless_ThenDeny()
		{
			var sut = Container.CreateRoot();

			sut.Bind<IStubService>("test").To<StubService>(l => new StubService());

			var r = sut.CanResolve<IStubService>(null, Scope.Default);

			Assert.IsFalse(r);
		}

		[TestMethod]
		public void WhenResolve_ThenReturnService()
		{
			var sut = Container.CreateRoot();

			sut.Bind<IStubService>("test").To<StubService>(l => new StubService());

			var r = sut.Resolve<IStubService>("test", Scope.Default);

			Assert.IsInstanceOfType(r, typeof(IStubService));
			Assert.IsInstanceOfType(r, typeof(StubService));
		}

		[TestMethod]
		public void WhenResolveDecorated_ThenReturnDecoratedService()
		{
			var sut = Container.CreateRoot();

			sut.Bind<IStubService>("test").To<StubService>(l => new StubService()).Decorate((l, c) => new StubDecorator(c));

			var r = sut.Resolve<IStubService>("test", Scope.Default);

			Assert.IsInstanceOfType(r, typeof(IStubService));
			Assert.IsInstanceOfType(r, typeof(StubDecorator));
			Assert.IsInstanceOfType(r.As<StubDecorator>().Source, typeof(StubService));
		}

		[TestMethod]
		public void WhenLateAwait_ThenCallbackBlocking()
		{
			var isCalled = false;
			var sut = Container.CreateRoot();

			sut.Bind<IStubService>("test").To<StubService>(l => new StubService());

			sut.Postpone<IStubService>("test", Scope.Default, c =>
			{
				isCalled = true;
				Assert.IsInstanceOfType(c, typeof(IStubService));
				Assert.IsInstanceOfType(c, typeof(StubService));
			});

			Assert.IsTrue(isCalled);
		}

		[TestMethod]
		public void WhenEarlyAwait_ThenNoBlockAndCallbackOnRegister()
		{
			var isCalled = false;
			var sut = Container.CreateRoot();

			sut.Postpone<IStubService>("test", Scope.Default, c =>
			{
				isCalled = true;
				Assert.IsInstanceOfType(c, typeof(IStubService));
				Assert.IsInstanceOfType(c, typeof(StubService));
			});

			Assert.IsFalse(isCalled);

			sut.Bind<IStubService>("test").To<StubService>(l => new StubService());

			Assert.IsTrue(isCalled);
		}
	}
}