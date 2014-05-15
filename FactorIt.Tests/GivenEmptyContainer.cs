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