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

namespace FactorIt.Extensions
{
    public static class ServiceLocatorExtensions
    {
        /// <summary>
        /// Returns wether the service locator can provides an instance of a
        /// contract with no key and within the default scope.
        /// </summary>
        /// <typeparam name="TContract">Type of the contract to provide</typeparam>
        /// <param name="serviceLocator">The service locator to scan for contracts</param>
        public static bool CanResolve<TContract>([NotNull] this IServiceLocator serviceLocator)
            where TContract : class
        {
            return serviceLocator.CanResolve<TContract>(null, Scope.Default);
        }

        /// <summary>
        /// Returns wether the service locator can provides an instance of a
        /// contract matching the specified key and within the default scope.
        /// </summary>
        /// <typeparam name="TContract">Type of the contract to provide</typeparam>
        /// <param name="serviceLocator">The service locator to scan for contracts</param>
        /// <param name="key">Key associated to the contract</param>
        public static bool CanResolve<TContract>([NotNull] this IServiceLocator serviceLocator, [CanBeNull] string key)
            where TContract : class
        {
            return serviceLocator.CanResolve<TContract>(key, Scope.Default);
        }

        /// <summary>
        /// Returns wether the service locator can provides an instance of a
        /// contract with no key and within the specified scope.
        /// </summary>
        /// <typeparam name="TContract">Type of the contract to provide</typeparam>
        /// <param name="serviceLocator">The service locator to scan for contracts</param>
        /// <param name="scope">The range that should be used when looking for the contract.</param>
        public static bool CanResolve<TContract>([NotNull] this IServiceLocator serviceLocator, Scope scope)
            where TContract : class
        {
            return serviceLocator.CanResolve<TContract>(null, scope);
        }

        /// <summary>
        /// Provides an instance of a contract with no key and within the default scope.
        /// </summary>
        /// <typeparam name="TContract">Type of the contract to provide</typeparam>
        /// <param name="serviceLocator">The service locator providing the contracts</param>
        public static TContract Resolve<TContract>([NotNull] this IServiceLocator serviceLocator)
            where TContract : class
        {
            return serviceLocator.Resolve<TContract>(null, Scope.Default);
        }

        /// <summary>
        /// Provides an instance of a contract matching the specified key and
        /// within the default scope.
        /// </summary>
        /// <typeparam name="TContract">Type of the contract to provide</typeparam>
        /// <param name="serviceLocator">The service locator providing the contracts</param>
        /// <param name="key">Key associated to the contract</param>
        public static TContract Resolve<TContract>([NotNull] this IServiceLocator serviceLocator, [CanBeNull] string key)
            where TContract : class
        {
            return serviceLocator.Resolve<TContract>(key, Scope.Default);
        }

        /// <summary>
        /// Provides an instance of a contract with no key and within the specified scope.
        /// </summary>
        /// <typeparam name="TContract">Type of the contract to provide</typeparam>
        /// <param name="serviceLocator">The service locator providing the contracts</param>
        /// <param name="scope">The range that should be used when looking for the contract.</param>
        public static TContract Resolve<TContract>([NotNull] this IServiceLocator serviceLocator, Scope scope)
            where TContract : class
        {
            return serviceLocator.Resolve<TContract>(null, scope);
        }

        /// <summary>
        /// Provides an instance of a contract with no  key and within the default
        /// scope or a default value if none can be found.
        /// </summary>
        /// <typeparam name="TContract">Type of the contract to provide</typeparam>
        /// <param name="serviceLocator">The service locator provinding the contracts</param>
        /// <returns>
        /// A service instance from the container associated to this service
        /// locator or a default service instance if none can be found.
        /// </returns>
        public static TContract ResolveOrDefault<TContract>([NotNull] this IServiceLocator serviceLocator)
            where TContract : class
        {
            return serviceLocator.ResolveOrDefault(null, Scope.Default, () => default(TContract));
        }

