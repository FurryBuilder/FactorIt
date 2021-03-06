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

using FluffIt;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FactorIt.Tests
{
    [TestClass]
    public class GivenKeyedContainer
    {
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

            Assert.IsInstanceOfType(r, typeof (IStubService));
            Assert.IsInstanceOfType(r, typeof (StubService));
        }

        [TestMethod]
        public void WhenResolveDecorated_ThenReturnDecoratedService()
        {
            var sut = Container.CreateRoot();

            sut.Bind<IStubService>("test").To<StubService>(l => new StubService()).Decorate(
                (l, c) => new StubDecorator(c));

            var r = sut.Resolve<IStubService>("test", Scope.Default);

            Assert.IsInstanceOfType(r, typeof (IStubService));
            Assert.IsInstanceOfType(r, typeof (StubDecorator));
            Assert.IsInstanceOfType(r.As<StubDecorator>().Source, typeof (StubService));
        }

        [TestMethod]
        public void WhenLateAwait_ThenCallbackBlocking()
        {
            var isCalled = false;
            var sut = Container.CreateRoot();

            sut.Bind<IStubService>("test").To<StubService>(l => new StubService());

            sut.Postpone<IStubService>(
                "test",
                Scope.Default,
                c =>
                {
                    isCalled = true;
                    Assert.IsInstanceOfType(c, typeof (IStubService));
                    Assert.IsInstanceOfType(c, typeof (StubService));
                });

            Assert.IsTrue(isCalled);
        }

        [TestMethod]
        public void WhenEarlyAwait_ThenNoBlockAndCallbackOnRegister()
        {
            var isCalled = false;
            var sut = Container.CreateRoot();

            sut.Postpone<IStubService>(
                "test",
                Scope.Default,
                c =>
                {
                    isCalled = true;
                    Assert.IsInstanceOfType(c, typeof (IStubService));
                    Assert.IsInstanceOfType(c, typeof (StubService));
                });

            Assert.IsFalse(isCalled);

            sut.Bind<IStubService>("test").To<StubService>(l => new StubService());

            Assert.IsTrue(isCalled);
        }

        private interface IStubService
        {
        }

        private class StubService : IStubService
        {
        }

        private class StubDecorator : IStubService
        {
            public StubDecorator(IStubService source)
            {
                Source = source;
            }

            public IStubService Source { get; private set; }
        }
    }
}