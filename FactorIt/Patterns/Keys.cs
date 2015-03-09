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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace FactorIt.Patterns
{
    [SuppressMessage("Microsoft.Usage", "CA2237:MarkISerializableTypesWithSerializable",
        Justification = "Not using built-in serialization to stay cross-platform friendly")]
    public class RequireEqualityComparerException<TSelf> : Exception
    {
        public RequireEqualityComparerException()
            : base(string.Format(Constants.UseEqualityComparer, typeof (TSelf).FullName))
        {
        }

        public static class Constants
        {
            public const string UseEqualityComparer =
                "An IEqualityComparer implementation is available for the type: {0} and must be used instead.";
        }
    }

    /// <summary>
    /// Base class for a key pattern that can be used inside collections to
    /// uniquely identify elements.
    /// </summary>
    /// <typeparam name="TKey">Type of the key</typeparam>
    public abstract class KeyBase<TKey>
    {
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations",
            Justification =
                "The Key pattern requires the use of an IEqualityComparer. Equals should never be called directly"
            )]
        public override bool Equals([NotNull] object obj)
        {
            throw new RequireEqualityComparerException<TKey>();
        }

        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations",
            Justification =
                "The Key patterns requires the use of an IEqualityComparer. GetHashCode should never be called directly"
            )]
        public override int GetHashCode()
        {
            throw new RequireEqualityComparerException<TKey>();
        }

        protected abstract string GetStringValues();

        public override string ToString()
        {
            return GetStringValues();
        }

        /// <summary>
        /// Base class for all key comparers.
        /// </summary>
        /// <typeparam name="TComparer">Type of the specialized comparer.</typeparam>
        public abstract class KeyComparerBase<TComparer> : DefaultProvider<TComparer>, IEqualityComparer<TKey>
            where TComparer : new()
        {
            public abstract bool Equals([NotNull] TKey x, [NotNull] TKey y);
            public abstract int GetHashCode([NotNull] TKey obj);
        }
    }
}