        /// <summary>
        /// Provides an instance of a contract matching the specified key and
        /// within the default scope or a default value if none can be found.
        /// </summary>
        /// <typeparam name="TContract">Type of the contract to provide</typeparam>
        /// <param name="serviceLocator">The service locator provinding the contracts</param>
        /// <param name="key">Key associated to the contract</param>
        /// <returns>
        /// A service instance from the container associated to this service
        /// locator or a default service instance if none can be found.
        /// </returns>
        public static TContract ResolveOrDefault<TContract>(
            [NotNull] this IServiceLocator serviceLocator,
            [CanBeNull] string key)
            where TContract : class
        {
            return serviceLocator.ResolveOrDefault(key, Scope.Default, () => default(TContract));
        }

        /// <summary>
        /// Provides an instance of a contract with no key key and within the
        /// specified scope or a default value if none can be found.
        /// </summary>
        /// <typeparam name="TContract">Type of the contract to provide</typeparam>
        /// <param name="serviceLocator">The service locator provinding the contracts</param>
        /// <param name="scope">The range that should be user when looking for the contract</param>
        /// <returns>
        /// A service instance from the container associated to this service
        /// locator or a default service instance if none can be found.
        /// </returns>
        public static TContract ResolveOrDefault<TContract>([NotNull] this IServiceLocator serviceLocator, Scope scope)
            where TContract : class
        {
            return serviceLocator.ResolveOrDefault(null, scope, () => default(TContract));
        }

        /// <summary>
        /// Provides an instance of a contract with no key key and within the
        /// default scope or a default value if none can be found.
        /// </summary>
        /// <typeparam name="TContract">Type of the contract to provide</typeparam>
        /// <param name="serviceLocator">The service locator provinding the contracts</param>
        /// <param name="defaultFactory">A factory to initialize the default value.</param>
        /// <returns>
        /// A service instance from the container associated to this service
        /// locator or a default service instance if none can be found.
        /// </returns>
        /// <exception cref="Exception">A delegate callback throws an exception. </exception>
        public static TContract ResolveOrDefault<TContract>(
            [NotNull] this IServiceLocator serviceLocator,
            [NotNull] Func<TContract> defaultFactory)
            where TContract : class
        {
            return serviceLocator.ResolveOrDefault(null, Scope.Default, defaultFactory);
        }

        /// <summary>
        /// Provides an instance of a contract matching the specified key and
        /// within the default scope or a default value if none can be found.
        /// </summary>
        /// <typeparam name="TContract">Type of the contract to provide</typeparam>
        /// <param name="serviceLocator">The service locator provinding the contracts</param>
        /// <param name="key">Key associated to the contract</param>
        /// <param name="defaultFactory">A factory to initialize the default value.</param>
        /// <returns>
        /// A service instance from the container associated to this service
        /// locator or a default service instance if none can be found.
        /// </returns>
        /// <exception cref="Exception">A delegate callback throws an exception. </exception>
        public static TContract ResolveOrDefault<TContract>(
            [NotNull] this IServiceLocator serviceLocator,
            [CanBeNull] string key,
            [NotNull] Func<TContract> defaultFactory)
            where TContract : class
        {
            if (serviceLocator == null)
            {
                return defaultFactory.Invoke();
            }

            return serviceLocator.CanResolve<TContract>(key, Scope.Default)
                ? serviceLocator.Resolve<TContract>(key, Scope.Default)
                : defaultFactory.Invoke();
        }

