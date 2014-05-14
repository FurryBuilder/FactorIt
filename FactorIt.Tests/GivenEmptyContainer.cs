using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FactorIt.Tests
{
	[TestClass]
	public class GivenEmptyContainer
	{
		private interface IStubService
		{ }

		[TestMethod]
		public void WhenCanResolve_ThenDeny()
		{
			var sut = Container.CreateRoot();

			var r = sut.CanResolve<IStubService>(null, Scope.Default);

			Assert.IsFalse(r);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void WhenResolve_ThenThrow()
		{
			var sut = Container.CreateRoot();

			sut.Resolve<IStubService>(null, Scope.Default);
		}

		[TestMethod]
		public void WhenAwait_ThenNoBlock()
		{
			var sut = Container.CreateRoot();

			sut.Postpone<IStubService>(null, Scope.Default, c => Assert.Fail());
		}

		[TestMethod]
		public void WhenAwaitTwice_ThenNoBlock()
		{
			var sut = Container.CreateRoot();

			sut.Postpone<IStubService>(null, Scope.Default, c => Assert.Fail());
			sut.Postpone<IStubService>(null, Scope.Default, c => Assert.Fail());
		}
	}
}