        /// <summary>
        /// Provides an instance of a contract with no key and within the
        /// specified scope or a default value if none can be found.
        /// </summary>
        /// <typeparam name="TContract">Type of the contract to provide</typeparam>
        /// <param name="serviceLocator">The service locator provinding the contracts</param>
        /// <param name="scope">The range that should be user when looking for the contract</param>
        /// <param name="defaultFactory">A factory to initialize the default value.</param>
        /// <returns>
        /// A service instance from the container associated to this service
        /// locator or a default service instance if none can be found.
        /// </returns>
        /// <exception cref="Exception">A delegate callback throws an exception. </exception>
        public static TContract ResolveOrDefault<TContract>(
            [NotNull] this IServiceLocator serviceLocator,
            Scope scope,
            [NotNull] Func<TContract> defaultFactory)
            where TContract : class
        {
            if (serviceLocator == null)
            {
                return defaultFactory.Invoke();
            }

            return serviceLocator.CanResolve<TContract>(null, scope)
                ? serviceLocator.Resolve<TContract>(null, scope)
                : defaultFactory.Invoke();
        }

        /// <summary>
        /// Provides an instance of a contract matching the specified key and
        /// within the specified scope or a default value if none can be found.
        /// </summary>
        /// <typeparam name="TContract">Type of the contract to provide</typeparam>
        /// <param name="serviceLocator">The service locator provinding the contracts</param>
        /// <param name="key">Key associated to the contract</param>
        /// <param name="scope">The range that should be user when looking for the contract</param>
        /// <param name="defaultFactory">A factory to initialize the default value.</param>
        /// <returns>
        /// A service instance from the container associated to this service
        /// locator or a default service instance if none can be found.
        /// </returns>
        /// <exception cref="Exception">A delegate callback throws an exception. </exception>
        public static TContract ResolveOrDefault<TContract>(
            [NotNull] this IServiceLocator serviceLocator,
            [CanBeNull] string key,
            Scope scope,
            [NotNull] Func<TContract> defaultFactory)
            where TContract : class
        {
            if (serviceLocator == null)
            {
                return defaultFactory.Invoke();
            }

            return serviceLocator.CanResolve<TContract>(key, scope)
                ? serviceLocator.Resolve<TContract>(key, scope)
                : defaultFactory.Invoke();
        }

        /// <summary>
        /// Schedule a callback to be executed once the service locator can
        /// provide an instance of a contract with no key and within the
        /// default scope.
        /// </summary>
        /// <typeparam name="TContract">Type of the contract to provide</typeparam>
        /// <param name="serviceLocator">The service locator providing the contracts</param>
        /// <param name="callback">A action to execute requiring a specific service instance that is not yet available.</param>
        public static void Postpone<TContract>(
            [NotNull] this IServiceLocator serviceLocator,
            [NotNull] Action<TContract> callback)
            where TContract : class
        {
            serviceLocator.Postpone(null, Scope.Default, callback);
        }

        /// <summary>
        /// Schedule a callback to be executed once the service locator can
        /// provide an instance of a contract matching the specified key and
        /// withing the default scope.
        /// </summary>
        /// <typeparam name="TContract">Type of the contract to provide</typeparam>
        /// <param name="serviceLocator">The service locator providing the contracts</param>
        /// <param name="key">Key associated to the contract</param>
        /// <param name="callback">A action to execute requiring a specific service instance that is not yet available.</param>
        public static void Postpone<TContract>(
            [NotNull] this IServiceLocator serviceLocator,
            [CanBeNull] string key,
            [NotNull] Action<TContract> callback)
            where TContract : class
        {
            serviceLocator.Postpone(key, Scope.Default, callback);
        }

        /// <summary>
        /// Schedule a callback to be executed once the service locator can
        /// provide an instance of a contract with no key and within the
        /// specified scope.
        /// </summary>
        /// <typeparam name="TContract">Type of the contract to provide</typeparam>
        /// <param name="serviceLocator">The service locator providing the contracts</param>
        /// <param name="scope">The range that should be used when looking for the contract.</param>
        /// <param name="callback">A action to execute requiring a specific service instance that is not yet available.</param>
        public static void Postpone<TContract>(
            [NotNull] this IServiceLocator serviceLocator,
            Scope scope,
            [NotNull] Action<TContract> callback)
            where TContract : class
        {
            serviceLocator.Postpone(null, scope, callback);
        }
    }
}